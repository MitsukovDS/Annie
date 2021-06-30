using Annie.Data.DatabaseProvider;
using Annie.Model;
using Annie.Model.Additional;
using Annie.Web.Models.Arguments.User;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Repositories.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<User> GetUserAsync(string emailOrLogin)
        {
            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
            #region Query
            string query = @"SELECT u.* 
                             FROM public.""User"" u
                             WHERE LOWER(u.""Email"") = LOWER(@emailOrLogin) OR 
                                   LOWER(u.""Login"") = LOWER(@emailOrLogin);";

            return (await dbConnection.QueryAsync<User>(query, new { emailOrLogin })).SingleOrDefault();
            #endregion
        }

        public async Task<User> GetUserAsync(Email email)
        {
            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
            #region Query
            string query = @"SELECT u.* 
                             FROM public.""User"" u
                             WHERE LOWER(u.""Email"") = LOWER(@email);";

            return (await dbConnection.QueryAsync<User>(query, new { email = email.Value })).SingleOrDefault();
            #endregion
        }

        public async Task<int> SignUpUserAsync(UserSignUp user)
        {
            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
            #region Query
            string query = @"SELECT create_user(
                                            @lastName, 
                                            @firstName, 
                                            @middleName, 
                                            @imageId, 
                                            @login, 
                                            @email, 
                                            @registrationConfirmKey, 
                                            @passwordHash, 
                                            @securityStamp, 
                                            @interestingDisciplineIds, 
                                            @roleIds)";

            return (await dbConnection.QueryAsync<int>(query, new
            {
                lastName = user.LastName,
                firstName = user.FirstName,
                middleName = user.MiddleName,
                imageId = user.ImageId,
                login = user.Login,
                email = user.Email,
                registrationConfirmKey = user.RegistrationConfirmKey,
                passwordHash = user.PasswordHash,
                securityStamp = user.SecurityStamp,
                interestingDisciplineIds = user.InterestingDisciplineIds,
                roleIds = user.RoleIds
            })).Single();
            #endregion
        }

        public async Task UpdateConfirmRegistrationDataAsync(int userId, string registrationConfirmKey)
        {
            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
            #region Query
            string query = @"UPDATE public.""User""
                             SET ""DateSendEmailConfirmed"" = CURRENT_TIMESTAMP,
                                 ""RegistrationConfirmKey"" = @registrationConfirmKey
                             WHERE ""Id"" = @Id";

            await dbConnection.ExecuteAsync(query, new { Id = userId, registrationConfirmKey = registrationConfirmKey });
            #endregion
        }
    }
}
