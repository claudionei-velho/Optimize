using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PrLinhaMap : EntityTypeConfiguration<PrLinha> {
    public PrLinhaMap() {
      // Primary Key
      this.HasKey(t => t.Id);
      
      // Table, Properties & Column Mappings
      this.ToTable("PrLinhas", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.PeriodoId).HasColumnName("PeriodoId");
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.Termino).HasColumnName("Termino");
      this.Property(t => t.CicloAB).HasColumnName("CicloAB");
      this.Property(t => t.CicloBA).HasColumnName("CicloBA");
      this.Property(t => t.CVeiculoId).HasColumnName("CVeiculoId");
      this.Property(t => t.OcupacaoId).HasColumnName("OcupacaoId");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Foreign Keys (Relationships)
      this.HasOptional(t => t.CVeiculo)
          .WithMany(t => t.PrLinhas).HasForeignKey(d => d.CVeiculoId).WillCascadeOnDelete(false);

      this.HasRequired(t => t.EPeriodo)
          .WithMany(t => t.PrLinhas).HasForeignKey(d => d.PeriodoId).WillCascadeOnDelete(false);

      this.HasRequired(t => t.Linha)
          .WithMany(t => t.PrLinhas).HasForeignKey(d => d.LinhaId).WillCascadeOnDelete(false);

      this.HasOptional(t => t.Ocupacao)
          .WithMany(t => t.PrLinhas).HasForeignKey(d => d.OcupacaoId).WillCascadeOnDelete(false);
    }
  }
}
