using System;

namespace Dto.Models {
  public class ItTronco {
    public int Id { get; set; }
    public int TroncoId { get; set; }
    public string Sentido { get; set; }
    public string Percurso { get; set; }
    public decimal? Extensao { get; set; }
    public int? PavimentoId { get; set; }
    public decimal? Abrangencia { get; set; }
    public int? CondicaoId { get; set; }
    public DateTime Cadastro { get; set; }

    // Navigation Properties
    public virtual Tronco Tronco { get; set; }
  }
}
