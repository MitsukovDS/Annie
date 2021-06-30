using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class PaymentDetail
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PaymentId { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
        public int UserOlympiadId { get; set; }
        public int RecipientUserId { get; set; }

        public virtual Payment Payment { get; set; }
        public virtual Product Product { get; set; }
        public virtual UserOlympiad UserOlympiad { get; set; }
        public virtual User RecipientUser { get; set; }
    }

    public class PaymentDetailConfiguration : IEntityTypeConfiguration<PaymentDetail>
    {
        public void Configure(EntityTypeBuilder<PaymentDetail> builder)
        {
            builder.ToTable(name: "PaymentDetail", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.PaymentId).IsRequired();
            builder.Property(p => p.ProductId).IsRequired();
            builder.Property(p => p.Amount).IsRequired();
            builder.Property(p => p.UserOlympiadId).IsRequired();
            builder.Property(p => p.RecipientUserId).IsRequired();

            builder.HasOne(p => p.Payment).WithMany(p => p.PaymentDetails).HasForeignKey(fk => fk.PaymentId);
            builder.HasOne(p => p.Product).WithMany(p => p.PaymentDetails).HasForeignKey(fk => fk.ProductId);
            builder.HasOne(p => p.UserOlympiad).WithMany(p => p.PaymentDetails).HasForeignKey(fk => fk.UserOlympiadId);
            builder.HasOne(p => p.RecipientUser).WithMany(p => p.PaymentDetails).HasForeignKey(fk => fk.RecipientUserId);
        }
    }
}
