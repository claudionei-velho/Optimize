using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class FInstalacaoMap : EntityTypeConfiguration<FInstalacao> {
    public FInstalacaoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Denominacao)
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Descricao)
          .HasMaxLength(512);

      // Table & Column Mappings
      this.ToTable("FInstalacoes");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao");
      this.Property(t => t.Descricao).HasColumnName("Descricao");
    }
  }
}
