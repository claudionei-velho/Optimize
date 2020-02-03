using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  public class LoteMap : EntityTypeConfiguration<Lote> {
    public LoteMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Lotes");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.BaciaId).HasColumnName("BaciaId").IsRequired();
      this.Property(t => t.Denominacao).HasColumnName("Denominacao")
          .IsRequired().HasMaxLength(32);

      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Bacia)
          .WithMany(f => f.Lotes).HasForeignKey(k => k.BaciaId)
          .WillCascadeOnDelete(false);
    }
  }
}
