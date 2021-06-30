using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class DisciplineAlias
    {
        public int Id { get; set; }
        public int DisciplineId { get; set; }
        public int OlympiadTypeId { get; set; }
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

        public virtual Discipline Discipline { get; set; }
        public virtual OlympiadType OlympiadType { get; set; }
        public virtual UploadedFile UploadedFile { get; set; }
    }

    public class DisciplineAliasConfiguration : IEntityTypeConfiguration<DisciplineAlias>
    {
        public void Configure(EntityTypeBuilder<DisciplineAlias> builder)
        {
            builder.ToTable("DisciplineAlias", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.DisciplineId).IsRequired();
            builder.Property(p => p.OlympiadTypeId).IsRequired();
            builder.HasAlternateKey(ak => new { ak.DisciplineId, ak.OlympiadTypeId });
            builder.Ignore(p => p.Image);

            builder.HasOne(p => p.Discipline).WithMany(p => p.DisciplineAliases).HasForeignKey(fk => fk.DisciplineId);
            builder.HasOne(p => p.OlympiadType).WithMany(p => p.DisciplineAliases).HasForeignKey(fk => fk.OlympiadTypeId);
            builder.HasOne(p => p.UploadedFile).WithMany(p => p.DisciplineAliases).HasForeignKey(fk => fk.ImageId);
        }
    }
}
