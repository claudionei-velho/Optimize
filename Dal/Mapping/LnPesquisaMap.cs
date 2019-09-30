using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class LnPesquisaMap : EntityTypeConfiguration<LnPesquisa> {
    public LnPesquisaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Responsavel)
          .HasMaxLength(64);

      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("LnPesquisas", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.PesquisaId).HasColumnName("PesquisaId");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.Uteis).HasColumnName("Uteis");
      this.Property(t => t.Sabados).HasColumnName("Sabados");
      this.Property(t => t.Domingos).HasColumnName("Domingos");
      this.Property(t => t.Responsavel).HasColumnName("Responsavel");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Linha)
          .WithMany(t => t.LnPesquisas).HasForeignKey(d => d.LinhaId);

      this.HasRequired(t => t.Pesquisa)
          .WithMany(t => t.LnPesquisas).HasForeignKey(d => d.PesquisaId);
    }
  }
}
