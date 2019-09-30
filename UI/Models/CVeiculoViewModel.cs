using System.ComponentModel.DataAnnotations;

namespace UI.Models {
  public class CVeiculoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "Categoria", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "CategoriaError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Categoria { get; set; }

    [Display(Name = "Classe", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "ClasseError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(32)]
    public string Classe { get; set; }

    [Display(Name = "Minimo", ResourceType = typeof(Properties.Resources))]
    public int? Minimo { get; set; }

    [Display(Name = "Maximo", ResourceType = typeof(Properties.Resources))]
    public int? Maximo { get; set; }
  }
}
