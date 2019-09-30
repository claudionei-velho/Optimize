using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ISinoticoMap : EntityTypeConfiguration<ISinotico> {
    public ISinoticoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Classe)
          .IsRequired().HasMaxLength(32);

      this.Property(t => t.Denominacao)
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Unidade)
          .HasMaxLength(16);

      // Table & Column Mappings
      this.ToTable("ISinotico");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Classe).HasColumnName("Classe");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao");
      this.Property(t => t.Unidade).HasColumnName("Unidade");
    }
  }
}
