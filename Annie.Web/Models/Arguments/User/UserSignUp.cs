using Annie.Web.Services.UserServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Models.Arguments.User
{
    public class UserSignUp
    {
        public UserSignUp(SignUpData signUpData)
        {
            this.LastName = signUpData.User.LastName;
            this.FirstName = signUpData.User.FirstName;
            this.MiddleName = signUpData.User.MiddleName;
            this.ImageId = signUpData.User.ImageId;
            this.Login = signUpData.User.Login;
            this.Email = signUpData.User.Email;
            this.RegistrationConfirmKey = signUpData.User.RegistrationConfirmKey;
            this.PasswordHash = signUpData.User.PasswordHash;
            this.SecurityStamp = signUpData.User.SecurityStamp;
            this.InterestingDisciplineIds = signUpData.InterestingDisciplinesIds;
            this.RoleIds = new int[] { signUpData.RoleId };
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int? ImageId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string RegistrationConfirmKey { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public int[] InterestingDisciplineIds { get; set; }
        public int[] RoleIds { get; set; }
    }
}
