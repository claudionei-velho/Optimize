using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class ReferenciaViewModel {
    public int Id { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Display(Name = "AtendimentoId", ResourceType = typeof(Properties.Resources))]
    public int? AtendimentoId { get; set; }

    [Display(Name = "Sentido", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "SentidoError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(2)]
    public string Sentido { get; set; }

    [Display(Name = "PInicioId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PInicioIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int PInicioId { get; set; }

    [Display(Name = "PTerminoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PTerminoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int PTerminoId { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual Atendimento Atendimento { get; set; }
    public virtual Ponto PInicio { get; set; }
    public virtual Ponto PTermino { get; set; }
  }
}
