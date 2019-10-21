using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class HorarioMap : EntityTypeConfiguration<Horario> {
    public HorarioMap() {
      // Primary Key
      this.HasKey(t => t.Id);
        
      // Table, Properties & Column Mappings
      this.ToTable("Horarios", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.DiaId).HasColumnName("DiaId");
      this.Property(t => t.Sentido).HasColumnName("Sentido").IsRequired().IsFixedLength().HasMaxLength(2);
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.AtendimentoId).HasColumnName("AtendimentoId");
      this.Property(t => t.PeriodoId).HasColumnName("PeriodoId")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Relationships
      this.HasOptional(t => t.Atendimento)
          .WithMany(t => t.Horarios).HasForeignKey(d => d.AtendimentoId);

      this.HasRequired(t => t.Linha)
          .WithMany(t => t.Horarios).HasForeignKey(d => d.LinhaId).WillCascadeOnDelete(false);

      this.HasOptional(t => t.PrLinha)
          .WithMany(t => t.Horarios).HasForeignKey(d => d.PeriodoId);
    }
  }
}