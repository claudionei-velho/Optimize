using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class DominioMap : EntityTypeConfiguration<Dominio> {
    public DominioMap() {
      // Primary Key
      this.HasKey(t => t.Id);        

      // Table, Properties & Column Mappings
      this.ToTable("Dominios");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao").IsRequired().HasMaxLength(32);
      this.Property(t => t.Descricao).HasColumnName("Descricao").HasMaxLength(256);
    }
  }
}
