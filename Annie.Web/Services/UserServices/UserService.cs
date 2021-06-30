using Annie.Model;
using Annie.Model.Additional;
using Annie.Web.Models.Arguments.User;
using Annie.Web.Repositories.UserRepositories;
using Annie.Web.Services.UserServices.Models;
using Annie.Web.Services.EmailServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Annie.Web.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<User> GetUserAsync(string emailOrLogin, string password)
        {
            User user = await _userRepository.GetUserAsync(emailOrLogin: emailOrLogin);
            if (user != null)
            {
                string passwordHash = User.GetHashedPassword(password, user.SecurityStamp);
                if (user.PasswordHash == passwordHash)
                    return user;
            }
            return null;
        }
        
        public async Task<User> GetUserAsync(Email email)
        {
            return await _userRepository.GetUserAsync(email);
        }

        public async Task<SignUpStatus> SignUpUserAsync(SignUpData signUp)
        {
            var status = new SignUpStatus();

            var user = await this.GetUserAsync(new Email() { Value = signUp.User.Email });

            if (user != null)
            {
                status.Status = SignUpStatus.UserSignUpStatus.EmailAlreadyExist;
                return status;
            }

            int userId = await _userRepository.SignUpUserAsync(new UserSignUp(signUp));

            var confirmRegistrationData = await this.UpdateConfirmRegistrationDataAsync(userId);
            await _emailService.SendMailAsync(new EmailMessages.ConfirmRegistrationMessage(signUp.User.Email, confirmRegistrationData.registrationConfirmKey));

            status.Status = SignUpStatus.UserSignUpStatus.Success;
            return status;
        }

        public async Task<(int userId, string registrationConfirmKey)> UpdateConfirmRegistrationDataAsync(int userId)
        {
            var user = new User() 
            { 
                Id = userId,
            };
            user.GenerateRegistrationConfirmKey();
            await _userRepository.UpdateConfirmRegistrationDataAsync(userId: user.Id, registrationConfirmKey: user.RegistrationConfirmKey);
            return (userId: user.Id, registrationConfirmKey: user.RegistrationConfirmKey);
        }

    }
}
