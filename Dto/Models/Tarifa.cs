using System;

namespace Dto.Models {
  public class Tarifa {
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public DateTime Referencia { get; set; }
    public decimal Valor { get; set; }
    public string Decreto { get; set; }

    public virtual Empresa Empresa { get; set; }
  }
}
