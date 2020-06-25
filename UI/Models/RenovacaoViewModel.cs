using System;
using System.ComponentModel.DataAnnotations;

using Dto.Lists;
using Dto.Models;

namespace UI.Models {
  public class RenovacaoViewModel {
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

    [Display(Name = "DiaId", ResourceType = typeof(Properties.Resources))]
    public int? DiaId { get; set; }

    public string DiaIdName {
      get {
        return Workday.Items[DiaId ?? 0];
      }
    }

    [Display(Name = "Indice", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "IndiceError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.000}", ApplyFormatInEditMode = true)]
    public decimal Indice { get; set; }

    [Display(Name = "Referencia", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Referencia { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
  }
}
