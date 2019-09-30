using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ViaMap : EntityTypeConfiguration<Via> {
    public ViaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Denominacao)
          .IsRequired().HasMaxLength(64);

      // Table & Column Mappings
      this.ToTable("Vias", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao");
    }
  }
}
