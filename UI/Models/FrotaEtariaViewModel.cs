using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Dto.Models;

namespace UI.Models {
  public class FrotaEtariaViewModel {
    [Key, Column(Order = 0)]
    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Key, Column(Order = 1)]
    [Display(Name = "EtariaId", ResourceType = typeof(Properties.Resources))]
    public int EtariaId { get; set; }

    [Display(Name = "Micro", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Micro { get; set; }

    [Display(Name = "Mini", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Mini { get; set; }

    [Display(Name = "Midi", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Midi { get; set; }

    [Display(Name = "Basico", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Basico { get; set; }

    [Display(Name = "Padron", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Padron { get; set; }

    [Display(Name = "Especial", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Especial { get; set; }

    [Display(Name = "Articulado", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Articulado { get; set; }

    [Display(Name = "BiArticulado", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? BiArticulado { get; set; }
    
    [Display(Name = "Frota", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Frota { get; set; }

    public decimal? Ratio { get; set; }
    public decimal? EqvIdade { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual FxEtaria FxEtaria { get; set; }
  }
}
