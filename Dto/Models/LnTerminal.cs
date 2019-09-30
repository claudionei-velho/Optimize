using System;

namespace Dto.Models {
  public class LnTerminal {
    public int Id { get; set; }
    public int TerminalId { get; set; }
    public int LinhaId { get; set; }
    public bool Uteis { get; set; }
    public int? UteisFluxo { get; set; }
    public bool Sabados { get; set; }
    public int? SabadosFluxo { get; set; }
    public bool Domingos { get; set; }
    public int? DomingosFluxo { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Terminal Terminal { get; set; }
    public virtual Linha Linha { get; set; }
  }
}
