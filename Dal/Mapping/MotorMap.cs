using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class MotorMap : EntityTypeConfiguration<Motor> {
    public MotorMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Denominacao)
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Classificacao)
          .IsRequired().HasMaxLength(64);

      this.Property(t => t.Descricao)
          .HasMaxLength(256);

      // Table & Column Mappings
      this.ToTable("Motores", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao");
      this.Property(t => t.Classificacao).HasColumnName("Classificacao");
      this.Property(t => t.Descricao).HasColumnName("Descricao");
    }
  }
}
