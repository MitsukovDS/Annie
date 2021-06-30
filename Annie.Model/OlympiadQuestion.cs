using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class OlympiadQuestion
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OlympiadId { get; set; }
        public int QuestionId { get; set; }
        public int QuestionCategoryId { get; set; }
        public int SequenceNumber { get; set; }
        public decimal? CostQuestion { get; set; }
        public decimal? CostAnswer { get; set; }

        private float widthImageQuestion = 30f;
        public float WidthImageQuestion
        {
            get => widthImageQuestion;
            set
            {
                widthImageQuestion = value;
                if (value > 100f)
                    widthImageQuestion = 100f;
                if (value < 0)
                    widthImageQuestion = 0;
            }
        }
        public bool AnswersAreVisible { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Olympiad Olympiad { get; set; }
        public virtual Question Question { get; set; }
        public virtual QuestionCategory QuestionCategory { get; set; }
        public virtual ICollection<UserResultDetail> UserResultDetails { get; set; }
    }

    public class OlympiadQuestionConfiguration : IEntityTypeConfiguration<OlympiadQuestion>
    {
        public void Configure(EntityTypeBuilder<OlympiadQuestion> builder)
        {
            builder.ToTable(name: "OlympiadQuestion", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.OlympiadId).IsRequired();
            builder.Property(p => p.QuestionId).IsRequired();
            builder.Property(p => p.SequenceNumber).IsRequired();
            builder.Property(p => p.WidthImageQuestion).IsRequired();
            builder.Property(p => p.WidthImageQuestion).HasDefaultValue(30f);
            builder.Property(p => p.AnswersAreVisible).IsRequired();
            builder.Property(p => p.AnswersAreVisible).HasDefaultValue(true);
            builder.Property(p => p.IsDeleted).IsRequired();

            //могут быть совпадающие удалённые запросы
            //builder.HasAlternateKey(ak => new { ak.OlympiadId, ak.QuestionId, ak.IsDeleted });
            //builder.HasAlternateKey(ak => new { ak.OlympiadId, ak.SequenceNumber, ak.IsDeleted });

            builder.HasOne(p => p.Olympiad).WithMany(p => p.OlympiadQuestions).HasForeignKey(fk => fk.OlympiadId);
            builder.HasOne(p => p.Question).WithMany(p => p.OlympiadQuestions).HasForeignKey(fk => fk.QuestionId);
            builder.HasOne(p => p.QuestionCategory).WithMany(p => p.OlympiadQuestions).HasForeignKey(fk => fk.QuestionCategoryId);
        }
    }
}
