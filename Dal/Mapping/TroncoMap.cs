using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class TroncoMap : EntityTypeConfiguration<Tronco> {
    public TroncoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Properties
      this.Property(t => t.Prefixo)
          .IsRequired().HasMaxLength(16);

      this.Property(t => t.Denominacao)
          .IsRequired().HasMaxLength(64);
          
      this.Property(t => t.Cadastro)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Table & Column Mappings
      this.ToTable("Troncos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId");
      this.Property(t => t.Prefixo).HasColumnName("Prefixo");
      this.Property(t => t.Denominacao).HasColumnName("Denominacao");
      this.Property(t => t.ExtensaoAB).HasColumnName("ExtensaoAB");
      this.Property(t => t.ExtensaoBA).HasColumnName("ExtensaoBA");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro");

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.Troncos).HasForeignKey(d => d.EmpresaId);
    }
  }
}
