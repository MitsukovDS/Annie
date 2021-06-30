using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class UploadedFileType
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<UploadedFile> UploadedFiles { get; set; }
    }

    public class UploadedFileTypeConfiguration : IEntityTypeConfiguration<UploadedFileType>
    {
        public void Configure(EntityTypeBuilder<UploadedFileType> builder)
        {
            builder.ToTable(name: "UploadedFileType", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Title).IsRequired();
        }
    }

    public enum UploadedFileTypes : int
    {
        OlympiadFile = 1,
        QuestionFile = 2,
        AnswerFile = 3,
        DisciplineFile = 4,
        DiplomaFile = 5,
        AvatarFile = 6,
        StaticFile = 7
    }
}
