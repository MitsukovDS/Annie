using Annie.Model;
using Annie.Model.Additional;
using Annie.Web.Services.UserServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Services.UserServices
{
    public interface IUserService
    {
        public Task<User> GetUserAsync(string emailOrLogin, string password);
        public Task<User> GetUserAsync(Email email);
        public Task<SignUpStatus> SignUpUserAsync(SignUpData signUp);
        public Task<(int userId, string registrationConfirmKey)> UpdateConfirmRegistrationDataAsync(int userId);

    }
}
