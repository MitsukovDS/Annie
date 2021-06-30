using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class RoleLevel
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }

        [Description(DescriptionsForEntityModels.IgnorePropertyInQuery)]
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }

    public class RoleLevelConfiguration : IEntityTypeConfiguration<RoleLevel>
    {
        public void Configure(EntityTypeBuilder<RoleLevel> builder)
        {
            builder.ToTable("RoleLevel", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.Keyword);
            builder.HasAlternateKey(ak => ak.Title);
        }
    }

    /// <summary>
    /// Enum содержит соответствие Keyword'а уровня роли и её Id
    /// Keyword уровня роли удобно используется в авторизации для указания ролей доступа
    /// </summary>
    /// <remarks>
    /// При добавлении/редактивровании уровней ролей в БД, необходимо корректировать Enum
    /// </remarks>
    /// <seealso cref="Helpers.StaticManager.RoleLevels"/>
    public enum RoleLevels : int
    {
        Global = 1,
        Olympiad = 2
    }

    public static class RoleLevelsExtension
    {
        public static List<int> GetValues(this RoleLevels[] roleLevels)
        {
            List<int> roleLevelIds = new List<int>();
            foreach (var roleLevel in roleLevels)
            {
                roleLevelIds.Add((int)roleLevel);
            }
            return roleLevelIds;
        }
    }
}
