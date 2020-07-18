using System;

namespace Dto.Models {
  public class Referencia {
    public int Id { get; set; }
    public int LinhaId { get; set; }
    public int? AtendimentoId { get; set; }
    public string Sentido { get; set; }
    public int PInicioId { get; set; }
    public int PTerminoId { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual Atendimento Atendimento { get; set; }
    public virtual Ponto PInicio { get; set; }
    public virtual Ponto PTermino { get; set; }
  }
}
