using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public partial class UserResult
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserOlympiadId { get; set; }
        public decimal? ResultA { get; set; }
        public decimal? ResultB { get; set; }
        public decimal? ResultC { get; set; }
        public decimal TotalResult { get; set; }
        public int? RatingPlace { get; set; }
        public int ParticipantStatusId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual UserOlympiad UserOlympiad { get; set; }
        public virtual ParticipantStatus ParticipantStatus { get; set; }
    }

    public class UserResultConfiguration : IEntityTypeConfiguration<UserResult>
    {
        public void Configure(EntityTypeBuilder<UserResult> builder)
        {
            builder.ToTable(name: "UserResult", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.UserOlympiadId).IsRequired();
            builder.Property(p => p.ParticipantStatusId).IsRequired();
            builder.Property(p => p.IsDeleted).IsRequired();

            builder.HasOne(p => p.UserOlympiad).WithMany(p => p.UserResults).HasForeignKey(fk => fk.UserOlympiadId);
            builder.HasOne(p => p.ParticipantStatus).WithMany(p => p.UserResults).HasForeignKey(fk => fk.ParticipantStatusId);
        }
    }
}