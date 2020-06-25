using System;
using System.ComponentModel.DataAnnotations;

using Bll.Lists;
using Dto.Models;

namespace UI.Models {
  public class LnTerminalViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "TerminalId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "TerminalIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int TerminalId { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Display(Name = "Uteis", ResourceType = typeof(Properties.Resources))]
    public bool Uteis { get; set; }

    public int? UteisFluxo { get; set; }
    public string UteisFluxoCap {
      get {
        return Fluxo.Data[UteisFluxo ?? 0];
      }
    }

    [Display(Name = "Sabados", ResourceType = typeof(Properties.Resources))]
    public bool Sabados { get; set; }

    public int? SabadosFluxo { get; set; }
    public string SabadosFluxoCap {
      get {
        return Fluxo.Data[SabadosFluxo ?? 0];
      }
    }

    [Display(Name = "Domingos", ResourceType = typeof(Properties.Resources))]
    public bool Domingos { get; set; }

    public int? DomingosFluxo { get; set; }
    public string DomingosFluxoCap {
      get {
        return Fluxo.Data[DomingosFluxo ?? 0];
      }
    }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Terminal Terminal { get; set; }
    public virtual Linha Linha { get; set; }
  }
}
