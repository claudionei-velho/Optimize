namespace Dto.Models {
  public class ECVeiculo {
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int ClasseId { get; set; }
    public int? Minimo { get; set; }
    public int? Maximo { get; set; }
    public byte Passageirom2 { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual CVeiculo CVeiculo { get; set; }
  }
}
