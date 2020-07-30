using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class RubricaMap : EntityTypeConfiguration<Rubrica> {
    public RubricaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Rubricas");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao")
          .IsRequired().HasMaxLength(128);

      this.Property(t => t.Sintetica).HasColumnName("Sintetica").IsRequired();
      this.Property(t => t.SinteticaId).HasColumnName("SinteticaId");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
    }
  }
}
