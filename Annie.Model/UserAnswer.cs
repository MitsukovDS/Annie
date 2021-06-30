using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public partial class UserAnswer
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public int UserOlympiadId { get; set; }
        public int AnswerId { get; set; }
        public int QuestionId { get; set; } // TODO: при изменении вопроса (старый Id IsDeleted, появляется новый Id) нужно перезаписывать, либо это поле не нужно
        public decimal? CostAnswer { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User User { get; set; }
        public virtual UserOlympiad UserOlympiad { get; set; }
        public virtual Answer Answer { get; set; }
        public virtual Question Question { get; set; }
    }

    public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.ToTable(name: "UserAnswer", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.CreatedUserId).IsRequired();
            //builder.Property(p => p.ParticipantUserId).IsRequired();
            builder.Property(p => p.UserOlympiadId).IsRequired();
            builder.Property(p => p.AnswerId).IsRequired();
            builder.Property(p => p.QuestionId).IsRequired();
            builder.Property(p => p.IsDeleted).IsRequired();

            builder.HasOne(p => p.User).WithMany(p => p.UserAnswers).HasForeignKey(fk => fk.CreatedUserId);
            //builder.HasOne(p => p.User).WithMany(p => p.UserAnswers).HasForeignKey(fk => fk.ParticipantUserId);
            builder.HasOne(p => p.UserOlympiad).WithMany(p => p.UserAnswers).HasForeignKey(fk => fk.UserOlympiadId);
            builder.HasOne(p => p.Answer).WithMany(p => p.UserAnswers).HasForeignKey(fk => fk.AnswerId);
            builder.HasOne(p => p.Question).WithMany(p => p.UserAnswers).HasForeignKey(fk => fk.QuestionId);
        }
    }

    public partial class UserAnswer
    {
        public override bool Equals(object obj) => this.Equals(obj as UserAnswer);

        public override int GetHashCode()
        {
            return
                UserOlympiadId.GetHashCode() ^
                AnswerId.GetHashCode() ^
                QuestionId.GetHashCode() ^
                IsDeleted.GetHashCode();
        }

        public bool Equals(UserAnswer other)
        {
            if (other == null)
                return false;

            bool equals =
                this.UserOlympiadId.Equals(other.UserOlympiadId) &&
                this.AnswerId.Equals(other.AnswerId) &&
                this.QuestionId.Equals(other.QuestionId) &&
                this.IsDeleted.Equals(other.IsDeleted);

            return equals;
        }
    }
}
