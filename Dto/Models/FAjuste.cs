using System;

namespace Dto.Models {
  public class FAjuste {
    public int LinhaId { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public DateTime? Referencia { get; set; }
    public int? Passageiros { get; set; }
    public decimal? Fator { get; set; }

    public virtual Linha Linha { get; set; }
  }
}
