using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PesquisaMap : EntityTypeConfiguration<Pesquisa> {
    public PesquisaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Identificacao)
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Fornecedor)
          .HasMaxLength(64);

      this.Property(t => t.Contrato)
          .HasMaxLength(32);

      this.Property(t => t.Responsavel)
          .HasMaxLength(64);

      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("Pesquisas", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.Identificacao).HasColumnName("Identificacao");
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.Termino).HasColumnName("Termino");
      this.Property(t => t.TerminalId).HasColumnName("TerminalId");
      this.Property(t => t.TroncoId).HasColumnName("TroncoId");
      this.Property(t => t.CorredorId).HasColumnName("CorredorId");
      this.Property(t => t.OperacaoId).HasColumnName("OperacaoId");
      this.Property(t => t.Classificacao).HasColumnName("Classificacao");      
      this.Property(t => t.Interna).HasColumnName("Interna");
      this.Property(t => t.Fornecedor).HasColumnName("Fornecedor");
      this.Property(t => t.Contrato).HasColumnName("Contrato");
      this.Property(t => t.Uteis).HasColumnName("Uteis");
      this.Property(t => t.Sabados).HasColumnName("Sabados");
      this.Property(t => t.Domingos).HasColumnName("Domingos");
      this.Property(t => t.Responsavel).HasColumnName("Responsavel");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.Pesquisas).HasForeignKey(d => d.EmpresaId);

      this.HasOptional(t => t.Terminal)
          .WithMany(t => t.Pesquisas).HasForeignKey(d => d.TerminalId);

      this.HasOptional(t => t.Tronco)
          .WithMany(t => t.Pesquisas).HasForeignKey(d => d.TroncoId);

      this.HasOptional(t => t.Corredor)
          .WithMany(t => t.Pesquisas).HasForeignKey(d => d.CorredorId);

      this.HasOptional(t => t.Operacao)
          .WithMany(t => t.Pesquisas).HasForeignKey(d => d.OperacaoId);

      this.HasOptional(t => t.CLinha)
          .WithMany(t => t.Pesquisas).HasForeignKey(d => d.Classificacao);
    }
  }
}
