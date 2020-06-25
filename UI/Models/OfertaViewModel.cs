using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class OfertaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Display(Name = "Ano", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "AnoError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int Ano { get; set; }

    [Display(Name = "Mes", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "MesError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int Mes { get; set; }

    public string MesCap {
      get {
        return Dto.Lists.Mes.Items[Mes];
      }
    }

    [Display(Name = "ClasseTarifa", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "ClasseTarifaError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int Categoria { get; set; }

    [Display(Name = "Passageiros", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PassageirosError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int Passageiros { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual TCategoria TCategoria { get; set; }
  }
}
