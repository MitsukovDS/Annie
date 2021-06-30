using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Model
{
    public partial class DiplomaProperty
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DiplomaId { get; set; }
        public int DiplomaElementId { get; set; }
        public float Left { get; set; }
        public float Bottom { get; set; }
        public float Width { get; set; }
        public int Alignment { get; set; }
        public float FontSize { get; set; }
        public FontWeight FontWeight { get; set; }
        public string FontColorHex { get; set; }
        public float LineSpacing { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Diploma Diploma { get; set; }
        public virtual DiplomaElement DiplomaElement { get; set; }
    }

    public class DiplomaPropertyConfiguration : IEntityTypeConfiguration<DiplomaProperty>
    {
        public void Configure(EntityTypeBuilder<DiplomaProperty> builder)
        {
            builder.ToTable(name: "DiplomaProperty", schema: SchemasPostgreSql.PublicSchema);
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.DiplomaId).IsRequired();
            builder.Property(p => p.DiplomaElementId).IsRequired();
            builder.Property(p => p.Left).IsRequired();
            builder.Property(p => p.Bottom).IsRequired();
            builder.Property(p => p.Width).IsRequired();
            builder.Property(p => p.Alignment).HasConversion<int>().HasDefaultValue(0).IsRequired();
            builder.Property(p => p.FontSize).IsRequired();
            builder.Property(p => p.FontWeight).HasConversion<int>().HasDefaultValue(FontWeight.Light).IsRequired();
            builder.Property(p => p.FontColorHex).IsRequired();
            builder.Property(p => p.LineSpacing).IsRequired();
            builder.Property(p => p.IsDeleted).IsRequired();

            builder.HasOne(p => p.Diploma).WithMany(p => p.DiplomaProperties).HasForeignKey(fk => fk.DiplomaId);
            builder.HasOne(p => p.DiplomaElement).WithMany(p => p.DiplomaProperties).HasForeignKey(fk => fk.DiplomaElementId);
        }
    }

    public partial class DiplomaProperty : IEquatable<DiplomaProperty>
    {
        public override bool Equals(object obj) => this.Equals(obj as DiplomaProperty);

        public override int GetHashCode()
        {
            return
                DiplomaId.GetHashCode() ^
                DiplomaElementId.GetHashCode() ^
                Left.GetHashCode() ^
                Bottom.GetHashCode() ^
                Width.GetHashCode() ^
                Alignment.GetHashCode() ^
                FontSize.GetHashCode() ^
                FontWeight.GetHashCode() ^
                FontColorHex.GetHashCode() ^
                LineSpacing.GetHashCode() ^
                IsDeleted.GetHashCode();
        }

        public bool Equals(DiplomaProperty other)
        {
            if (other == null)
                return false;

            bool equals =
                this.DiplomaId.Equals(other.DiplomaId) &&
                this.DiplomaElementId.Equals(other.DiplomaElementId) &&
                this.Left.Equals(other.Left) &&
                this.Bottom.Equals(other.Bottom) &&
                this.Width.Equals(other.Width) &&
                this.Alignment.Equals(other.Alignment) &&
                this.FontSize.Equals(other.FontSize) &&
                (
                    object.ReferenceEquals(this.FontColorHex, other.FontColorHex) ||
                    this.FontColorHex != null ||
                    this.FontColorHex.Equals(other.FontColorHex)
                ) &&
                this.FontWeight.Equals(other.FontWeight) &&
                this.LineSpacing.Equals(other.LineSpacing) &&
                this.IsDeleted.Equals(other.IsDeleted);

            return equals;
        }
    }

    public enum FontWeight : int
    {
        Light = 0,
        Bold = 1
    }
}
