using ApniDukaan.Core.RepositoryContracts;
using ApniDukaan.Infrastructure.DBContext;
using ApniDukaan.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApniDukaan.Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Extension methods for registering infrastructure services in the dependency injection container.
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO: Add your services to the IoC container
            // Infrastructure services often include things like database contexts/access,
            // caching, file storage services and other low level components etc.

            var connectionStringTemplate = configuration.GetConnectionString("ApplicationUsersSqlConnection")!;

            var host = Environment.GetEnvironmentVariable("MSSQL_HOST") ?? string.Empty;
            var port = Environment.GetEnvironmentVariable("MSSQL_PORT") ?? "1434"; // Default SQL Server port
            var database = Environment.GetEnvironmentVariable("MSSQL_DATABASE") ?? string.Empty;
            var user = Environment.GetEnvironmentVariable("MSSQL_USER") ?? string.Empty;
            var password = Environment.GetEnvironmentVariable("MSSQL_PASSWORD") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(port) ||
                string.IsNullOrWhiteSpace(database) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("One or more MSSQL_* environment variables are missing. Ensure the selected launch profile provides MSSQL_HOST, MSSQL_DATABASE, MSSQL_USER and MSSQL_PASSWORD.");
            }

            var connectionString = connectionStringTemplate
                .Replace("$MSSQL_HOST", host)
                .Replace("$MSSQL_PORT", port)
                .Replace("$MSSQL_DATABASE", database)
                .Replace("$MSSQL_USER", user)
                .Replace("$MSSQL_PASSWORD", password);

            services.AddDbContext<ApplicationUserDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
            });

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
