using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PtAtendimentoMap : EntityTypeConfiguration<PtAtendimento> {
    public PtAtendimentoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Sentido)
          .IsRequired().IsFixedLength().HasMaxLength(2);

      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("PtAtendimentos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.AtendimentoId).HasColumnName("AtendimentoId");
      this.Property(t => t.Sentido).HasColumnName("Sentido");
      this.Property(t => t.PontoId).HasColumnName("PontoId");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Atendimento)
          .WithMany(t => t.PtAtendimentos).HasForeignKey(d => d.AtendimentoId);

      this.HasRequired(t => t.Ponto)
          .WithMany(t => t.PtAtendimentos).HasForeignKey(d => d.PontoId);
    }
  }
}
