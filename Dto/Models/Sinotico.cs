using System.ComponentModel.DataAnnotations.Schema;

namespace Dto.Models {
  public class Sinotico {
    public int LinhaId { get; set; }
    public int DiaId { get; set; }
    public string Sentido { get; set; }
    public int SinoticoId { get; set; }

    [NotMapped]
    public string IndiceAtual { get; set; }

    [NotMapped]
    public string DimensionaE { get; set; }

    [NotMapped]
    public string DimensionaP { get; set; }

    [NotMapped]
    public decimal? EvolucaoE { get; set; }

    [NotMapped]
    public decimal? EvolucaoP { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual ISinotico ISinotico { get; set; }
  }
}
