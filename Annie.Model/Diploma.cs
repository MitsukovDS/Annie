using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class Diploma
    {
        public int Id { get; set; }
        //public string Keyword { get; set; }
        //public string Title { get; set; }
        //public string NameTemplate { get; set; }
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
        public int OlympiadTypeId { get; set; }
        public int? OlympiadSubtypeId { get; set; }
        public int? DisciplineId { get; set; }
        public int? DepartmentId { get; set; }
        public int? TourNumber { get; set; }
        public int StudyYearId { get; set; }
        public int? SessionId { get; set; }
        public int? RoleId { get; set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public virtual ICollection<UserDiploma> UserDiplomas { get; set; }
        public virtual UploadedFile UploadedFile { get; set; }
        public virtual OlympiadType OlympiadType { get; set; }
        public virtual OlympiadSubtype OlympiadSubtype { get; set; }
        public virtual Discipline Discipline { get; set; }
        public virtual Department Department { get; set; }
        public virtual StudyYear StudyYear { get; set; }
        public virtual Session Session { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<DiplomaProperty> DiplomaProperties { get; set; }
    }

    public class DiplomaConfiguration : IEntityTypeConfiguration<Diploma>
    {
        public void Configure(EntityTypeBuilder<Diploma> builder)
        {
            builder.ToTable(name: "Diploma", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.ImageId).IsRequired();
            builder.Property(p => p.OlympiadTypeId).IsRequired();
            builder.Property(p => p.StudyYearId).IsRequired();
            builder.Ignore(p => p.Image);
            builder.Ignore(p => p.Width);
            builder.Ignore(p => p.Height);

            builder.HasOne(p => p.OlympiadType).WithMany(p => p.Diplomas).HasForeignKey(fk => fk.OlympiadTypeId);
            builder.HasOne(p => p.OlympiadSubtype).WithMany(p => p.Diplomas).HasForeignKey(fk => fk.OlympiadSubtypeId);
            builder.HasOne(p => p.Discipline).WithMany(p => p.Diplomas).HasForeignKey(fk => fk.DisciplineId);
            builder.HasOne(p => p.Department).WithMany(p => p.Diplomas).HasForeignKey(fk => fk.DepartmentId);
            builder.HasOne(p => p.StudyYear).WithMany(p => p.Diplomas).HasForeignKey(fk => fk.StudyYearId);
            builder.HasOne(p => p.Session).WithMany(p => p.Diplomas).HasForeignKey(fk => fk.SessionId);
            builder.HasOne(p => p.Role).WithMany(p => p.Diplomas).HasForeignKey(fk => fk.RoleId);
            builder.HasOne(p => p.UploadedFile).WithMany(p => p.Diplomas).HasForeignKey(fk => fk.ImageId);
        }
    }
}
