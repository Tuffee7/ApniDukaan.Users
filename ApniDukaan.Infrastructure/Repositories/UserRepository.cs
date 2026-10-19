using ApniDukaan.Core.Common;
using ApniDukaan.Core.Entities;
using ApniDukaan.Core.RepositoryContracts;
using ApniDukaan.Infrastructure.DBContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ApniDukaan.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationUserDbContext _dbContext;

        public UserRepository(ApplicationUserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApplicationUser?> AddUser(ApplicationUser user)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Email == user.Email);
            if (userExists)
                return null;

            // Generate new Guid for every new unique UserId
            user.UserId = Guid.NewGuid();

            #region Adding Data using Raw SQL Query
            //// SQL Query to Insert Data into "ApplicationUsers" table
            //string sqlQuery = "INSERT INTO ApplicationUsers (UserId, Email, Password, PersonName, Gender) VALUES (@UserId, @Email, @Password, @PersonName, @Gender)";

            //var parameters = new[]
            //{
            //    new SqlParameter("@UserId", user.UserId),
            //    new SqlParameter("@Email", user.Email ?? (object)DBNull.Value),
            //    new SqlParameter("@Password", user.Password ?? (object)DBNull.Value),
            //    new SqlParameter("@PersonName", user.PersonName ?? (object)DBNull.Value),
            //    new SqlParameter("@Gender", user.Gender ?? (object)DBNull.Value)
            //};

            //// Execute the SQL query (implementation for actual database execution would go here)
            //// _dbContext.Database.ExecuteSqlRaw(sqlQuery, user.UserId, user.Email, user.Password, user.PersonName, user.Gender);
            //int rowsAffected = await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, parameters);

            //if (rowsAffected > 0)
            //    return user;
            //else
            //    return null;
            #endregion

            #region Adding Data Using Entity Framework LINQ

            var result = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            if (result is null)
                return null;
            else
                return user;
            #endregion
        }

        public async Task<ApplicationUser?> GetUserByEmail(string? email)
        {
            #region Retrieving Data using Raw SQL Query
            //string sqlQuery = "SELECT UserId, Email, PersonName, Gender FROM ApplicationUsers WHERE Email = @Email AND Password = @Password";
            //var parameters = new[] {
            //    new SqlParameter("@Email", email ?? (object)DBNull.Value),
            //    new SqlParameter("@Password", password ?? (object)DBNull.Value)
            //};

            //List<ApplicationUser>? users = await _dbContext.Users.FromSqlRaw(sqlQuery, parameters).ToListAsync();

            //if (users != null && users.Count > 0)
            //    return users.FirstOrDefault(user => user.Email == email);
            //else
            //    return null;
            #endregion

            #region Retrieving data using Entity Framework LINQ

            var users = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email /*&& user.PasswordHash == password*/);

            if (users != null)
                return users;
            else
                return null;

            #endregion
        }

        public async Task<ApplicationUser?> GetUserByUserID(Guid? userID)
        {
            var user = await _dbContext.Users.FindAsync(userID);

            if (user is null)
                return null;
            else
                return user; 
        }

        public async Task<IEnumerable<ApplicationUser>?> GetAllUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            if (users != null && users.Count > 0)
                return users;
            else
                return null;
        }

        public async Task<bool> UpdateUser(ApplicationUser user)
        {
            var result = _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            if (result is null)
                return false;
            else
                return true;
        }
    }
}
