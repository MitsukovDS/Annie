using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class PaymentRecipient
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PaymentId { get; set; }
        public int UserOlympiadId { get; set; }
        public int RecipientUserId { get; set; }

        public virtual Payment Payment { get; set; }
        public virtual UserOlympiad UserOlympiad { get; set; }
        public virtual User RecipientUser { get; set; }
    }

    public class PaymentRecipientConfiguration : IEntityTypeConfiguration<PaymentRecipient>
    {
        public void Configure(EntityTypeBuilder<PaymentRecipient> builder)
        {
            builder.ToTable(name: "PaymentRecipient", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.PaymentId).IsRequired();
            builder.Property(p => p.UserOlympiadId).IsRequired();
            builder.Property(p => p.RecipientUserId).IsRequired();

            builder.HasOne(p => p.Payment).WithMany(p => p.PaymentRecipients).HasForeignKey(fk => fk.PaymentId);
            builder.HasOne(p => p.UserOlympiad).WithMany(p => p.PaymentRecipients).HasForeignKey(fk => fk.UserOlympiadId);
            builder.HasOne(p => p.RecipientUser).WithMany(p => p.PaymentRecipients).HasForeignKey(fk => fk.RecipientUserId);
        }
    }
}
