using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class LinhaMap : EntityTypeConfiguration<Linha> {
    public LinhaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Linhas", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.Prefixo).HasColumnName("Prefixo")
          .IsRequired().HasMaxLength(16);

      this.Property(t => t.Denominacao).HasColumnName("Denominacao")
          .IsRequired().HasMaxLength(128);

      this.Property(t => t.Viagem).HasColumnName("Viagem").HasMaxLength(256);
      this.Property(t => t.Uteis).HasColumnName("Uteis");
      this.Property(t => t.Sabados).HasColumnName("Sabados");
      this.Property(t => t.Domingos).HasColumnName("Domingos");
      this.Property(t => t.DominioId).HasColumnName("DominioId");
      this.Property(t => t.OperacaoId).HasColumnName("OperacaoId");
      this.Property(t => t.Classificacao).HasColumnName("Classificacao");
      this.Property(t => t.Captacao).HasColumnName("Captacao");
      this.Property(t => t.Transporte).HasColumnName("Transporte");
      this.Property(t => t.Distribuicao).HasColumnName("Distribuicao");
      this.Property(t => t.ExtensaoAB).HasColumnName("ExtensaoAB").HasPrecision(18, 3);
      this.Property(t => t.ExtensaoBA).HasColumnName("ExtensaoBA").HasPrecision(18, 3);
      this.Property(t => t.LoteId).HasColumnName("LoteId");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.Linhas).HasForeignKey(d => d.EmpresaId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.EDominio)
          .WithMany(t => t.Linhas).HasForeignKey(d => d.DominioId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.Operacao)
          .WithMany(t => t.Linhas).HasForeignKey(d => d.OperacaoId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.CLinha)
          .WithMany(t => t.Linhas).HasForeignKey(d => d.Classificacao)
          .WillCascadeOnDelete(false);

      this.HasOptional(t => t.Lote)
          .WithMany(t => t.Linhas).HasForeignKey(d => d.LoteId)
          .WillCascadeOnDelete(false);
    }
  }
}
