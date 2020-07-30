using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PontoMap : EntityTypeConfiguration<Ponto> {
    public PontoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Pontos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.Prefixo).HasColumnName("Prefixo").HasMaxLength(16);
      this.Property(t => t.Identificacao).HasColumnName("Identificacao").IsRequired().HasMaxLength(32);
      this.Property(t => t.Endereco).HasColumnName("Endereco").IsRequired().HasMaxLength(128);
      this.Property(t => t.EnderecoRef).HasColumnName("EnderecoRef").HasMaxLength(64);
      this.Property(t => t.Cep).HasColumnName("Cep");
      this.Property(t => t.Bairro).HasColumnName("Bairro").HasMaxLength(32);
      this.Property(t => t.Municipio).HasColumnName("Municipio").IsRequired().HasMaxLength(32);
      this.Property(t => t.UfId).HasColumnName("UfId").IsRequired().IsFixedLength().HasMaxLength(2);
      this.Property(t => t.Intercambio).HasColumnName("Intercambio");
      this.Property(t => t.Garagem).HasColumnName("Garagem");
      this.Property(t => t.Latitude).HasColumnName("Latitude").HasPrecision(24, 12);
      this.Property(t => t.Longitude).HasColumnName("Longitude").HasPrecision(24, 12);
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.Pontos).HasForeignKey(d => d.EmpresaId).WillCascadeOnDelete(false);
    }
  }
}
