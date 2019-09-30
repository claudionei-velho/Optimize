using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class TServicoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "TerminalId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "TerminalIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int TerminalId { get; set; }

    [Display(Name = "Denominacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DenominacaoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(64)]
    public string Denominacao { get; set; }

    [Display(Name = "Descricao", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.MultilineText), StringLength(256)]
    public string Descricao { get; set; }

    [Display(Name = "HoraInicio", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan? Inicio { get; set; }

    [Display(Name = "HoraTermino", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan? Termino { get; set; }

    [Display(Name = "Horario", ResourceType = typeof(Properties.Resources))]
    public string Horario {
      get {
        return $"{$@"{this.Inicio:hh\:mm}"} - {$@"{this.Termino:hh\:mm}"}";
      }
    }

    // Navigation Properties
    public virtual Terminal Terminal { get; set; }
  }
}
