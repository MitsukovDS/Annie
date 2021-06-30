using Annie.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Services.AuthServices.Models
{
    public class LoginStatus
    {
        public User User { get; set; }
        public UserLoginStatus Status { get; set; }

        public enum UserLoginStatus : int
        {
            DataIsNotValid,
            NoUser,
            EmailNotConfirmed,
            UserIsDeactivated,
            Success
        }
    }
}
