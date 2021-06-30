using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int RoleLevelId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        public virtual RoleLevel RoleLevel { get; set; }
    }

    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.RoleId).IsRequired();
            builder.Property(p => p.RoleLevelId).IsRequired();
            builder.Property(p => p.StartDate).IsRequired();
            builder.Property(p => p.IsActive).IsRequired();

            builder.HasOne(p => p.User).WithMany(p => p.UserRoles).HasForeignKey(fk => fk.UserId);
            builder.HasOne(p => p.Role).WithMany(p => p.UserRoles).HasForeignKey(fk => fk.RoleId);
            builder.HasOne(p => p.RoleLevel).WithMany(p => p.UserRoles).HasForeignKey(fk => fk.RoleLevelId);
        }
    }
}