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
    public class ParticipantStatus
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }

        public virtual ICollection<UserResult> UserResults { get; set; }
    }

    public class ParticipantStatusConfiguration : IEntityTypeConfiguration<ParticipantStatus>
    {
        public void Configure(EntityTypeBuilder<ParticipantStatus> builder)
        {
            builder.ToTable("ParticipantStatus", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.Keyword);
            builder.HasAlternateKey(ak => ak.Title);
        }
    }

    public enum ParticipantStatuses : int
    {
        Winner = 1,
        PrizeWinner = 2,
        Participant = 3
    }
}