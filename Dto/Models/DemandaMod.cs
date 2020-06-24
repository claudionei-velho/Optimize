namespace Dto.Models {
  public class DemandaMod {
    public int EmpresaId { get; set; }
    public int LinhaId { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int? Passageiros { get; set; }
    public int? Equivalente { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual Linha Linha { get; set; }
  }
}
