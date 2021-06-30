using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class OlympiadType
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Olympiad> Olympiads { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Diploma> Diplomas { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<DisciplineAlias> DisciplineAliases { get; set; }
    }

    public class OlympiadTypeConfiguration : IEntityTypeConfiguration<OlympiadType>
    {
        public void Configure(EntityTypeBuilder<OlympiadType> builder)
        {
            builder.ToTable(name: "OlympiadType", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.Keyword);
            builder.HasAlternateKey(ak => ak.Title);
        }
    }

    public enum OlympiadTypes : int
    {
        Kindergarten = 1,
        School = 2,
        University = 3
    }
}
