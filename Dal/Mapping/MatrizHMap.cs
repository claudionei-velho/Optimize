using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class MatrizHMap : EntityTypeConfiguration<MatrizH> {
    public MatrizHMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("MatrizHorarios", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.AtendimentoId).HasColumnName("AtendimentoId");
      this.Property(t => t.DiaId).HasColumnName("DiaId").IsRequired();
      this.Property(t => t.Item).HasColumnName("Item");
      this.Property(t => t.Inicio).HasColumnName("Inicio").IsRequired();
      this.Property(t => t.Termino).HasColumnName("Termino").IsRequired();
      this.Property(t => t.Duracao).HasColumnName("Duracao");
      this.Property(t => t.Sentido).HasColumnName("Sentido").IsRequired()
          .IsFixedLength().HasMaxLength(2);

      this.Property(t => t.PInicioId).HasColumnName("PInicioId");
      this.Property(t => t.PTerminoId).HasColumnName("PTerminoId");

      // Foreign keys (Relationships)
      this.HasOptional(t => t.PInicio)
          .WithMany(f => f.PInicioMatriz).HasForeignKey(k => k.PInicioId);

      this.HasOptional(t => t.PTermino)
          .WithMany(f => f.PTerminoMatriz).HasForeignKey(k => k.PTerminoId);
    }
  }
}
