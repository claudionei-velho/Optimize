using System.ComponentModel.DataAnnotations;

namespace UI.Models {
  public class PaisViewModel {

    [Display(Name = "Código")]
    public string Id { get; set; }

    [Display(Name = "Pais")]
    public string Nome { get; set; }

    [Display(Name = "Capital")]
    public string Capital { get; set; }

    [Display(Name = "Continente")]
    public string Continente { get; set; }
  }
}