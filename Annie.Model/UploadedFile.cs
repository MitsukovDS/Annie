using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Annie.Model
{
    [Serializable()]
    public class UploadedFile
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Path")]
        public string Path { get; set; }
        [JsonPropertyName("FullPath")]
        public string FullPath
        {
            get => string.IsNullOrEmpty(Path) ? Path : "https://store.1zvonok.com" + Path;
        }

        [JsonPropertyName("UploadedFileTypeId")]
        public int UploadedFileTypeId { get; set; }
        [JsonPropertyName("OriginalName")]
        public string OriginalName { get; set; }
        [JsonPropertyName("Hash")]
        public string Hash { get; set; }

        [JsonIgnore]
        public virtual ICollection<Olympiad> Olympiads { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
        [JsonIgnore]
        public virtual ICollection<Answer> Answers { get; set; }
        [JsonIgnore]
        public virtual ICollection<Question> Questions { get; set; }
        [JsonIgnore]
        public virtual ICollection<Discipline> Disciplines { get; set; }
        [JsonIgnore]
        public virtual ICollection<DisciplineAlias> DisciplineAliases { get; set; }
        [JsonIgnore]
        public virtual ICollection<Diploma> Diplomas { get; set; }
        [JsonIgnore]
        public virtual UploadedFileType UploadedFileType { get; set; }
    }

    public class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedFile>
    {
        public void Configure(EntityTypeBuilder<UploadedFile> builder)
        {
            builder.ToTable(name: "UploadedFile", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Ignore(p => p.FullPath);

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.OriginalName).IsRequired();
            builder.Property(p => p.Hash).IsRequired();
            builder.Property(p => p.Path).IsRequired();

            builder.HasOne(p => p.UploadedFileType).WithMany(p => p.UploadedFiles).HasForeignKey(fk => fk.UploadedFileTypeId);
        }
    }
}
