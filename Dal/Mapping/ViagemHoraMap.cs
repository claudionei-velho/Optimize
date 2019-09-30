using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ViagemHoraMap : EntityTypeConfiguration<ViagemHora> {
    public ViagemHoraMap() {
      // Primary Key
      this.HasKey(t => new { t.EmpresaId, t.LinhaId, t.Hora });

      // Table, Properties & Column Mappings
      this.ToTable("ViagensHora", "opc");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.Hora).HasColumnName("Hora").IsRequired();
      this.Property(t => t.UteisAB).HasColumnName("UteisAB");
      this.Property(t => t.UteisBA).HasColumnName("UteisBA");
      this.Property(t => t.SabadosAB).HasColumnName("SabadosAB");
      this.Property(t => t.SabadosBA).HasColumnName("SabadosBA");
      this.Property(t => t.DomingosAB).HasColumnName("DomingosAB");
      this.Property(t => t.DomingosBA).HasColumnName("DomingosBA");

      // Foreign Keys (Relationships)
      this.HasRequired(p => p.Linha)
          .WithMany(f => f.ViagensHora).HasForeignKey(k => k.LinhaId).WillCascadeOnDelete(false);
    }
  }
}
