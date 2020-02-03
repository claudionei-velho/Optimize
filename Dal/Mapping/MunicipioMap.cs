using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class MunicipioMap : EntityTypeConfiguration<Municipio> {
    public MunicipioMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Municipios");

      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.UfId).HasColumnName("UfId").IsRequired();
      this.Property(t => t.Nome).HasColumnName("Nome")
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Estado).HasColumnName("Estado").IsRequired();

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Uf)
          .WithMany(f => f.Municipios).HasForeignKey(k => k.UfId)
          .WillCascadeOnDelete(false);
    }
  }
}
