using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public class DiplomaElement
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<DiplomaProperty> DiplomaProperties { get; set; }
    }

    public class DiplomaElementConfiguration : IEntityTypeConfiguration<DiplomaElement>
    {
        public void Configure(EntityTypeBuilder<DiplomaElement> builder)
        {
            builder.ToTable(name: "DiplomaElement", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasAlternateKey(ak => ak.Title);
        }
    }

    public enum DiplomaElements : int
    {
        NumberDiploma = 1,
        FullName = 2,
        SurnameFirstname = 3,
        InstituteTitle = 4,
        Discipline = 5,
        Department = 6,
        TotalResult = 7,
        RatingPlace = 8,
        TourNumber = 9,
        StudyYear = 10,
        Session = 11,
        QRCode = 12,
        ParticipantStatus = 13,
        CountAllPreparedParticipants = 14,
        CountWinnerStatus = 15,
        CountPrizeWinnerStatus = 16,
        CountParticipantStatus = 17
    }
}
