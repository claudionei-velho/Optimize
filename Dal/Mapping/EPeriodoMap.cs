using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class EPeriodoMap : EntityTypeConfiguration<EPeriodo> {
    public EPeriodoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Denominacao)
          .IsRequired().HasMaxLength(32);

      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("EPeriodos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.PeriodoId).HasColumnName("PeriodoId");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.EPeriodos).HasForeignKey(d => d.EmpresaId);

      this.HasRequired(t => t.Periodo)
          .WithMany(t => t.EPeriodos).HasForeignKey(d => d.PeriodoId);
    }
  }
}
