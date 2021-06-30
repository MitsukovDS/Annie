using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class OlympiadSubtype
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Olympiad> Olympiads { get; set; }
        public virtual ICollection<Diploma> Diplomas { get; set; }

    }

    public class OlympiadSubtypeConfiguration : IEntityTypeConfiguration<OlympiadSubtype>
    {
        public void Configure(EntityTypeBuilder<OlympiadSubtype> builder)
        {
            builder.ToTable(name: "OlympiadSubtype", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.Keyword);
            builder.HasAlternateKey(ak => ak.Title);
        }
    }

    public enum OlympiadSubtypes : int
    {
        Default = 1,
        Winner = 2,
        Tour = 3
    }
}
