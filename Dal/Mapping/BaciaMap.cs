using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class BaciaMap : EntityTypeConfiguration<Bacia> {
    public BaciaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Bacias");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.MunicipioId).HasColumnName("Municipio").IsRequired();
      this.Property(t => t.Denominacao).HasColumnName("Denominacao")
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Descricao).HasColumnName("Descricao").HasMaxLength(256);
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Municipio)
          .WithMany(f => f.Bacias).HasForeignKey(k => k.MunicipioId)
          .WillCascadeOnDelete(false);
    }
  }
}
