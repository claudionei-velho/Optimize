using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class FxEtariaMap : EntityTypeConfiguration<FxEtaria> {
    public FxEtariaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("FxEtarias", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao").IsRequired().HasMaxLength(32);
      this.Property(t => t.Minimo).HasColumnName("Minimo").IsRequired();
      this.Property(t => t.Maximo).HasColumnName("Maximo").IsRequired();
    }
  }
}
