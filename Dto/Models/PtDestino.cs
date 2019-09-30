using System.Collections.Generic;

namespace Dto.Models {
  public class PtDestino {
    public PtDestino() {
      this.PtLinhas = new HashSet<PtLinha>();
    }

    public int Id { get; set; }
    public int LinhaId { get; set; }
    public string Sentido { get; set; }
    public int PontoId { get; set; }
    public string Prefixo { get; set; }
    public string Identificacao { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual Ponto Ponto { get; set; }

    public virtual ICollection<PtLinha> PtLinhas { get; set; }
  }
}
