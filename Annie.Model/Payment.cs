using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class Payment
    {
        public int Id { get; set; } // возможно, стоит объединять id и UserOlympiadId в качестве InvoisId
        public DateTime CreatedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public int PaymentTypeId { get; set; }
        public int OlympiadId { get; set; }
        public int? PayerUserId { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal? Commission { get; set; }
        public string Signature { get; set; }
        public string LinkPayment { get; set; }
        public string HashRecipients { get; set; }
        public DateTime? DateConfirmed { get; set; }
        public DateTime? DateSuccess { get; set; } // это поле нужно обновлять после редиректа на Success
        public DateTime? DateFail { get; set; }
        public bool IsManual { get; set; }
        public bool IsDeleted { get; set; }

        public virtual PaymentType PaymentType { get; set; }
        public virtual Olympiad Olympiad { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User PayerUser { get; set; }
        public virtual ICollection<PaymentRecipient> PaymentRecipients { get; set; }
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }

    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable(name: "Payment", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedDate).IsRequired();
            //builder.Property(p => p.CreatedUserId).IsRequired();
            builder.Property(p => p.PaymentTypeId).IsRequired();
            builder.Property(p => p.OlympiadId).IsRequired();
            //builder.Property(p => p.PayerUserId).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(100);
            builder.Property(p => p.Amount).IsRequired();
            builder.Property(p => p.HashRecipients).IsRequired();
            builder.Property(p => p.IsManual).IsRequired();
            builder.Property(p => p.IsDeleted).IsRequired();

            builder.HasIndex(p => p.HashRecipients).IsUnique();

            builder.HasOne(p => p.PaymentType).WithMany(p => p.Payments).HasForeignKey(fk => fk.PaymentTypeId);
            builder.HasOne(p => p.Olympiad).WithMany(p => p.Payments).HasForeignKey(fk => fk.OlympiadId);
            builder.HasOne(p => p.PayerUser).WithMany(p => p.PayerUserPayments).HasForeignKey(fk => fk.PayerUserId);
            builder.HasOne(p => p.CreatedUser).WithMany(p => p.CreatedUserPayments).HasForeignKey(fk => fk.CreatedUserId);
        }
    }
}
