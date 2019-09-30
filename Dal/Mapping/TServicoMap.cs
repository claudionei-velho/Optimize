using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class TServicoMap : EntityTypeConfiguration<TServico> {
    public TServicoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Denominacao)
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Descricao)
          .HasMaxLength(256);

      // Table & Column Mappings
      this.ToTable("TServicos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.TerminalId).HasColumnName("TerminalId");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao");
      this.Property(t => t.Descricao).HasColumnName("Descricao");
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.Termino).HasColumnName("Termino");

      // Relationships
      this.HasRequired(t => t.Terminal)
          .WithMany(t => t.TServicos).HasForeignKey(d => d.TerminalId);
    }
  }
}
