using Annie.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.ViewModels.User
{
    public class SignUp
    {
        [Required(ErrorMessage = "Фамилия не указана")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 100 символов")]
        private string lastName;
        public string LastName
        {
            get => lastName;
            set => lastName = value.Trim();
        }

        [Required(ErrorMessage = "Имя не указано")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 100 символов")]
        private string firstName;
        public string FirstName
        {
            get => firstName;
            set => firstName = value.Trim();
        }

        [Required(ErrorMessage = "Отчество не указано")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 100 символов")]
        private string middleName;
        public string MiddleName
        {
            get => middleName;
            set => middleName = value.Trim();
        }

        [Required(ErrorMessage = "Почта не указана")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес почты")]
        private string email;
        public string Email
        {
            get => email;
            set => email = value.Trim();
        }

        [Required(ErrorMessage = "Пароль не указан")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Длина строки должна быть от 8 до 100 символов")]
        public string Password { get; set; }

        public int RoleId { get; set; }
        public int[] InterestingDisciplinesIds { get; set; }


        public List<Role> Roles { get; set; }
        public List<Discipline> Disciplines { get; set; }

    }
}
