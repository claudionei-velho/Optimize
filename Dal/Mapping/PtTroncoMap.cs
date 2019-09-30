using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PtTroncoMap : EntityTypeConfiguration<PtTronco> {
    public PtTroncoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Sentido)
          .IsRequired().IsFixedLength().HasMaxLength(2);

      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("PtTroncos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.TroncoId).HasColumnName("TroncoId");
      this.Property(t => t.Sentido).HasColumnName("Sentido");
      this.Property(t => t.PontoId).HasColumnName("PontoId");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Tronco)
          .WithMany(t => t.PtTroncos).HasForeignKey(d => d.TroncoId);

      this.HasRequired(t => t.Ponto)
          .WithMany(t => t.PtTroncos).HasForeignKey(d => d.PontoId);
    }
  }
}
