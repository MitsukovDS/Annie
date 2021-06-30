using Annie.Data.DatabaseProvider;
using Annie.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Authentication
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public AuthenticationRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<User> GetUserAsync(string email, string passwordHash)
        {
            using (IDbConnection dbConnection = _dbConnectionFactory.CreateConnection())
            {
                #region Query
                string query = @"SELECT u.* 
                                 FROM public.""User"" u
                                 WHERE LOWER(u.""Email"") = LOWER(@email) AND 
                                       u.""PasswordHash"" = @passwordHash;";

                return (await dbConnection.QueryAsync<User>(query, new { email, passwordHash })).SingleOrDefault();
                #endregion
            }
        }

        public async Task<List<UserRole>> GetUserRolesAsync(int userId, bool activeUserRole = true)
        {
            using var dbConnection = _dbConnectionFactory.CreateConnection();
            #region Query
            string query = @"SELECT *
                             FROM public.""UserRole"" ur
                             WHERE ""UserId"" = @userId AND 
                                   ur.""IsActive"" = @activeUserRole AND
                                  (ur.""EndDate"" IS NULL OR ur.""EndDate"" > CURRENT_TIMESTAMP);";

            return (await dbConnection.QueryAsync<UserRole>(query, new { userId, activeUserRole })).ToList();
            #endregion
        }
    }
}
