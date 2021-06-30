using Annie.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Data.DatabaseProvider
{
    public class DomainModelContext : DbContext
    {
        //передача в конструктор базового класса объекта DbContextOptions, который инкапсулирует параметры конфигурации
        public DomainModelContext(DbContextOptions<DomainModelContext> options) : base(options)
        {
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuestionCategory> QuestionCategories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<DisciplineAlias> DisciplineWrappers { get; set; }
        public DbSet<Olympiad> Olympiads { get; set; }
        public DbSet<OlympiadQuestion> OlympiadQuestions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleLevel> RoleLevels { get; set; }
        public DbSet<Diploma> Diplomas { get; set; }
        public DbSet<DiplomaElement> DiplomaElements { get; set; }
        public DbSet<DiplomaProperty> DiplomaProperties { get; set; }
        public DbSet<DiplomaType> DiplomaTypes { get; set; }
        public DbSet<OlympiadSubtype> OlympiadSubtypes { get; set; }
        public DbSet<OlympiadType> OlympiadTypes { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<StudyYear> StudyYears { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<UploadedFileType> UploadedFileTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<ParticipantStatus> ParticipantStatuses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentRecipient> PaymentRecipients { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<UserResult> UserResults { get; set; }
        public DbSet<UserResultDetail> UserResultDetails { get; set; }
        public DbSet<UserDiploma> UserDiplomas { get; set; }
        public DbSet<UserDiscipline> UserDisciplines { get; set; }
        public DbSet<UserOlympiad> UserOlympiads { get; set; }
        public DbSet<UserOlympiadLocation> UserOlympiadLocations { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //выносим конфигурацию модели в отдельный класс, который реализует интерфейс EntityTypeConfiguration
            builder.ApplyConfiguration(new AnswerConfiguration());
            builder.ApplyConfiguration(new QuestionCategoryConfiguration());
            builder.ApplyConfiguration(new DepartmentConfiguration());
            builder.ApplyConfiguration(new DisciplineConfiguration());
            builder.ApplyConfiguration(new DisciplineAliasConfiguration());
            builder.ApplyConfiguration(new OlympiadConfiguration());
            builder.ApplyConfiguration(new OlympiadQuestionConfiguration());
            builder.ApplyConfiguration(new OlympiadSubtypeConfiguration());
            builder.ApplyConfiguration(new OlympiadTypeConfiguration());
            builder.ApplyConfiguration(new PaymentTypeConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new QuestionConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new RoleLevelConfiguration());
            builder.ApplyConfiguration(new DiplomaConfiguration());
            builder.ApplyConfiguration(new DiplomaElementConfiguration());
            builder.ApplyConfiguration(new DiplomaPropertyConfiguration());
            builder.ApplyConfiguration(new DiplomaTypeConfiguration());
            builder.ApplyConfiguration(new SessionConfiguration());
            builder.ApplyConfiguration(new StudyYearConfiguration());
            builder.ApplyConfiguration(new UploadedFileConfiguration());
            builder.ApplyConfiguration(new UploadedFileTypeConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserAnswerConfiguration());
            builder.ApplyConfiguration(new UserResultConfiguration());
            builder.ApplyConfiguration(new UserResultDetailConfiguration());
            builder.ApplyConfiguration(new UserDiplomaConfiguration());
            builder.ApplyConfiguration(new UserDisciplineConfiguration());
            builder.ApplyConfiguration(new UserOlympiadConfiguration());
            builder.ApplyConfiguration(new UserOlympiadLocationConfiguration());
            builder.ApplyConfiguration(new ParticipantStatusConfiguration());
            builder.ApplyConfiguration(new PaymentConfiguration());
            builder.ApplyConfiguration(new PaymentRecipientConfiguration());
            builder.ApplyConfiguration(new PaymentDetailConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());

            // использование Fluent API
            base.OnModelCreating(builder);
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DomainModelContext>
    {
        public DomainModelContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Annie/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<DomainModelContext>();
            var connectionString = configuration.GetSection("PostgreSQLSettings:ConnectionString").Value; // GetConnectionString("DatabaseConnection");
            builder.UseNpgsql(connectionString);
            return new DomainModelContext(builder.Options);
        }
    }
}
