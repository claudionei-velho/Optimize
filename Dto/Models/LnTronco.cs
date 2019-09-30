using System;

namespace Dto.Models {
  public class LnTronco {
    public int Id { get; set; }
    public int TroncoId { get; set; }
    public int LinhaId { get; set; }
    public DateTime Cadastro { get; set; }

    // Navigation Properties
    public virtual Tronco Tronco { get; set; }
    public virtual Linha Linha { get; set; }
  }
}
