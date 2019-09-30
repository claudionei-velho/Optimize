using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class OfertaMap : EntityTypeConfiguration<Oferta> {
    public OfertaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("Ofertas", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.Ano).HasColumnName("Ano");
      this.Property(t => t.Mes).HasColumnName("Mes");
      this.Property(t => t.Categoria).HasColumnName("Categoria");
      this.Property(t => t.Passageiros).HasColumnName("Passageiros");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Linha)
          .WithMany(t => t.Ofertas).HasForeignKey(d => d.LinhaId);

      this.HasRequired(t => t.TCategoria)
          .WithMany(t => t.Ofertas).HasForeignKey(d => d.Categoria);
    }
  }
}
