using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class EUsuarioMap : EntityTypeConfiguration<EUsuario> {
    public EUsuarioMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
     this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("EUsuarios");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
      this.Property(t => t.Ativo).HasColumnName("Ativo");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.EUsuarios).HasForeignKey(d => d.EmpresaId);

      this.HasRequired(t => t.Usuario)
          .WithMany(t => t.EUsuarios).HasForeignKey(d => d.UsuarioId);
    }
  }
}
