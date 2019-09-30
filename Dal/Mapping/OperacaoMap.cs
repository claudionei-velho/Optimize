using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class OperacaoMap : EntityTypeConfiguration<Operacao> {
    public OperacaoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("Operacoes", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.OperLinhaId).HasColumnName("OperLinhaId");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.Operacoes).HasForeignKey(d => d.EmpresaId);

      this.HasRequired(t => t.OperLinha)
          .WithMany(t => t.Operacoes).HasForeignKey(d => d.OperLinhaId);
    }
  }
}
