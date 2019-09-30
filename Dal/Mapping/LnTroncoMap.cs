using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class LnTroncoMap : EntityTypeConfiguration<LnTronco> {
    public LnTroncoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("LnTroncos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.TroncoId).HasColumnName("TroncoId");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Tronco)
          .WithMany(t => t.LnTroncos).HasForeignKey(d => d.TroncoId);

      this.HasRequired(t => t.Linha)
          .WithMany(t => t.LnTroncos).HasForeignKey(d => d.LinhaId);
    }
  }
}
