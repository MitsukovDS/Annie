using Annie.Web.Services.AuthServices.Models;
using Annie.Web.Services.GrpcServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Annie.Authorization.JWToken;
using Annie.Web.Services.UserServices;
using Annie.Model.Additional;

namespace Annie.Web.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGrpcService _grpcService;
        private readonly IUserService _userService;

        public AuthService(IHttpContextAccessor httpContextAccessor, IGrpcService grpcService, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _grpcService = grpcService;
            _userService = userService;
        }

        public void Logout() => _httpContextAccessor.HttpContext.Response.Cookies.Delete(JwtSettings.Parameters.JwtName);

        public async Task<LoginStatus> LoginAsync(LoginData loginData)
        {
            var status = new LoginStatus();
            status.User = await _userService.GetUserAsync(emailOrLogin: loginData.EmailOrLogin, password: loginData.Password);

            if (status.User == null)
            {
                status.Status = LoginStatus.UserLoginStatus.NoUser;
                return status;
            }

            if (!status.User.EmailConfirmed)
            {
                status.Status = LoginStatus.UserLoginStatus.EmailNotConfirmed;
                return status;
            }

            if (!status.User.IsActive)
            {
                status.Status = LoginStatus.UserLoginStatus.UserIsDeactivated;
                return status;
            }

            var tokenData = await _grpcService.GetTokenAsync(new Authentication.AuthenticationRequest()
            {
                UserId = status.User.Id,
                RememberMe = loginData.RememberMe
            });

            status.Status = LoginStatus.UserLoginStatus.Success;
            _httpContextAccessor.HttpContext.Response.Cookies.Append(JwtSettings.Parameters.JwtName, tokenData.Token);

            return status;
        }

    }
}
