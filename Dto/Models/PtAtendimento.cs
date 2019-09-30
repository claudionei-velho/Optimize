using System;

namespace Dto.Models {
  public class PtAtendimento {
    public int Id { get; set; }
    public int AtendimentoId { get; set; }
    public string Sentido { get; set; }
    public int PontoId { get; set; }
    public DateTime? Cadastro { get; set; }

    public virtual Atendimento Atendimento { get; set; }
    public virtual Ponto Ponto { get; set; }
  }
}
