using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class PremissaMap : EntityTypeConfiguration<Premissa> {
    public PremissaMap() {
      // Primary Key
      this.HasKey(t => t.EmpresaId);

      // Table, Properties & Column Mappings
      this.ToTable("Premissas");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.JornadaDia).HasColumnName("JornadaDia")
          .IsRequired().HasPrecision(6, 3);

      this.Property(t => t.JornadaMax).HasColumnName("JornadaMax")
          .IsRequired().HasPrecision(6, 3);

      this.Property(t => t.JornadaSemana).HasColumnName("JornadaSemana")
          .IsRequired().HasPrecision(6, 3);

      this.Property(t => t.InterJornada).HasColumnName("InterJornada")
          .IsRequired().HasPrecision(6, 3);

      this.Property(t => t.IntraJornadaMin).HasColumnName("IntraJornadaMin")
          .IsRequired().HasPrecision(6, 3);

      this.Property(t => t.IntraJornadaMax).HasColumnName("IntraJornadaMax")
          .IsRequired().HasPrecision(6, 3);

      this.Property(t => t.DeslocaInicial).HasColumnName("DeslocaInicial");
      this.Property(t => t.Deslocamento).HasColumnName("Deslocamento");
      this.Property(t => t.VetorPadrao).HasColumnName("VetorPadrao")
          .IsRequired().HasPrecision(6, 3);

      this.Property(t => t.NoturnoInicio).HasColumnName("NoturnoInicio").IsRequired();
      this.Property(t => t.NoturnoTermino).HasColumnName("NoturnoTermino").IsRequired();

      // Foreign Keys (Relationships)
      this.HasRequired(p => p.Empresa)
          .WithOptional(f => f.Premissa).WillCascadeOnDelete(false);
    }
  }
}
