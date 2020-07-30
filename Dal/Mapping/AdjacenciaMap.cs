using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class AdjacenciaMap : EntityTypeConfiguration<Adjacencia> {
    public AdjacenciaMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Adjacencias", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.PontoId).HasColumnName("PontoId").IsRequired();
      this.Property(t => t.AdjacenteId).HasColumnName("AdjacenciaId").IsRequired();
      this.Property(t => t.Distancia).HasColumnName("Distancia").HasPrecision(18, 3);
      this.Property(t => t.Ciclo).HasColumnName("Ciclo");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Foreign Keys (Relationships)
      this.HasRequired(p => p.Empresa)
          .WithMany(f => f.Adjacencias).HasForeignKey(k => k.EmpresaId)
          .WillCascadeOnDelete(false);

      this.HasRequired(p => p.Ponto)
          .WithMany(f => f.Referentes).HasForeignKey(k => k.PontoId)
          .WillCascadeOnDelete(false);

      this.HasRequired(p => p.Adjacente)
          .WithMany(f => f.Adjacentes).HasForeignKey(k => k.AdjacenteId)
          .WillCascadeOnDelete(false);
    }
  }
}
