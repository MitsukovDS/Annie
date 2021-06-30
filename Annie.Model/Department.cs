using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class Department
    {
        /// <summary>
        /// список классов, групп и т.д.
        /// </summary>
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }
        public int OlympiadTypeId { get; set; }

        public virtual ICollection<Olympiad> Olympiad { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Diploma> Diplomas { get; set; }
        public virtual OlympiadType OlympiadType { get; set; }
    }

    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable(name: "Department", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.Keyword);
            builder.HasAlternateKey(ak => ak.Title);

            builder.HasOne(p => p.OlympiadType).WithMany(p => p.Departments).HasForeignKey(fk => fk.OlympiadTypeId);
        }
    }

    //public enum Departments: int
    //{
    //    class1 = 1,
    //    class2 = 2,
    //}
}
