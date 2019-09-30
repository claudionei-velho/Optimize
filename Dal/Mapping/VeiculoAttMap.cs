using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class VeiculoAttMap : EntityTypeConfiguration<VeiculoAtt> {
    public VeiculoAttMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Conteudo)
          .IsRequired().HasMaxLength(512);

      // Table & Column Mappings
      this.ToTable("VeiculosAtt", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Classe).HasColumnName("Classe");
      this.Property(t => t.Attributo).HasColumnName("Attributo");
      this.Property(t => t.Conteudo).HasColumnName("Conteudo");

      // Relationships
      this.HasRequired(t => t.CVeiculo)
          .WithMany(t => t.VeiculosAtt).HasForeignKey(d => d.Classe);

      this.HasRequired(t => t.CVeiculoAtt)
          .WithMany(t => t.VeiculosAtt).HasForeignKey(d => d.Attributo);
    }
  }
}
