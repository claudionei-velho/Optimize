using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class LnCorredorViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "CorredorId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "CorredorIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int CorredorId { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Display(Name = "Sentido", ResourceType = typeof(Properties.Resources))]
    [StringLength(2)]
    public string Sentido { get; set; }

    [Display(Name = "PercursoExtensao", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? Extensao { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Corredor Corredor { get; set; }
    public virtual Linha Linha { get; set; }
  }
}
