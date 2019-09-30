using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class FViagemViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "ViagemId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "ViagemIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int ViagemId { get; set; }

    [Display(Name = "PontoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PontoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int PontoId { get; set; }

    [Display(Name = "Embarques", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Embarques { get; set; }

    [Display(Name = "Desembarques", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Desembarques { get; set; }

    [Display(Name = "Passageiros", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Acumulado { get; set; }

    // Navigation Properties
    public virtual PtLinha PtLinha { get; set; }
    public virtual Viagem Viagem { get; set; }
  }
}
