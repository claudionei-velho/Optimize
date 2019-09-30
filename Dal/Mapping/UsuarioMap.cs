using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class UsuarioMap : EntityTypeConfiguration<Usuario> {
    public UsuarioMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Nome)
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Login)
          .IsRequired().HasMaxLength(256);

      this.Property(t => t.Senha)
          .IsRequired().HasMaxLength(256);

      this.Property(t => t.Perfil)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("Usuarios");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Nome).HasColumnName("Nome");
      this.Property(t => t.Login).HasColumnName("Login");
      this.Property(t => t.Senha).HasColumnName("Senha");
      this.Property(t => t.Perfil).HasColumnName("Perfil");
      this.Property(t => t.Ativo).HasColumnName("Ativo");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");
    }
  }
}
