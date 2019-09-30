using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ItTroncoMap : EntityTypeConfiguration<ItTronco> {
    public ItTroncoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("ItTroncos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.TroncoId).HasColumnName("TroncoId");
      this.Property(t => t.Sentido).HasColumnName("Sentido").IsRequired().IsFixedLength().HasMaxLength(2);
      this.Property(t => t.Percurso).HasColumnName("Percurso").IsRequired().HasMaxLength(256);
      this.Property(t => t.Extensao).HasColumnName("Extensao").HasPrecision(18, 3);
      this.Property(t => t.PavimentoId).HasColumnName("PavimentoId");
      this.Property(t => t.Abrangencia).HasColumnName("Abrangencia");
      this.Property(t => t.CondicaoId).HasColumnName("CondicaoId");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Relationships
      this.HasRequired(t => t.Tronco)
          .WithMany(t => t.ItTroncos).HasForeignKey(d => d.TroncoId).WillCascadeOnDelete(false);

      this.HasOptional(t => t.Via)
          .WithMany(t => t.ItTroncos).HasForeignKey(d => d.PavimentoId).WillCascadeOnDelete(false);
    }
  }
}
