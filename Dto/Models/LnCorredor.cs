using System;

namespace Dto.Models {
  public class LnCorredor {
    public int Id { get; set; }
    public int CorredorId { get; set; }
    public int LinhaId { get; set; }
    public string Sentido { get; set; }
    public decimal? Extensao { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Corredor Corredor { get; set; }
    public virtual Linha Linha { get; set; }
  }
}
