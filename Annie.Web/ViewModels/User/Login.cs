using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.ViewModels.User
{
    public class Login
    {
        private string emailOrLogin;
        public string EmailOrLogin { get => emailOrLogin; set => emailOrLogin = value?.Trim(); }

        private string password;
        public string Password { get => password; set => password = value?.Trim(); }

        public bool RememberMe { get; set; }
    }
}
