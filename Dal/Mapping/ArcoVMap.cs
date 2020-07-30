using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class ArcoVMap : EntityTypeConfiguration<ArcoV> {
    public ArcoVMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("ArcosV", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.ArcoId).HasColumnName("ArcoId").IsRequired();
      this.Property(t => t.Item).HasColumnName("Item").IsRequired();
      this.Property(t => t.VetorId).HasColumnName("VetorId").IsRequired();

      // Foreign keys(Relationships)
      this.HasRequired(t => t.Arco)
          .WithMany(f => f.ArcosV).HasForeignKey(k => k.ArcoId)
          .WillCascadeOnDelete(false);

      this.HasRequired(t => t.Vetor)
          .WithMany(f => f.ArcosV).HasForeignKey(k => k.VetorId)
          .WillCascadeOnDelete(false);
    }
  }
}
