using Annie.Web.Services.AuthServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Services.AuthServices
{
    public interface IAuthService
    {
        public void Logout();
        public Task<LoginStatus> LoginAsync(LoginData loginData);
    }
}
