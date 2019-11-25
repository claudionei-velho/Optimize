using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ViagemMap : EntityTypeConfiguration<Viagem> {
    public ViagemMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Viagens", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.Item).HasColumnName("Item");
      this.Property(t => t.Data).HasColumnName("Data");
      this.Property(t => t.Sentido).HasColumnName("Sentido")
          .IsRequired().IsFixedLength().HasMaxLength(2);

      this.Property(t => t.HorarioId).HasColumnName("HorarioId");
      this.Property(t => t.PontoId).HasColumnName("PontoId");
      this.Property(t => t.VeiculoId).HasColumnName("VeiculoId");
      this.Property(t => t.Chegada).HasColumnName("Chegada");
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.Termino).HasColumnName("Termino");
      this.Property(t => t.PeriodoId).HasColumnName("PeriodoId");
      this.Property(t => t.Passageiros).HasColumnName("Passageiros");
      this.Property(t => t.Inicial).HasColumnName("Inicial");
      this.Property(t => t.Final).HasColumnName("Final");
      this.Property(t => t.Responsavel).HasColumnName("Responsavel").HasMaxLength(32);
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Relationships
      this.HasRequired(t => t.LnPesquisa)
          .WithMany(t => t.Viagens).HasForeignKey(d => d.LinhaId)
          .WillCascadeOnDelete(false);

      this.HasOptional(t => t.Horario)
          .WithMany(t => t.Viagens).HasForeignKey(d => d.HorarioId)
          .WillCascadeOnDelete(false);

      this.HasOptional(t => t.PtLinha)
          .WithMany(t => t.Viagens).HasForeignKey(d => d.PontoId)
          .WillCascadeOnDelete(false);

      this.HasOptional(t => t.Veiculo)
          .WithMany(t => t.Viagens).HasForeignKey(d => d.VeiculoId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.PrLinha)
          .WithMany(t => t.Viagens).HasForeignKey(d => d.PeriodoId)
          .WillCascadeOnDelete(false);
    }
  }
}
