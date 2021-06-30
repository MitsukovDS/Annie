using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    [Description(SchemasPostgreSql.PublicSchema)]
    public class Role
    {
        [Description(DescriptionsForEntityModels.IgnorePropertyInQuery)]
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }
        public int RoleLevelId { get; set; }
        public string RoleLevelKeyword { get; set; }

        [Description(DescriptionsForEntityModels.IgnorePropertyInQuery)]
        public virtual RoleLevel RoleLevel { get; set; }
        [Description(DescriptionsForEntityModels.IgnorePropertyInQuery)]
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Diploma> Diplomas { get; set; }

        //public static implicit operator Role(RoleValues roleValues)
        //{
        //    Type myType = (typeof(RoleValues));
        //    Type[] myTypeArray = myType.GetNestedTypes(BindingFlags.Public);

        //    List<Role> roles = new List<Role>();
        //    Role role = new Role();
        //    foreach(var type in myTypeArray)
        //    {
        //        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        //        try
        //        {
        //            List<string> listFieldsName = (fields.Where(fieldInfo => fieldInfo.IsLiteral
        //                && !fieldInfo.IsInitOnly
        //                && fieldInfo.FieldType == typeof(string)).Select(f => (string)f.Name)).ToList();

        //            foreach (var fieldName in listFieldsName)
        //            {
        //                string fieldValue = (fields.Where(fieldInfo => fieldInfo.IsLiteral
        //                    && !fieldInfo.IsInitOnly
        //                    && fieldInfo.Name == fieldName
        //                    && fieldInfo.FieldType == typeof(string)).Select(f => (string)f.GetRawConstantValue())).FirstOrDefault();

        //                switch (fieldName)
        //                {
        //                    case "Title":
        //                        {
        //                            role.Title = fieldValue;
        //                            break;
        //                        };
        //                    case "Keyword":
        //                        {
        //                            role.Keyword = fieldValue;
        //                            break;
        //                        };
        //                    case "RoleLevelKeyword":
        //                        {
        //                            role.RoleLevelKeyword = fieldValue;
        //                            break;
        //                        }
        //                }
        //            }
        //            roles.Add(role);
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //    return roles.FirstOrDefault();
        //}
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.Title);
            builder.HasAlternateKey(ak => ak.Keyword);

            builder.Property(p => p.RoleLevelId).IsRequired();
            builder.Property(p => p.RoleLevelKeyword).IsRequired();
            //WithOptional (навигационное свойство может иметь один или ноль экземпляров)

            builder.HasOne(p => p.RoleLevel).WithMany(p => p.Roles).HasForeignKey(fk => fk.RoleLevelId);
            //builder.HasOne(p => p.RoleLevel).WithMany(p => p.Roles).HasForeignKey(fk => fk.RoleLevelKeyword);
        }
    }

    /// <summary>
    /// Enum содержит соответствие Keyword'а роли и её Id
    /// Keyword роли удобно используется в авторизации для указания ролей доступа
    /// </summary>
    /// <remarks>
    /// При добавлении/редактивровании ролей в БД, необходимо корректировать Enum
    /// </remarks>
    /// <seealso cref="Helpers.StaticManager.Roles"/>
    public enum Roles : int
    {
        Global = 1,
        Admin = 2,
        Teacher = 3,
        Student = 4,
        Parent = 5,
    }

    public static class RolesExtension
    {
        public static List<int> GetValues(this Roles[] roles)
        {
            List<int> roleIds = new List<int>();
            foreach (var role in roles)
            {
                roleIds.Add((int)role);
            }
            return roleIds;
        }
    }

    //public enum RoleUser : int
    //{
    //    [Dec(NameAtr1: "asd0", NameAtr2: "dsa0")]
    //    Global = 1,
    //    [Dec(NameAtr1: "asd1", NameAtr2: "dsa1")]
    //    Student = 2,
    //    [Dec(NameAtr1: "asd2", NameAtr2: "dsa2")]
    //    Parent = 3,
    //    [Dec(NameAtr1: "asd3", NameAtr2: "dsa3")]
    //    Teacher = 4
    //}

    //public class DecAttribute : Attribute
    //{
    //    public string NameAtr1 { get; }
    //    public string NameAtr2 { get; }

    //    public DecAttribute(string NameAtr1, string NameAtr2)
    //    {
    //        this.NameAtr1 = NameAtr1;
    //        this.NameAtr2 = NameAtr2;
    //    }
    //}
}