using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public partial class Question
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CategoryId { get; set; }
        public int DisciplineId { get; set; }
        public int DepartmentId { get; set; }
        public int OlympiadTypeId { get; set; }
        private string questionText;
        public string QuestionText
        {
            get => questionText;
            set => questionText = value?.Trim();
        }
        public IFormFile Image { get; set; }
        private int? imageId;
        public int? ImageId
        {
            get => imageId;
            set
            {
                if (value == 0)
                    imageId = null;
                else
                    imageId = value;
            }
        }
        public int CountReference { get; set; }
        public DateTime? LastUseDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual QuestionCategory QuestionCategory { get; set; }
        public virtual Discipline Discipline { get; set; }
        public virtual Department Department { get; set; }
        public virtual OlympiadType OlympiadType { get; set; }
        public virtual UploadedFile UploadedFile { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<OlympiadQuestion> OlympiadQuestions { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }

    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable(name: "Question", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.CategoryId).IsRequired();
            builder.Property(p => p.DisciplineId).IsRequired();
            builder.Property(p => p.DepartmentId).IsRequired();
            builder.Property(p => p.OlympiadTypeId).IsRequired();
            builder.Property(p => p.QuestionText).IsRequired();
            builder.Property(p => p.CountReference).IsRequired();
            builder.Property(p => p.CountReference).HasDefaultValue(0);
            builder.Property(p => p.IsDeleted).IsRequired();
            builder.Ignore(p => p.Image);

            builder.HasOne(p => p.QuestionCategory).WithMany(p => p.Questions).HasForeignKey(fk => fk.CategoryId);
            builder.HasOne(p => p.OlympiadType).WithMany(p => p.Questions).HasForeignKey(fk => fk.OlympiadTypeId);
            builder.HasOne(p => p.Discipline).WithMany(p => p.Questions).HasForeignKey(fk => fk.DisciplineId);
            builder.HasOne(p => p.Department).WithMany(p => p.Questions).HasForeignKey(fk => fk.DepartmentId);
            builder.HasOne(p => p.UploadedFile).WithMany(p => p.Questions).HasForeignKey(fk => fk.ImageId);
        }
    }

    public partial class Question : IEquatable<Question>
    {
        public override bool Equals(object obj) => this.Equals(obj as Question);

        public override int GetHashCode()
        {
            return
                CategoryId.GetHashCode() ^
                DisciplineId.GetHashCode() ^
                DepartmentId.GetHashCode() ^
                OlympiadTypeId.GetHashCode() ^
                QuestionText.GetHashCode() ^
                ImageId.GetHashCode() ^
                IsDeleted.GetHashCode();
        }

        public bool Equals(Question other)
        {
            if (other == null)
                return false;

            bool equals =
                this.CategoryId.Equals(other.CategoryId) &&
                this.DisciplineId.Equals(other.DisciplineId) &&
                this.ImageId.Equals(other.ImageId) &&
                this.IsDeleted.Equals(other.IsDeleted) &&
                this.DepartmentId.Equals(other.DepartmentId) &&
                this.OlympiadTypeId.Equals(other.OlympiadTypeId) &&
                (
                     object.ReferenceEquals(this.QuestionText, other.QuestionText) ||
                     this.QuestionText != null &&
                     this.QuestionText.Equals(other.QuestionText)
                );

            if (equals && (this.Answers != null || other.Answers != null))
            {
                equals =
                    this.Answers != null &&
                    other.Answers != null &&
                    this.Answers.OrderBy(a => a.SequenceNumber).SequenceEqual(other.Answers.OrderBy(a => a.SequenceNumber));
            }

            return equals;
        }
    }
}
