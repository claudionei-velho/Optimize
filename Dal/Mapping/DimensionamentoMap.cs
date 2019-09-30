using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class DimensionamentoMap : EntityTypeConfiguration<Dimensionamento> {
    public DimensionamentoMap() {
      // Primary Key
      this.HasKey(t => new { t.LinhaId, t.DiaId, t.PeriodoId, t.Sentido });

      // Table, Properties & Column Mappings
      this.ToTable("Dimensionar", "opc");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.DiaId).HasColumnName("DiaId");
      this.Property(t => t.PeriodoId).HasColumnName("PeriodoId");
      this.Property(t => t.Sentido).HasColumnName("Sentido");
      this.Property(t => t.QtdViagens).HasColumnName("QtdViagens");
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.Termino).HasColumnName("Termino");
      this.Property(t => t.Ociosidade).HasColumnName("Ociosidade");
      this.Property(t => t.Passageiros).HasColumnName("Passageiros");
      this.Property(t => t.Ajustado).HasColumnName("Ajustado");
      this.Property(t => t.Critica).HasColumnName("Critica");
      this.Property(t => t.CriticaAjuste).HasColumnName("CriticaAjuste");
      this.Property(t => t.Desvio).HasColumnName("Desvio");
      this.Property(t => t.DesvioAjuste).HasColumnName("DesvioAjuste");
      this.Property(t => t.LotacaoP).HasColumnName("LotacaoP");
      this.Property(t => t.Veiculos).HasColumnName("Veiculos");
      this.Property(t => t.CicloAB).HasColumnName("CicloAB");
      this.Property(t => t.CicloBA).HasColumnName("CicloBA");
      this.Property(t => t.Extensao).HasColumnName("Extensao");

      // Relationships
      this.HasRequired(t => t.Linha)
          .WithMany(t => t.Dimensionamentos).HasForeignKey(d => d.LinhaId).WillCascadeOnDelete(false);

      this.HasRequired(t => t.PrLinha)
          .WithMany(t => t.Dimensionamentos).HasForeignKey(d => d.PeriodoId).WillCascadeOnDelete(false);
    }
  }
}
