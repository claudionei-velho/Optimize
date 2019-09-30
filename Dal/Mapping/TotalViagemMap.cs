using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class TotalViagemMap : EntityTypeConfiguration<TotalViagem> {
    public TotalViagemMap() {
      // Primary Key
      this.HasKey(t => new { t.LinhaId, t.DiaId, t.Sentido, t.PeriodoId });

      // Table, Properties & Column Mappings
      this.ToTable("TotalViagens", "opc");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.DiaId).HasColumnName("DiaId");
      this.Property(t => t.Sentido).HasColumnName("Sentido");
      this.Property(t => t.PeriodoId).HasColumnName("PeriodoId");
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.Termino).HasColumnName("Termino");
      this.Property(t => t.Ciclo).HasColumnName("Ciclo");
      this.Property(t => t.QtdViagens).HasColumnName("QtdViagens");
      this.Property(t => t.QtdAtendimentos).HasColumnName("QtdAtendimentos");
      this.Property(t => t.KmDia).HasColumnName("KmDia");
      this.Property(t => t.KmSemana).HasColumnName("KmSemana");
      this.Property(t => t.KmMes).HasColumnName("KmMes");

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Linha)
          .WithMany(f => f.TotalViagens).HasForeignKey(d => d.LinhaId).WillCascadeOnDelete(false);

      this.HasRequired(t => t.PrLinha)
          .WithMany(f => f.TotalViagens).HasForeignKey(d => d.PeriodoId).WillCascadeOnDelete(false);
    }
  }
}
