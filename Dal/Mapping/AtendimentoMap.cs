using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class AtendimentoMap : EntityTypeConfiguration<Atendimento> {
    public AtendimentoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("Atendimentos", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.LinhaId).HasColumnName("LinhaId").IsRequired();
      this.Property(t => t.Prefixo).HasColumnName("Prefixo").IsRequired().HasMaxLength(16);
      this.Property(t => t.Denominacao).HasColumnName("Denominacao").IsRequired().HasMaxLength(128);
      this.Property(t => t.Uteis).HasColumnName("Uteis").IsRequired();
      this.Property(t => t.Sabados).HasColumnName("Sabados").IsRequired();
      this.Property(t => t.Domingos).HasColumnName("Domingos").IsRequired();
      this.Property(t => t.Escolar).HasColumnName("Escolar").IsRequired();
      this.Property(t => t.ExtensaoAB).HasColumnName("ExtensaoAB").HasPrecision(18, 3);
      this.Property(t => t.ExtensaoBA).HasColumnName("ExtensaoBA").HasPrecision(18, 3);
      this.Property(t => t.Cadastro).HasColumnName("Cadastro")
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

      // Foreign Keys (Relationships)
      this.HasRequired(t => t.Linha)
          .WithMany(t => t.Atendimentos).HasForeignKey(d => d.LinhaId).WillCascadeOnDelete(false);
    }
  }
}
