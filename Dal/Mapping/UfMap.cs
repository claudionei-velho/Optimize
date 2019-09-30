using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class UfMap : EntityTypeConfiguration<Uf> {
    public UfMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Ufs");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Sigla).HasColumnName("Sigla").IsRequired().IsFixedLength().HasMaxLength(2);
      this.Property(t => t.Estado).HasColumnName("Estado").IsRequired().HasMaxLength(32);
      this.Property(t => t.Capital).HasColumnName("Capital").IsRequired().HasMaxLength(32);
      this.Property(t => t.Regiao).HasColumnName("Regiao").IsRequired().HasMaxLength(16);
    }
  }
}
