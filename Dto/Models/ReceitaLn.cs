namespace Dto.Models {
  public class ReceitaLn {
    public int EmpresaId { get; set; }
    public int LinhaId { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int Passageiros { get; set; }
    public decimal Receita { get; set; }
    public decimal? Aliquota { get; set; }
    
    public decimal Impostos {
      get {
        return Receita * (Aliquota ?? 0);
      }
    }

    public decimal Liquida {
      get {
        return Receita * (1 - (Aliquota ?? 0));
      }
    }
  }
}
