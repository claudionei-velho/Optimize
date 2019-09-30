using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class LnTerminalMap : EntityTypeConfiguration<LnTerminal> {
    public LnTerminalMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("LnTerminais", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.TerminalId).HasColumnName("TerminalId");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.Uteis).HasColumnName("Uteis");
      this.Property(t => t.UteisFluxo).HasColumnName("UteisFluxo");
      this.Property(t => t.Sabados).HasColumnName("Sabados");
      this.Property(t => t.SabadosFluxo).HasColumnName("SabadosFluxo");
      this.Property(t => t.Domingos).HasColumnName("Domingos");
      this.Property(t => t.DomingosFluxo).HasColumnName("DomingosFluxo");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Terminal)
          .WithMany(t => t.LnTerminais).HasForeignKey(d => d.TerminalId);

      this.HasRequired(t => t.Linha)
          .WithMany(t => t.LnTerminais).HasForeignKey(d => d.LinhaId);
    }
  }
}
