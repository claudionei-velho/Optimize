using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Viagem {
    public Viagem() {
      this.FViagens = new HashSet<FViagem>();
    }

    public int Id { get; set; }
    public int LinhaId { get; set; }
    public int? Item { get; set; }
    public DateTime Data { get; set; }
    public string Sentido { get; set; }
    public int? HorarioId { get; set; }
    public int? PontoId { get; set; }
    public int? VeiculoId { get; set; }
    public TimeSpan? Chegada { get; set; }
    public TimeSpan Inicio { get; set; }
    public TimeSpan? Termino { get; set; }
    public int? Passageiros { get; set; }
    public int? Inicial { get; set; }
    public int? Final { get; set; }
    public string Responsavel { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Horario Horario { get; set; }
    public virtual LnPesquisa LnPesquisa { get; set; }
    public virtual PtLinha PtLinha { get; set; }
    public virtual Veiculo Veiculo { get; set; }

    public virtual ICollection<FViagem> FViagens { get; set; }
  }
}
