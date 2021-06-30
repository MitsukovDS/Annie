using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public partial class UserResultDetail
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserOlympiadId { get; set; }
        public int OlympiadQuestionId { get; set; }
        public decimal Result { get; set; }
        public bool IsDeleted { get; set; }

        public virtual UserOlympiad UserOlympiad { get; set; }
        public virtual OlympiadQuestion OlympiadQuestion { get; set; }
    }

    public class UserResultDetailConfiguration : IEntityTypeConfiguration<UserResultDetail>
    {
        public void Configure(EntityTypeBuilder<UserResultDetail> builder)
        {
            builder.ToTable(name: "UserResultDetail", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.UserOlympiadId).IsRequired();
            builder.Property(p => p.IsDeleted).IsRequired();

            builder.HasOne(p => p.UserOlympiad).WithMany(p => p.UserResultDetails).HasForeignKey(fk => fk.UserOlympiadId);
            builder.HasOne(p => p.OlympiadQuestion).WithMany(p => p.UserResultDetails).HasForeignKey(fk => fk.OlympiadQuestionId);
        }
    }
}