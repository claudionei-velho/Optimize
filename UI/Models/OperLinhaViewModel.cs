using System.ComponentModel.DataAnnotations;

namespace UI.Models {
  public class OperLinhaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "Denominacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DenominacaoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(64)]
    public string Denominacao { get; set; }

    [Display(Name = "Descricao", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.MultilineText), StringLength(512)]
    public string Descricao { get; set; }
  }
}
