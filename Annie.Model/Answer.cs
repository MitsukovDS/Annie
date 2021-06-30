using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public partial class Answer
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int QuestionId { get; set; }
        private string answerText;
        public string AnswerText
        {
            get => answerText;
            set => answerText = value?.Trim();
        }
        public int SequenceNumber { get; set; }
        public bool IsCorrect { get; set; }
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
        public bool IsDeleted { get; set; }

        public virtual Question Question { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual UploadedFile UploadedFile { get; set; }
    }

    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable(name: "Answer", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            //builder.HasAlternateKey(ak => new { ak.QuestionId, ak.SequenceNumber, ak.IsDeleted });

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.QuestionId).IsRequired();
            builder.Property(p => p.AnswerText).IsRequired();
            builder.Property(p => p.SequenceNumber).IsRequired();
            builder.Property(p => p.IsCorrect).IsRequired();
            builder.Property(p => p.IsDeleted).IsRequired();
            builder.Ignore(p => p.Image);

            builder.HasOne(p => p.Question).WithMany(p => p.Answers).HasForeignKey(fk => fk.QuestionId);
            builder.HasOne(p => p.UploadedFile).WithMany(p => p.Answers).HasForeignKey(fk => fk.ImageId);
        }
    }

    public partial class Answer : IEquatable<Answer>
    {
        public override bool Equals(object obj) => this.Equals(obj as Answer);

        public override int GetHashCode()
        {
            return
                QuestionId.GetHashCode() ^
                AnswerText.GetHashCode() ^
                SequenceNumber.GetHashCode() ^
                IsCorrect.GetHashCode() ^
                ImageId.GetHashCode() ^
                IsDeleted.GetHashCode();
        }

        public bool Equals(Answer other)
        {
            if (other == null)
                return false;

            bool equals =
                (
                    this.QuestionId.Equals(0) ||
                    other.QuestionId.Equals(0) ||
                    this.QuestionId.Equals(other.QuestionId)
                ) &&
                (
                    object.ReferenceEquals(this.AnswerText, other.AnswerText) ||
                    this.AnswerText != null &&
                    this.AnswerText.Equals(other.AnswerText)
                ) &&
                this.SequenceNumber.Equals(other.SequenceNumber) &&
                this.IsCorrect.Equals(other.IsCorrect) &&
                this.ImageId.Equals(other.ImageId) &&
                this.IsDeleted.Equals(other.IsDeleted);

            return equals;
        }
    }
}
