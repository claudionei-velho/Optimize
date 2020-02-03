using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class TarifaMap : EntityTypeConfiguration<Tarifa> {
    public TarifaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Tarifas", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.Referencia).HasColumnName("Referencia");
      this.Property(t => t.Valor).HasColumnName("Valor");
      this.Property(t => t.Decreto).HasColumnName("Decreto").HasMaxLength(128);

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.Tarifas).HasForeignKey(d => d.EmpresaId);
    }
  }
}
