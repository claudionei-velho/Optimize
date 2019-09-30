using System;

namespace Dto.Models {
  public class TServico {
    public int Id { get; set; }
    public int TerminalId { get; set; }
    public string Denominacao { get; set; }
    public string Descricao { get; set; }
    public TimeSpan? Inicio { get; set; }
    public TimeSpan? Termino { get; set; }

    public virtual Terminal Terminal { get; set; }
  }
}
