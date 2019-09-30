using System.ComponentModel.DataAnnotations;

namespace UI.Models {
  public class OcupacaoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "Denominacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DenominacaoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(32)]
    public string Denominacao { get; set; }

    [Display(Name = "Nivel", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "NivelError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(4)]
    public string Nivel { get; set; }

    [Display(Name = "Densidade", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DensidadeError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.000}", ApplyFormatInEditMode = true)]
    public decimal Densidade { get; set; }
  }
}
