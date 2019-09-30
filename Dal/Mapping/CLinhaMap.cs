using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class CLinhaMap : EntityTypeConfiguration<CLinha> {
    public CLinhaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("CLinhas", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.ClassLinhaId).HasColumnName("ClassLinhaId");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.CLinhas).HasForeignKey(d => d.EmpresaId);

      this.HasRequired(t => t.ClassLinha)
          .WithMany(t => t.CLinhas).HasForeignKey(d => d.ClassLinhaId);
    }
  }
}
