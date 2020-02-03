using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PaisMap : EntityTypeConfiguration<Pais> {
    public PaisMap() {
      // Primary Key
      this.HasKey(t => t.Id);       

      // Table, Properties & Column Mappings
      this.ToTable("Paises");
      this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(8);
      this.Property(t => t.Nome).HasColumnName("Nome")
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Capital).HasColumnName("Capital").HasMaxLength(32);
      this.Property(t => t.Continente).HasColumnName("Continente").HasMaxLength(32);
    }
  }
}
