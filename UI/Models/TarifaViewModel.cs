using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class TarifaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "DataReferencia", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DataReferenciaError", 
              ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime Referencia { get; set; }

    [Display(Name = "Tarifa", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "TarifaError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.00##}", ApplyFormatInEditMode = true)]
    public decimal Valor { get; set; }

    [Display(Name = "Decreto", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.MultilineText), StringLength(128)]
    public string Decreto { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
  }
}
