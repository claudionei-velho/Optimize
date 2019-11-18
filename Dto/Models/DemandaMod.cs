namespace Dto.Models {
  public class DemandaMod {
    public int LinhaId { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int? Passageiros { get; set; }
    public int? Equivalente { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
  }
}
