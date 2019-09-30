using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class TarifaModMap : EntityTypeConfiguration<TarifaMod> {
    public TarifaModMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Denominacao)
          .IsRequired().HasMaxLength(32);

      this.Property(t => t.Tarifa)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("TarifaMod", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao");
      this.Property(t => t.Gratuidade).HasColumnName("Gratuidade");
      this.Property(t => t.Rateio).HasColumnName("Rateio");
      this.Property(t => t.Tarifa).HasColumnName("Tarifa");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.TarifaMods).HasForeignKey(d => d.EmpresaId);
    }
  }
}
