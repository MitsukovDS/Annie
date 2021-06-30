using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Annie.Model
{
    [Description(SchemasPostgreSql.PublicSchema)]
    public partial class User
    {
        [Description(DescriptionsForEntityModels.IgnorePropertyInQuery)]
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? ImageId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime? DateSendEmailConfirmed { get; set; }
        public string RegistrationConfirmKey { get; private set; }
        public string PasswordRecoveryKey { get; private set; }
        public string PasswordHash { get; private set; }
        public string SecurityStamp { get; private set; }
        public string FirstPassword { get; private set; }
        public bool IsActive { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserOlympiad> LeaderOlympiads { get; set; }
        public virtual ICollection<UserOlympiad> ParticipantOlympiads { get; set; }
        public virtual ICollection<UserDiscipline> UserDisciplines { get; set; }
        public virtual UploadedFile UploadedFile { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<UserDiploma> UserDiplomas { get; set; }
        public virtual ICollection<Payment> PayerUserPayments { get; set; }
        public virtual ICollection<Payment> CreatedUserPayments { get; set; }
        public virtual ICollection<PaymentRecipient> PaymentRecipients { get; set; }
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            //builder.HasAlternateKey(ak => ak.Login);
            //builder.HasAlternateKey(ak => ak.Email); // устанавлиает ограничение на уникальность (email должен быть уникальным)
            builder.HasAlternateKey(ak => ak.ExternalId);
            builder.Property(p => p.ExternalId).HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.LastName).IsRequired();
            builder.Property(p => p.FirstName).IsRequired();
            builder.Property(p => p.MiddleName).IsRequired();
            //builder.Property(p => p.BirthDate).IsRequired();
            //builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.EmailConfirmed).IsRequired();
            builder.Property(p => p.PasswordHash).IsRequired();
            builder.Property(p => p.SecurityStamp).IsRequired();
            builder.Property(p => p.IsActive).IsRequired();
            builder.Property(p => p.IsActive).HasDefaultValue(true);

            builder.HasOne(p => p.UploadedFile).WithMany(p => p.Users).HasForeignKey(fk => fk.ImageId);
        }
    }

    public partial class User
    {
        public void ChangePassword(string sourcePassword)
        {
            var securityStamp = GetSecurityStamp();
            var passwordHash = GetHashedPassword(sourcePassword.Trim(), securityStamp);

            this.SecurityStamp = securityStamp;
            this.PasswordHash = passwordHash;
        }

        public void GenerateFirstPassword()
        {
            this.FirstPassword = GenerateRandomString(8);
        }

        public void GeneratePasswordRecoveryKey()
        {
            this.PasswordRecoveryKey = GenerateRandomString();
        }

        public void GenerateRegistrationConfirmKey()
        {
            this.RegistrationConfirmKey = GenerateRandomString();
        }

        /// <summary>
        /// Метод для исходного пароля с участием соли возвращает хэш-пароль
        /// </summary>
        /// <param name="password">Исходный пароль</param>
        /// <param name="securityStamp">Соль</param>
        /// <returns>Хэшированный пароль</returns>
        public static string GetHashedPassword(string password, string securityStamp)
        {
            return Hash(Hash(password) + securityStamp);
        }


        #region privates methods

        /// <summary>
        /// Метод генерирует соль 
        /// </summary>
        /// <returns>Соль</returns>
        private static string GetSecurityStamp() => GenerateRandomString();


        public static string GenerateRandomString(int length = 50)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder result = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    result.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Метод возвращает хэшированное значения для исходного значения
        /// </summary>
        /// <param name="input">Значение, которое необходимо хэшировать</param>
        /// <returns>Хэшированное значение</returns>
        private static string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
        #endregion
    }
}
