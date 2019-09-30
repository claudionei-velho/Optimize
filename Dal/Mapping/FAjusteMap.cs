using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class FAjusteMap : EntityTypeConfiguration<FAjuste> {
    public FAjusteMap() {
      // Primary Key
      this.HasKey(t => new { t.LinhaId, t.Ano, t.Mes });

      // Properties
      this.Property(t => t.LinhaId)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

      this.Property(t => t.Ano)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

      this.Property(t => t.Mes)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

      // Table & Column Mappings
      this.ToTable("FAjuste", "opc");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.Ano).HasColumnName("Ano");
      this.Property(t => t.Mes).HasColumnName("Mes");
      this.Property(t => t.Referencia).HasColumnName("Referencia");
      this.Property(t => t.Passageiros).HasColumnName("Passageiros");
      this.Property(t => t.Fator).HasColumnName("Fator");

      // Relationships
      this.HasRequired(t => t.Linha)
          .WithMany(t => t.FAjustes).HasForeignKey(d => d.LinhaId);
    }
  }
}
