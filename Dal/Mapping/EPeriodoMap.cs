using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class EPeriodoMap : EntityTypeConfiguration<EPeriodo> {
    public EPeriodoMap() {
      // Primary Key
      this.HasKey(t => t.Id);        

      // Table, Properties & Column Mappings
      this.ToTable("EPeriodos", "opc");
      this.Property(t => t.Id).HasColumnName("Id").IsRequired();
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.PeriodoId).HasColumnName("PeriodoId").IsRequired();
      this.Property(t => t.Denominacao).HasColumnName("Denominacao")
          .IsRequired().HasMaxLength(32);

      this.Property(t => t.Velocidade).HasColumnName("Velocidade").HasPrecision(9, 3);
      this.Property(t => t.Pico).HasColumnName("Pico").IsRequired();
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.EPeriodos).HasForeignKey(d => d.EmpresaId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.Periodo)
          .WithMany(t => t.EPeriodos).HasForeignKey(d => d.PeriodoId)
          .WillCascadeOnDelete(false);
    }
  }
}
