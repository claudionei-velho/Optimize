using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class FrotaEtariaMap : EntityTypeConfiguration<FrotaEtaria> {
    public FrotaEtariaMap() {
      // Primary Key
      this.HasKey(t => new { t.EmpresaId, t.EtariaId });

      // Table, Properties & Column Mappings
      this.ToTable("FrotaEtaria", "opc");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.EtariaId).HasColumnName("EtariaId");
      this.Property(t => t.Micro).HasColumnName("Micro");
      this.Property(t => t.Mini).HasColumnName("Mini");
      this.Property(t => t.Midi).HasColumnName("Midi");
      this.Property(t => t.Basico).HasColumnName("Basico");
      this.Property(t => t.Padron).HasColumnName("Padron");
      this.Property(t => t.Especial).HasColumnName("Especial");
      this.Property(t => t.Articulado).HasColumnName("Articulado");
      this.Property(t => t.BiArticulado).HasColumnName("BiArticulado");      
      this.Property(t => t.Frota).HasColumnName("Frota");
      this.Property(t => t.EqvIdade).HasColumnName("EqvIdade");

      // Foreign Keys (Relationships)
      this.HasRequired(p => p.Empresa)
          .WithMany(f => f.FrotaEtarias).HasForeignKey(k => k.EmpresaId).WillCascadeOnDelete(false);

      this.HasRequired(p => p.FxEtaria)
          .WithMany(f => f.FrotaEtarias).HasForeignKey(k => k.EtariaId).WillCascadeOnDelete(false);
    }
  }
}
