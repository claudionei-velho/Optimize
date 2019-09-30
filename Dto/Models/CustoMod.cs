using System;

namespace Dto.Models {
  public class CustoMod {
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public DateTime Referencia { get; set; }
    public decimal Fixo { get; set; }
    public decimal Variavel { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
  }
}
