using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class RenovacaoMap : EntityTypeConfiguration<Renovacao> {
    public RenovacaoMap() {
      // Primary Key
      this.HasKey(t => t.Id);
          
      // Table, Properties & Column Mappings
      this.ToTable("Renovacao", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.Ano).HasColumnName("Ano");
      this.Property(t => t.Mes).HasColumnName("Mes");
      this.Property(t => t.DiaId).HasColumnName("DiaId");
      this.Property(t => t.Indice).HasColumnName("Indice");
      this.Property(t => t.Referencia).HasColumnName("Referencia")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Relationships
      this.HasRequired(t => t.Linha)
          .WithMany(t => t.Renovacoes).HasForeignKey(d => d.LinhaId);
    }
  }
}
