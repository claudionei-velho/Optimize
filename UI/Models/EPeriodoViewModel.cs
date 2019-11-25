using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class EPeriodoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "EPeriodoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EPeriodoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int PeriodoId { get; set; }

    [Display(Name = "Denominacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DenominacaoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(32)]
    public string Denominacao { get; set; }

    [Display(Name = "Pico", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PicoError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(32)]
    public bool Pico { get; set; }

    [Display(Name = "Velocidade", ResourceType = typeof(Properties.Resources))]
    public decimal? Velocidade { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual Periodo Periodo { get; set; }
  }
}
