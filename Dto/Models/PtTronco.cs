using System;

namespace Dto.Models {
  public class PtTronco {
    public int Id { get; set; }
    public int TroncoId { get; set; }
    public string Sentido { get; set; }
    public int PontoId { get; set; }
    public DateTime Cadastro { get; set; }

    // Navigation Properties
    public virtual Tronco Tronco { get; set; }
    public virtual Ponto Ponto { get; set; }
  }
}
