using System;
using System.ComponentModel.DataAnnotations;

using Dto.Lists;
using Dto.Models;

namespace UI.Models {
  public class HorarioViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Display(Name = "AtendimentoId", ResourceType = typeof(Properties.Resources))]
    public int? AtendimentoId { get; set; }

    [Display(Name = "DiaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DiaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int DiaId { get; set; }

    public string DiaIdName {
      get {
        return Workday.Items[DiaId];
      }
    }

    [Display(Name = "Item", ResourceType = typeof(Properties.Resources))]
    public int? Item { get; set; }

    [Display(Name = "Sentido", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "SentidoError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(2)]
    public string Sentido { get; set; }

    [Display(Name = "HoraInicio", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "HoraInicioError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan Inicio { get; set; }

    [Display(Name = "PeriodoId", ResourceType = typeof(Properties.Resources))]
    public int? PeriodoId { get; set; }

    [Display(Name = "Extensao", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? Extensao { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Atendimento Atendimento { get; set; }
    public virtual Linha Linha { get; set; }
    public virtual PrLinha PrLinha { get; set; }
  }
}
