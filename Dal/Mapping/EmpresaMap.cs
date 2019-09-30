using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class EmpresaMap : EntityTypeConfiguration<Empresa> {
    public EmpresaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Razao)
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Fantasia)
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Cnpj)
          .IsRequired().HasMaxLength(32);

      this.Property(t => t.IEstadual)
          .HasMaxLength(16);

      this.Property(t => t.IMunicipal)
          .HasMaxLength(16);

      this.Property(t => t.Endereco)
          .IsRequired().HasMaxLength(128);

      this.Property(t => t.EnderecoNo)
          .HasMaxLength(8);

      this.Property(t => t.Complemento)
          .HasMaxLength(64);

      this.Property(t => t.Bairro)
          .HasMaxLength(32);

      this.Property(t => t.Municipio)
          .IsRequired().HasMaxLength(32);

      this.Property(t => t.UfId)
          .IsRequired().IsFixedLength().HasMaxLength(2);

      this.Property(t => t.PaisId)
          .IsRequired().HasMaxLength(8);

      this.Property(t => t.Telefone)
          .HasMaxLength(32);

      this.Property(t => t.Email)
          .HasMaxLength(256);

      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("Empresas");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Razao).HasColumnName("Razao");
      this.Property(t => t.Fantasia).HasColumnName("Fantasia");
      this.Property(t => t.Cnpj).HasColumnName("Cnpj");
      this.Property(t => t.IEstadual).HasColumnName("IEstadual");
      this.Property(t => t.IMunicipal).HasColumnName("IMunicipal");
      this.Property(t => t.Endereco).HasColumnName("Endereco");
      this.Property(t => t.EnderecoNo).HasColumnName("EnderecoNo");
      this.Property(t => t.Complemento).HasColumnName("Complemento");
      this.Property(t => t.Cep).HasColumnName("Cep");
      this.Property(t => t.Bairro).HasColumnName("Bairro");
      this.Property(t => t.Municipio).HasColumnName("Municipio");
      this.Property(t => t.UfId).HasColumnName("UfId");
      this.Property(t => t.PaisId).HasColumnName("PaisId");
      this.Property(t => t.Telefone).HasColumnName("Telefone");
      this.Property(t => t.Email).HasColumnName("Email");
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.Termino).HasColumnName("Termino");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Pais)
          .WithMany(t => t.Empresas).HasForeignKey(d => d.PaisId);
    }
  }
}
