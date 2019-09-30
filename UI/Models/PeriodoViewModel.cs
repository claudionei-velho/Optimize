using System.ComponentModel.DataAnnotations;

namespace UI.Models {
  public class PeriodoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "Denominação")]
    [Required, StringLength(32)]
    public string Denominacao { get; set; }
  }
}