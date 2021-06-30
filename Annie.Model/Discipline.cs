using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public partial class Discipline
    {
        public int Id { get; set; }
        public string Title { get; set; }
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

        public virtual ICollection<Olympiad> Olympiads { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<UserDiscipline> UserDisciplines { get; set; }
        public virtual UploadedFile UploadedFile { get; set; }
        public virtual ICollection<Diploma> Diplomas { get; set; }
        public virtual List<DisciplineAlias> DisciplineAliases { get; set; }
    }

    public class DisciplineConfiguration : IEntityTypeConfiguration<Discipline>
    {
        public void Configure(EntityTypeBuilder<Discipline> builder)
        {
            builder.ToTable("Discipline", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.Title);
            builder.Ignore(p => p.Image);

            builder.HasOne(p => p.UploadedFile).WithMany(p => p.Disciplines).HasForeignKey(fk => fk.ImageId);
        }
    }

    public partial class Discipline
    {
        public static explicit operator Discipline(DisciplineAlias disciplineAlias)
        {
            return new Discipline()
            {
                Id = disciplineAlias.DisciplineId,
                Title = disciplineAlias.Title,
                Description = disciplineAlias.Description,
                Image = disciplineAlias.Image,
                ImageId = disciplineAlias.ImageId,
                UploadedFile = disciplineAlias.UploadedFile
            };
        }

        public Discipline GetAlias(OlympiadTypes olympiadType)
        {
            var disciplineAlias = GetDisciplineAlias(olympiadType);

            if (disciplineAlias == null)
                return this;

            return new Discipline()
            {
                Id = disciplineAlias.DisciplineId,
                Title = disciplineAlias.Title ?? this.Title,
                Description = disciplineAlias.Description ?? this.Description,
                Image = disciplineAlias.Image ?? this.Image,
                ImageId = disciplineAlias.ImageId ?? this.ImageId,
                UploadedFile = disciplineAlias.UploadedFile ?? this.UploadedFile
            };

            // при такой конверсии не работает схема: если нет данных в псевдониме, взять данные из дисциплины
            //return disciplineAlias != null ? (Discipline)disciplineAlias : this;
        }

        public string GetTitleAliasOrDefault(OlympiadTypes olympiadType)
        {
            var disciplineAlias = GetDisciplineAlias(olympiadType);
            return disciplineAlias != null ? disciplineAlias.Title : this.Title;
        }

        public string GetDescriptionAliasOrDefault(OlympiadTypes olympiadType)
        {
            var disciplineAlias = GetDisciplineAlias(olympiadType);
            return disciplineAlias != null ? disciplineAlias.Description : this.Description;
        }

        public IFormFile GetImageAliasOrDefault(OlympiadTypes olympiadType)
        {
            var disciplineAlias = GetDisciplineAlias(olympiadType);
            return disciplineAlias != null ? disciplineAlias.Image : this.Image;
        }

        private DisciplineAlias GetDisciplineAlias(OlympiadTypes olympiadType)
        {
            return this.DisciplineAliases?.SingleOrDefault(da => da.DisciplineId == this.Id && da.OlympiadTypeId == (int)olympiadType);
        }
    }

}
