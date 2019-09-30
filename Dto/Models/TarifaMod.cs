using System;

namespace Dto.Models {
  public class TarifaMod {
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Denominacao { get; set; }
    public bool Gratuidade { get; set; }
    public decimal? Rateio { get; set; }
    public decimal? Tarifa { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
  }
}
