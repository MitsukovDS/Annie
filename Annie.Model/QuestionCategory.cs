using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class QuestionCategory
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<OlympiadQuestion> OlympiadQuestions { get; set; }
    }

    public class QuestionCategoryConfiguration : IEntityTypeConfiguration<QuestionCategory>
    {
        public void Configure(EntityTypeBuilder<QuestionCategory> builder)
        {
            builder.ToTable("QuestionCategory", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.Keyword);
            builder.HasAlternateKey(ak => ak.Title);
        }
    }


    public enum Categories : int
    {
        A = 1,
        B = 2,
        C = 3
    }
}
