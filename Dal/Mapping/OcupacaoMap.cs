using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class OcupacaoMap : EntityTypeConfiguration<Ocupacao> {
    public OcupacaoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Denominacao)
          .IsRequired().HasMaxLength(32);

      this.Property(t => t.Nivel)
          .IsRequired().HasMaxLength(4);

      // Table & Column Mappings
      this.ToTable("Ocupacoes", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao");
      this.Property(t => t.Nivel).HasColumnName("Nivel");
      this.Property(t => t.Densidade).HasColumnName("Densidade");
    }
  }
}
