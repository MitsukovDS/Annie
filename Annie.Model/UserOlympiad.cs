using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class UserOlympiad
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OlympiadId { get; set; }
        public int LeaderUserId { get; set; }
        public int ParticipantUserId { get; set; }
        public string InstituteTitle { get; set; }
        public DateTime? StartTimeOnline { get; set; }
        public DateTime? EndTimeOnline { get; set; }
        public string IndexRecipient { get; set; }
        public string AddressRecipient { get; set; }
        public string FullNameRecipient { get; set; }
        public bool DiplomaSent { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Olympiad Olympiad { get; set; }
        public virtual User LeaderUser { get; set; }
        public virtual User ParticipantUser { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<UserDiploma> UserDiplomas { get; set; }
        public virtual List<UserResult> UserResults { get; set; }
        public virtual ICollection<UserResultDetail> UserResultDetails { get; set; }
        public virtual ICollection<PaymentRecipient> PaymentRecipients { get; set; }
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
        public virtual ICollection<UserOlympiadLocation> UserOlympiadLocations { get; set; }
    }

    public class UserOlympiadConfiguration : IEntityTypeConfiguration<UserOlympiad>
    {
        public void Configure(EntityTypeBuilder<UserOlympiad> builder)
        {
            builder.ToTable(name: "UserOlympiad", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.ExternalId);
            builder.Property(p => p.ExternalId).HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.OlympiadId).IsRequired();
            builder.Property(p => p.LeaderUserId).IsRequired();
            builder.Property(p => p.ParticipantUserId).IsRequired();
            builder.Property(p => p.DiplomaSent).IsRequired();
            builder.Property(p => p.IsDeleted).IsRequired();

            builder.HasOne(p => p.Olympiad).WithMany(p => p.UserOlympiads).HasForeignKey(fk => fk.OlympiadId);
            builder.HasOne(p => p.LeaderUser).WithMany(p => p.LeaderOlympiads).HasForeignKey(fk => fk.LeaderUserId);
            builder.HasOne(p => p.ParticipantUser).WithMany(p => p.ParticipantOlympiads).HasForeignKey(fk => fk.ParticipantUserId);
            //builder.HasOne(typeof(User)).WithMany().HasForeignKey(nameof(UserOlympiad.LeaderUserId));
        }
    }
}