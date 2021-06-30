using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class UserOlympiadLocation
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserOlympiadId { get; set; }
        public string IP { get; set; }
        public string GeoLatitude { get; set; }
        public string GeoLongitude { get; set; }
        public string House { get; set; }
        public string Street { get; set; }
        public string Settlement { get; set; }
        public string SettlementType { get; set; }
        public string CityDistrict { get; set; }
        public string City { get; set; }
        public string RegionArea { get; set; }
        public string Region { get; set; }
        public string FederalDistrict { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; } // Адрес одной строкой (как показывается в списке подсказок)
        public string AddressFull { get; set; } // Адрес одной строкой (полный, с индексом)
        public string HouseFiasId { get; set; } // Код ФИАС дома

        public bool IsDeleted { get; set; }

        public virtual UserOlympiad UserOlympiad { get; set; }
    }

    public class UserOlympiadLocationConfiguration : IEntityTypeConfiguration<UserOlympiadLocation>
    {
        public void Configure(EntityTypeBuilder<UserOlympiadLocation> builder)
        {
            builder.ToTable(name: "UserOlympiadLocation", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.UserOlympiadId).IsRequired();
            builder.Property(p => p.IsDeleted).IsRequired();

            builder.HasOne(p => p.UserOlympiad).WithMany(p => p.UserOlympiadLocations).HasForeignKey(fk => fk.UserOlympiadId);
        }
    }
}
