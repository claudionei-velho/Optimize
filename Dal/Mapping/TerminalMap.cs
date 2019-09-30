using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class TerminalMap : EntityTypeConfiguration<Terminal> {
    public TerminalMap() {
      // Primary Key
      this.HasKey(t => t.Id);
                
      // Table, Properties & Column Mappings
      this.ToTable("Terminais", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.Prefixo).HasColumnName("Prefixo").HasMaxLength(16);
      this.Property(t => t.Denominacao).HasColumnName("Denominacao").IsRequired().HasMaxLength(64);
      this.Property(t => t.Cnpj).HasColumnName("Cnpj").HasMaxLength(32);
      this.Property(t => t.IEstadual).HasColumnName("IEstadual").HasMaxLength(16);
      this.Property(t => t.IMunicipal).HasColumnName("IMunicipal").HasMaxLength(16);
      this.Property(t => t.Endereco).HasColumnName("Endereco").IsRequired().HasMaxLength(128);
      this.Property(t => t.EnderecoNo).HasColumnName("EnderecoNo").HasMaxLength(8);
      this.Property(t => t.Complemento).HasColumnName("Complemento").HasMaxLength(64);
      this.Property(t => t.Cep).HasColumnName("Cep");
      this.Property(t => t.Bairro).HasColumnName("Bairro").HasMaxLength(32);
      this.Property(t => t.Municipio).HasColumnName("Municipio").IsRequired().HasMaxLength(32);
      this.Property(t => t.UfId).HasColumnName("UfId").IsRequired().IsFixedLength().HasMaxLength(2);
      this.Property(t => t.Telefone).HasColumnName("Telefone").HasMaxLength(32);
      this.Property(t => t.Email).HasColumnName("Email").HasMaxLength(256);
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.Termino).HasColumnName("Termino");
      this.Property(t => t.Latitude).HasColumnName("Latitude").HasPrecision(24, 12);
      this.Property(t => t.Longitude).HasColumnName("Longitude").HasPrecision(24, 12);
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.Terminais).HasForeignKey(d => d.EmpresaId).WillCascadeOnDelete(false);
    }
  }
}
