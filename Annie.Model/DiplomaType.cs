using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class DiplomaType
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }

        public virtual ICollection<UserDiploma> UserDiplomas { get; set; }
    }

    public class DiplomaTypeConfiguration : IEntityTypeConfiguration<DiplomaType>
    {
        public void Configure(EntityTypeBuilder<DiplomaType> builder)
        {
            builder.ToTable(name: "DiplomaType", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.Keyword);
            builder.HasAlternateKey(ak => ak.Title);
        }
    }

    public enum DiplomaTypes : int
    {
        Electronic = 1,
        Paper = 2
    }
}
