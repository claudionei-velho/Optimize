using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class FuUtilMap : EntityTypeConfiguration<FuUtil> {
    public FuUtilMap() {
      // Primary Key
      this.HasKey(t => new { t.EmpresaId, t.Hora });

      // Table, Properties & Column Mappings
      this.ToTable("FuUteis", "opc");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.Hora).HasColumnName("Hora").IsRequired();
      this.Property(t => t.Faixa).HasColumnName("Faixa");
      this.Property(t => t.QtdLinhas).HasColumnName("QtdLinhas");
      this.Property(t => t.Viagens).HasColumnName("Viagens");
      this.Property(t => t.Extensao).HasColumnName("Extensao").HasPrecision(24, 6);
      this.Property(t => t.Veiculos).HasColumnName("Veiculos");
      this.Property(t => t.Percurso).HasColumnName("Percurso").HasPrecision(24, 6);      
    }
  }
}
