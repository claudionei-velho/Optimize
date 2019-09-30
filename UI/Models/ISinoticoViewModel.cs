using System.ComponentModel.DataAnnotations;

namespace UI.Models {
  public class ISinoticoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "Classe", ResourceType = typeof(Properties.Resources))]
    [Required, StringLength(32)]
    public string Classe { get; set; }

    [Display(Name = "Denominacao", ResourceType = typeof(Properties.Resources))]
    [Required, StringLength(64)]
    public string Denominacao { get; set; }

    [Display(Name = "Unidade", ResourceType = typeof(Properties.Resources))]
    [Required, StringLength(16)]
    public string Unidade { get; set; }
  }
}
