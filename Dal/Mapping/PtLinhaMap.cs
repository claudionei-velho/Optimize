using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PtLinhaMap : EntityTypeConfiguration<PtLinha> {
    public PtLinhaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Sentido)
          .IsRequired().IsFixedLength().HasMaxLength(2);

      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("PtLinhas", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.Sentido).HasColumnName("Sentido");
      this.Property(t => t.PontoId).HasColumnName("PontoId");
      this.Property(t => t.Fluxo).HasColumnName("Fluxo");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Linha)
          .WithMany(t => t.PtLinhas).HasForeignKey(d => d.LinhaId);

      this.HasRequired(t => t.Ponto)
          .WithMany(t => t.PtLinhas).HasForeignKey(d => d.PontoId);
    }
  }
}
