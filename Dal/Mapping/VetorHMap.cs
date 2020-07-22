using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class VetorHMap : EntityTypeConfiguration<VetorH> {
    public VetorHMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("VetoresH", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Item).HasColumnName("Item").IsRequired();
      this.Property(t => t.VetorId).HasColumnName("VetorId").IsRequired();
      this.Property(t => t.HorarioId).HasColumnName("HorarioId").IsRequired();

      // Foreign keys(Relationships)
      this.HasRequired(t => t.Vetor)
          .WithMany(f => f.VetoresH).HasForeignKey(k => k.VetorId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.Horario)
          .WithMany(f => f.VetoresH).HasForeignKey(k => k.HorarioId)
          .WillCascadeOnDelete(false);
    }
  }
}
