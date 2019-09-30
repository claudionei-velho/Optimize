using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class EDominioMap : EntityTypeConfiguration<EDominio> {
    public EDominioMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("EDominios", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.DominioId).HasColumnName("DominioId");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Dominio)
          .WithMany(t => t.EDominios).HasForeignKey(d => d.DominioId);

      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.EDominios).HasForeignKey(d => d.EmpresaId);
    }
  }
}
