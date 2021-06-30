using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class UserDiscipline
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DisciplineId { get; set; }

        public virtual User User { get; set; }
        public virtual Discipline Discipline { get; set; }
    }

    public class UserDisciplineConfiguration : IEntityTypeConfiguration<UserDiscipline>
    {
        public void Configure(EntityTypeBuilder<UserDiscipline> builder)
        {
            builder.ToTable(name: "UserDiscipline", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.DisciplineId).IsRequired();

            builder.HasOne(p => p.User).WithMany(p => p.UserDisciplines).HasForeignKey(fk => fk.UserId);
            builder.HasOne(p => p.Discipline).WithMany(p => p.UserDisciplines).HasForeignKey(fk => fk.DisciplineId);
        }
    }
}
