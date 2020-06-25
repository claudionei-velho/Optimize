using System;
using System.ComponentModel.DataAnnotations;

using Dto.Lists;
using Dto.Models;

namespace UI.Models {
  public class ItTroncoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "TroncoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "TroncoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int TroncoId { get; set; }

    [Display(Name = "Sentido", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "SentidoError", ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(2)]
    public string Sentido { get; set; }

    [Display(Name = "Percurso", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PercursoError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.MultilineText), StringLength(256)]
    public string Percurso { get; set; }

    [Display(Name = "PercursoExtensao", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? Extensao { get; set; }

    [Display(Name = "PavimentoId", ResourceType = typeof(Properties.Resources))]
    public int? PavimentoId { get; set; }

    [Display(Name = "Abrangencia", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:##0.00}", ApplyFormatInEditMode = true), Range(0, 100)]
    public decimal? Abrangencia { get; set; }

    [Display(Name = "CondicaoId", ResourceType = typeof(Properties.Resources))]
    public int? CondicaoId { get; set; }

    public string CondicaoCap {
      get {
        return Condicao.Items[CondicaoId ?? 0];
      }
    }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Tronco Tronco { get; set; }
    public virtual Via Via { get; set; }
  }
}
