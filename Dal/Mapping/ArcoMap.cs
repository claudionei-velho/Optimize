using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ArcoMap : EntityTypeConfiguration<Arco> {
    public ArcoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Arcos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.DiaId).HasColumnName("DiaId").IsRequired();
      this.Property(t => t.Inicio).HasColumnName("Inicio").IsRequired();
      this.Property(t => t.PInicioId).HasColumnName("PInicioId");
      this.Property(t => t.Termino).HasColumnName("Termino").IsRequired();
      this.Property(t => t.PTerminoId).HasColumnName("PTerminoId");
      this.Property(t => t.Duracao).HasColumnName("Duracao")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Foreign keys (Relationships)
      this.HasRequired(t => t.PInicio)
          .WithMany(f => f.ArcosInicio).HasForeignKey(k => k.PInicioId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.PTermino)
          .WithMany(f => f.ArcosTermino).HasForeignKey(k => k.PTerminoId)
          .WillCascadeOnDelete(false);
    }
  }
}
