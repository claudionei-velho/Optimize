using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class LnCorredorMap : EntityTypeConfiguration<LnCorredor> {
    public LnCorredorMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Sentido)
          .IsFixedLength().HasMaxLength(2);

      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("LnCorredores", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.CorredorId).HasColumnName("CorredorId");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId");
      this.Property(t => t.Sentido).HasColumnName("Sentido");
      this.Property(t => t.Extensao).HasColumnName("Extensao");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Corredor)
          .WithMany(t => t.LnCorredores).HasForeignKey(d => d.CorredorId);

      this.HasRequired(t => t.Linha)
          .WithMany(t => t.LnCorredores).HasForeignKey(d => d.LinhaId);
    }
  }
}
