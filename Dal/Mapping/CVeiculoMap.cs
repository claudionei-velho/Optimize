using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class CVeiculoMap : EntityTypeConfiguration<CVeiculo> {
    public CVeiculoMap() {
      // Primary Key
      this.HasKey(t => t.Id);
        
      // Table, Properties & Column Mappings
      this.ToTable("CVeiculos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Categoria).HasColumnName("Categoria").IsRequired().HasMaxLength(16);
      this.Property(t => t.Classe).HasColumnName("Classe").IsRequired().HasMaxLength(32);
      this.Property(t => t.Minimo).HasColumnName("Minimo");
      this.Property(t => t.Maximo).HasColumnName("Maximo");
    }
  }
}
