using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class CorredorMap : EntityTypeConfiguration<Corredor> {
    public CorredorMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Prefixo)
          .IsRequired().HasMaxLength(16);

      this.Property(t => t.Denominacao)
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Caracteristicas)
          .HasMaxLength(512);

      this.Property(t => t.Municipio)
          .IsRequired().HasMaxLength(32);

      this.Property(t => t.UfId)
          .IsRequired().IsFixedLength().HasMaxLength(2);

      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("Corredores", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.Prefixo).HasColumnName("Prefixo");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao");
      this.Property(t => t.Caracteristicas).HasColumnName("Caracteristicas");
      this.Property(t => t.Municipio).HasColumnName("Municipio");
      this.Property(t => t.UfId).HasColumnName("UfId");
      this.Property(t => t.Extensao).HasColumnName("Extensao");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.Corredores).HasForeignKey(d => d.EmpresaId);
    }
  }
}
