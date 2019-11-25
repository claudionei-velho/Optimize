using System.ComponentModel.DataAnnotations.Schema;

namespace Dto.Models {
  public class Sinotico {
    public int PesquisaId { get; set; }
    public int LinhaId { get; set; }
    public int DiaId { get; set; }
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
    public virtual Pesquisa Pesquisa { get; set; }
    public virtual Linha Linha { get; set; }
    public virtual ISinotico ISinotico { get; set; }
  }
}
