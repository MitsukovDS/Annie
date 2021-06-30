using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Services.UserServices.Models
{
    public class SignUpStatus
    {
        public UserSignUpStatus Status { get; set; }

        public enum UserSignUpStatus : int
        {
            DataIsNotValid,
            EmailAlreadyExist,
            Success
        }
    }
}
