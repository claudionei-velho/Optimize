using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class CorredorMap : EntityTypeConfiguration<Corredor> {
    public CorredorMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Corredores", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.Prefixo).HasColumnName("Prefixo")
          .IsRequired().HasMaxLength(16);

      this.Property(t => t.Denominacao).HasColumnName("Denominacao")
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Caracteristicas).HasColumnName("Caracteristicas").HasMaxLength(512);
      this.Property(t => t.Municipio).HasColumnName("Municipio")
          .IsRequired().HasMaxLength(32);

      this.Property(t => t.UfId).HasColumnName("UfId")
          .IsRequired().IsFixedLength().HasMaxLength(2);

      this.Property(t => t.Extensao).HasColumnName("Extensao");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.Corredores).HasForeignKey(d => d.EmpresaId);
    }
  }
}
