using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class Olympiad
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public int OlympiadTypeId { get; set; }
        public int OlympiadSubtypeId { get; set; }
        public int DisciplineId { get; set; }
        public int DepartmentId { get; set; }
        public int? TourNumber { get; set; }
        public int? ChildOlympiadId { get; set; }
        public int? ParentOlympiadId { get; set; }
        public int StudyYearId { get; set; }
        public int SessionId { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        private int? imageId;
        public int? ImageId
        {
            get => imageId;
            set
            {
                if (value == 0)
                    imageId = null;
                else
                    imageId = value;
            }
        }
        public DateTime StartDateRegistration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxMinutesOnline { get; set; }
        public int? MinResult { get; set; }
        public decimal MinResultForWinnerStatus { get; set; } = 99.00m;
        public decimal MinResultForPrizeWinnerStatus { get; set; } = 85.00m;
        public decimal MinResultForParticipantStatus { get; set; } = 0.00m;
        public double Price { get; set; }
        public double PaperPrice { get; set; }
        public bool IsFree { get; set; }
        public bool IsCompleted { get; set; }

        public virtual OlympiadSubtype OlympiadSubtype { get; set; }
        public virtual OlympiadType OlympiadType { get; set; }
        public virtual Discipline Discipline { get; set; }
        public virtual Department Department { get; set; }
        public virtual Olympiad ChildOlympiad { get; set; }
        public virtual Olympiad ParentOlympiad { get; set; }
        public virtual ICollection<OlympiadQuestion> OlympiadQuestions { get; set; }
        public virtual ICollection<UserOlympiad> UserOlympiads { get; set; }
        public virtual UploadedFile UploadedFile { get; set; }
        public virtual ICollection<UserDiploma> UserDiplomas { get; set; }
        public virtual Session Session { get; set; }
        public virtual StudyYear StudyYear { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }

    public class OlympiadConfiguration : IEntityTypeConfiguration<Olympiad>
    {
        public void Configure(EntityTypeBuilder<Olympiad> builder)
        {
            builder.ToTable(name: "Olympiad", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.ExternalId);
            builder.Property(p => p.ExternalId).HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.OlympiadTypeId).IsRequired();
            builder.Property(p => p.OlympiadSubtypeId).IsRequired();
            builder.Property(p => p.OlympiadSubtypeId).HasDefaultValue((int)OlympiadSubtypes.Default);
            builder.Property(p => p.DisciplineId).IsRequired();
            builder.Property(p => p.DepartmentId).IsRequired();
            builder.Property(p => p.StartDate).IsRequired();
            builder.Property(p => p.EndDate).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.PaperPrice).IsRequired();
            builder.Property(p => p.IsFree).IsRequired();
            builder.Property(p => p.IsCompleted).IsRequired();
            builder.Ignore(p => p.Image);

            builder.HasOne(p => p.OlympiadSubtype).WithMany(p => p.Olympiads).HasForeignKey(fk => fk.OlympiadSubtypeId);
            builder.HasOne(p => p.OlympiadType).WithMany(p => p.Olympiads).HasForeignKey(fk => fk.OlympiadTypeId);
            builder.HasOne(p => p.Discipline).WithMany(p => p.Olympiads).HasForeignKey(fk => fk.DisciplineId);
            builder.HasOne(p => p.Department).WithMany(p => p.Olympiad).HasForeignKey(fk => fk.DepartmentId);
            //builder.HasOne(p => p.ChildOlympiad).WithOne(p => p.ParentOlympiad).HasForeignKey<Olympiad>(fk => fk.ChildOlympiadId);
            builder.HasOne(p => p.ChildOlympiad).WithOne(p => p.ParentOlympiad).HasForeignKey(typeof(Olympiad), "ChildOlympiadId");
            //builder.HasOne(p => p.ParentOlympiad).WithOne(p => p.ChildOlympiad).HasForeignKey<Olympiad>(fk => fk.ParentOlympiadId);
            builder.HasOne(p => p.ParentOlympiad).WithOne(p => p.ChildOlympiad).HasForeignKey(typeof(Olympiad), "ParentOlympiadId");
            builder.HasOne(p => p.UploadedFile).WithMany(p => p.Olympiads).HasForeignKey(fk => fk.ImageId);
            builder.HasOne(p => p.Session).WithMany(p => p.Olympiads).HasForeignKey(fk => fk.SessionId);
            builder.HasOne(p => p.StudyYear).WithMany(p => p.Olympiads).HasForeignKey(fk => fk.SessionId);
        }
    }
}
