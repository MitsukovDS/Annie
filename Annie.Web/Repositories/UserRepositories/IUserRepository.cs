using Annie.Model;
using Annie.Model.Additional;
using Annie.Web.Models.Arguments.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Repositories.UserRepositories
{
    public interface IUserRepository
    {
        public Task<User> GetUserAsync(string emailOrLogin);
        public Task<User> GetUserAsync(Email email);
        public Task<int> SignUpUserAsync(UserSignUp user);
        public Task UpdateConfirmRegistrationDataAsync(int userId, string registrationConfirmKey);

    }
}
