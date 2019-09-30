using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class CVeiculoAttMap : EntityTypeConfiguration<CVeiculoAtt> {
    public CVeiculoAttMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Caracteristica)
          .IsRequired().HasMaxLength(128);

      this.Property(t => t.Unidade)
          .HasMaxLength(16);

      // Table & Column Mappings
      this.ToTable("CVeiculosAtt", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Caracteristica).HasColumnName("Caracteristica");
      this.Property(t => t.Unidade).HasColumnName("Unidade");
      this.Property(t => t.Variavel).HasColumnName("Variavel");
    }
  }
}
