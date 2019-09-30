using System.ComponentModel.DataAnnotations;

namespace UI.Models {
  public class UfViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "UF")]
    public string Sigla { get; set; }

    [Display(Name = "Nome do Estado")]
    public string Estado { get; set; }

    [Display(Name = "Capital")]
    public string Capital { get; set; }

    [Display(Name = "Região")]
    public string Regiao { get; set; }
  }
}