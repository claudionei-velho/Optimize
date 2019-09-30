using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class CustoViewModel {
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

    [Display(Name = "Fixo", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "FixoError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.00##}", ApplyFormatInEditMode = true)]
    public decimal Fixo { get; set; }

    [Display(Name = "Variavel", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "VariavelError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.00##}", ApplyFormatInEditMode = true)]
    public decimal Variavel { get; set; }

    [Display(Name = "CustoTotal", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.00##}", ApplyFormatInEditMode = false)]
    public decimal CustoTotal {
      get {
        return Fixo + Variavel;
      }
    }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
  }
}
