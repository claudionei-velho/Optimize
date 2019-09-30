using System.Data.Entity.ModelConfiguration;

using Dto.Models;

namespace Dal.Mapping {
  internal class DiaTrabalhoMap : EntityTypeConfiguration<DiaTrabalho> {
    public DiaTrabalhoMap() {
      // Primary Key
      this.HasKey(t => t.Id);

      // Table, Properties & Column Mappings
      this.ToTable("DiasTrabalho", "opc");
      this.Property(t => t.Id).HasColumnName("Id");
      this.Property(t => t.Dias).HasColumnName("Dias");
    }
  }
}
