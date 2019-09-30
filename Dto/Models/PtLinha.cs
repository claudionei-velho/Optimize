using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class PtLinha {
    public PtLinha() {
      this.FViagens = new HashSet<FViagem>();
      this.Viagens = new HashSet<Viagem>();
    }

    public int Id { get; set; }
    public int LinhaId { get; set; }
    public string Sentido { get; set; }
    public int PontoId { get; set; }
    public int? OrigemId { get; set; }
    public int? DestinoId { get; set; }
    public int? Fluxo { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual Ponto Ponto { get; set; }
    public virtual PtOrigem Origem { get; set; }
    public virtual PtDestino Destino { get; set; }

    public virtual ICollection<FViagem> FViagens { get; set; }
    public virtual ICollection<Viagem> Viagens { get; set; }
  }
}
