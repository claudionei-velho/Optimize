using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class VetorMap : EntityTypeConfiguration<Vetor> {
    public VetorMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Vetores", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.DiaId).HasColumnName("DiaId").IsRequired();
      this.Property(t => t.Inicio).HasColumnName("Inicio").IsRequired();
      this.Property(t => t.PInicioId).HasColumnName("PInicioId");
      this.Property(t => t.Termino).HasColumnName("Termino").IsRequired();
      this.Property(t => t.PTerminoId).HasColumnName("PTerminoId");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Foreign keys (Relationships)
      this.HasRequired(t => t.Empresa)
          .WithMany(f => f.Vetores).HasForeignKey(k => k.EmpresaId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.PInicio)
          .WithMany(f => f.VetoresInicio).HasForeignKey(k => k.PInicioId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.PTermino)
          .WithMany(f => f.VetoresTermino).HasForeignKey(k => k.PTerminoId)
          .WillCascadeOnDelete(false);
    }
  }
}
