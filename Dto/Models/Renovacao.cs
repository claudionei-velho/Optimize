using System;

namespace Dto.Models {
  public class Renovacao {
    public int Id { get; set; }
    public int LinhaId { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int? DiaId { get; set; }
    public decimal Indice { get; set; }
    public DateTime? Referencia { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Linha Linha { get; set; }
  }
}
