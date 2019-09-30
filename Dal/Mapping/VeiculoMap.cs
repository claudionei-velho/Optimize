using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class VeiculoMap : EntityTypeConfiguration<Veiculo> {
    public VeiculoMap() {
      // Primary Key
      this.HasKey(t => t.Id);
      
      // Table, Properties & Column Mappings
      this.ToTable("Veiculos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.EmpresaId).HasColumnName("EmpresaId").IsRequired();
      this.Property(t => t.Numero).HasColumnName("Numero").IsRequired().HasMaxLength(16);
      this.Property(t => t.Cor).HasColumnName("Cor").IsRequired().HasMaxLength(32);
      this.Property(t => t.Classe).HasColumnName("Classe").IsRequired();
      this.Property(t => t.Categoria).HasColumnName("Categoria");
      this.Property(t => t.Placa).HasColumnName("Placa").IsRequired().HasMaxLength(16);
      this.Property(t => t.Renavam).HasColumnName("Renavam").IsRequired().HasMaxLength(16);
      this.Property(t => t.Antt).HasColumnName("Antt").HasMaxLength(16);
      this.Property(t => t.Inicio).HasColumnName("Inicio");
      this.Property(t => t.Inativo).HasColumnName("Inativo");
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Relationships
      this.HasRequired(t => t.Empresa)
          .WithMany(t => t.Veiculos).HasForeignKey(d => d.EmpresaId).WillCascadeOnDelete(false);

      this.HasRequired(t => t.CVeiculo)
          .WithMany(t => t.Veiculos).HasForeignKey(d => d.Classe).WillCascadeOnDelete(false);
    }
  }
}
