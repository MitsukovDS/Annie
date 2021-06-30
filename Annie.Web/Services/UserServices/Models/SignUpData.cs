using Annie.Model;
using Annie.Web.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Services.UserServices.Models
{
    public class SignUpData
    {
        public SignUpData(SignUp viewModel)
        {
            var fullName = viewModel.LastName + viewModel.FirstName + viewModel.MiddleName;

            if (!fullName.Any(c => !char.IsLetter(c)))
            {
                viewModel.LastName = char.ToUpper(viewModel.LastName[0]) + viewModel.LastName.Substring(1).ToLower();
                viewModel.FirstName = char.ToUpper(viewModel.FirstName[0]) + viewModel.FirstName.Substring(1).ToLower();
                viewModel.MiddleName = char.ToUpper(viewModel.MiddleName[0]) + viewModel.MiddleName.Substring(1).ToLower();
            }

            var user = new User()
            {
                LastName = viewModel.LastName.Trim(),
                FirstName = viewModel.FirstName.Trim(),
                MiddleName = viewModel.MiddleName.Trim(),
                Email = viewModel.Email.Trim(),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                EmailConfirmed = false
            };

            this.InterestingDisciplinesIds = viewModel.InterestingDisciplinesIds;
            this.RoleId = viewModel.RoleId;

            user.ChangePassword(viewModel.Password);
            user.GenerateRegistrationConfirmKey();

            this.User = user;
        }

        public User User { get; private set; }
        public int RoleId { get; private set; }
        public int[] InterestingDisciplinesIds { get; private set; }
    }
}
