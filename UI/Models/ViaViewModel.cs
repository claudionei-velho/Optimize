using System.ComponentModel.DataAnnotations;

namespace UI.Models {
  public class ViaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "Denominacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DenominacaoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(64)]
    public string Denominacao { get; set; }
  }
}
