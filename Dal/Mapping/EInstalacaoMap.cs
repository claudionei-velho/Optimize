using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class EInstalacaoMap : EntityTypeConfiguration<EInstalacao> {
    public EInstalacaoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("EInstalacoes", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.InstalacaoId).HasColumnName("InstalacaoId");
      this.Property(t => t.PropositoId).HasColumnName("PropositoId");
      this.Property(t => t.AreaCoberta).HasColumnName("AreaCoberta");
      this.Property(t => t.AreaTotal).HasColumnName("AreaTotal");
      this.Property(t => t.QtdEmpregados).HasColumnName("QtdEmpregados");
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.Termino).HasColumnName("Termino");
      this.Property(t => t.Efluentes).HasColumnName("Efluentes");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Instalacao)
          .WithMany(t => t.EInstalacoes).HasForeignKey(d => d.InstalacaoId);

      this.HasRequired(t => t.FInstalacao)
          .WithMany(t => t.EInstalacoes).HasForeignKey(d => d.PropositoId);
    }
  }
}
