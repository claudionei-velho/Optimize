using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PaisMap : EntityTypeConfiguration<Pais> {
    public PaisMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Id)
          .IsRequired().HasMaxLength(8);

      this.Property(t => t.Nome)
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Capital)
          .HasMaxLength(32);

      this.Property(t => t.Continente)
          .HasMaxLength(32);

      // Table & Column Mappings
      this.ToTable("Paises");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Nome).HasColumnName("Nome");
      this.Property(t => t.Capital).HasColumnName("Capital");
      this.Property(t => t.Continente).HasColumnName("Continente");
    }
  }
}
