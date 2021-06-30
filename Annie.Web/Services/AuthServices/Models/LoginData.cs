using Annie.Web.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Services.AuthServices.Models
{
    public class LoginData
    {
        public LoginData(Login viewModel)
        {
            this.EmailOrLogin = viewModel.EmailOrLogin;
            this.Password = viewModel.Password;
            this.RememberMe = viewModel.RememberMe;
        }

        public string EmailOrLogin { get; private set; }
        public string Password { get; private set; }
        public bool RememberMe { get; private set; }
    }
}
