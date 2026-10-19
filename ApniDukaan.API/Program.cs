using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using ApniDukaan.Infrastructure;
using ApniDukaan.Core;
using ApniDukaan.API.Middleware;
using System.Text.Json.Serialization;
using ApniDukaan.Core.Mappers;
using Microsoft.EntityFrameworkCore;
using ApniDukaan.Infrastructure.DBContext;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Adding infrastructure services to the dependency injection container using the extension method defined in the Infrastructure project.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddCoreServices();

// Add Controllers to the Service collections
builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
        };
    });

// Adding AutoMapper service
builder.Services.AddAutoMapper(typeof(ApplicationUserToAuthenticationResponseMappingProfile).Assembly); // Adding one profile class automatically takes all profile classes in the assembly and registers them with AutoMapper.

// Fluent Validation
builder.Services.AddFluentValidationAutoValidation();

// Add API explorer
builder.Services.AddEndpointsApiExplorer();

// Add Swagger generation for API documentation and testing
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT bearer token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add CORS policy (configure as needed)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200") // TODO: Replace this with frontend URL
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Build the Web application
var app = builder.Build();

// Apply migrations in development or when explicitly enabled via environment variable.
// In production, migrations should be applied as a separate deployment step (CI/CD or init container).
//using (var scope = app.Services.CreateScope())
//{
//    var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
//    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
//    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationUserDbContext>();

//    var enableAutoMigrate = env.IsDevelopment() ||
//                            Environment.GetEnvironmentVariable("ENABLE_AUTO_MIGRATE") == "true";

//    if (enableAutoMigrate)
//    {
//        try
//        {
//            logger.LogInformation("Applying EF Core migrations (auto-migrate enabled).");
//            dbContext.Database.Migrate();
//        }
//        catch (Exception ex)
//        {
//            logger.LogError(ex, "Automatic migration failed.");
//            throw;
//        }
//    }
//}

// Adding Exception Handling Middleware to the HTTP request pipeline using the extension method defined in the API project.
app.UseExceptionHandlingMiddleware();

// Routing
app.UseRouting();

// CORS middleware to allow cross-origin requests (configure as needed)
app.UseCors();

// Swagger middleware for API documentation and testing
app.UseSwagger();
// Swagger UI middleware for interactive API documentation
app.UseSwaggerUI();

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Controller routes
app.MapControllers();

app.Run();
