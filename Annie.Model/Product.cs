using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }

        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(name: "Product", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.Keyword);
            builder.HasAlternateKey(ak => ak.Title);
        }
    }

    public enum Products : int
    {
        Olympiad = 1,
        PaperDiploma = 2
    }
}
