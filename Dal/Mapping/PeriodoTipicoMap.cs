using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PeriodoTipicoMap : EntityTypeConfiguration<PeriodoTipico> {
    public PeriodoTipicoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("PeriodosTipicos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.PeriodoId).HasColumnName("PeriodoId").IsRequired();
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.Termino).HasColumnName("Termino");
      this.Property(t => t.QtdViagens).HasColumnName("QtdViagens");
      this.Property(t => t.CicloAB).HasColumnName("CicloAB");
      this.Property(t => t.CicloBA).HasColumnName("CicloBA");

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Linha)
          .WithMany(f => f.PeriodosTipicos).HasForeignKey(k => k.LinhaId).WillCascadeOnDelete(false);

      this.HasRequired(t => t.EPeriodo)
          .WithMany(f => f.PeriodosTipicos).HasForeignKey(k => k.PeriodoId).WillCascadeOnDelete(false);
    }
  }
}
