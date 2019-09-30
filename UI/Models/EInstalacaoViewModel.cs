using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class EInstalacaoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "InstalacaoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "InstalacaoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int InstalacaoId { get; set; }

    [Display(Name = "PropositoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PropositoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int PropositoId { get; set; }

    [Display(Name = "AreaCoberta", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? AreaCoberta { get; set; }

    [Display(Name = "AreaTotal", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? AreaTotal { get; set; }

    [Display(Name = "QtdEmpregados", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
    public int? QtdEmpregados { get; set; }

    [Display(Name = "Inicio", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan? Inicio { get; set; }

    [Display(Name = "Termino", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan? Termino { get; set; }

    [Display(Name = "Horario", ResourceType = typeof(Properties.Resources))]
    public string Horario {
      get {
        return $"{$@"{this.Inicio:hh\:mm}"} - {$@"{this.Termino:hh\:mm}"}";
      }
    }

    [Display(Name = "Efluentes", ResourceType = typeof(Properties.Resources))]
    public bool Efluentes { get; set; }

    [ScaffoldColumn(false)]
    public DateTime Cadastro { get; set; }

    // Navigation Properties
    public virtual Instalacao Instalacao { get; set; }
    public virtual FInstalacao FInstalacao { get; set; }
  }
}
