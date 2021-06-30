using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class UserDiploma
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public int OlympiadId { get; set; }
        public int UserOlympiadId { get; set; }
        public int DiplomaId { get; set; }
        public string NumberDiploma { get; set; }
        public Guid SecurityStamp { get; set; }
        public int DiplomaTypeId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Olympiad Olympiad { get; set; }
        public virtual UserOlympiad UserOlympiad { get; set; }
        public virtual Diploma Diploma { get; set; }
        public virtual DiplomaType DiplomaType { get; set; }
        public virtual User User { get; set; }
    }

    public class UserDiplomaConfiguration : IEntityTypeConfiguration<UserDiploma>
    {
        public void Configure(EntityTypeBuilder<UserDiploma> builder)
        {
            builder.ToTable(name: "UserDiploma", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasAlternateKey(ak => ak.NumberDiploma);

            builder.HasAlternateKey(ak => ak.ExternalId);
            builder.Property(p => p.ExternalId).HasDefaultValueSql("uuid_generate_v4()");

            builder.HasAlternateKey(ak => ak.SecurityStamp);
            builder.Property(p => p.SecurityStamp).HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.OlympiadId).IsRequired();
            builder.Property(p => p.UserOlympiadId).IsRequired();
            builder.Property(p => p.DiplomaId).IsRequired();
            builder.Property(p => p.DiplomaTypeId).IsRequired();
            builder.Property(p => p.DiplomaTypeId).HasDefaultValue((int)DiplomaTypes.Electronic);
            builder.Property(p => p.IsDeleted).IsRequired();

            builder.HasOne(p => p.Olympiad).WithMany(p => p.UserDiplomas).HasForeignKey(fk => fk.OlympiadId);
            builder.HasOne(p => p.UserOlympiad).WithMany(p => p.UserDiplomas).HasForeignKey(fk => fk.UserOlympiadId);
            builder.HasOne(p => p.Diploma).WithMany(p => p.UserDiplomas).HasForeignKey(fk => fk.DiplomaId);
            builder.HasOne(p => p.DiplomaType).WithMany(p => p.UserDiplomas).HasForeignKey(fk => fk.DiplomaTypeId);
            builder.HasOne(p => p.User).WithMany(p => p.UserDiplomas).HasForeignKey(fk => fk.UserId);
        }
    }
}
