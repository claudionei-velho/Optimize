using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class FAjusteViewModel {
    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Display(Name = "Ano", ResourceType = typeof(Properties.Resources))]
    public int Ano { get; set; }

    [Display(Name = "Mes", ResourceType = typeof(Properties.Resources))]
    public int Mes { get; set; }

    public string MesRef {
      get {
        return Bll.Lists.Mes.Data[Mes];
      }
    }

    [Display(Name = "Referencia", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Referencia { get; set; }

    [Display(Name = "Passageiros", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Passageiros { get; set; }

    [Display(Name = "Fator", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0000}")]
    public decimal? Fator { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
  }
}
