using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class AGaragemViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "InstalacaoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "InstalacaoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int InstalacaoId { get; set; }

    [Display(Name = "AreaCoberta", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? AreaCoberta {
      get {
        return this.EInstalacao.AreaCoberta;
      }
    }

    [Display(Name = "AreaTotal", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? AreaTotal {
      get {
        return this.EInstalacao.AreaTotal;
      }
    }

    [Display(Name = "QtdEmpregados", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
    public int? QtdEmpregados {
      get {
        return this.EInstalacao.QtdEmpregados;
      }
    }

    [Display(Name = "Frota", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "FrotaError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
    public int Frota { get; set; }

    [Display(Name = "Requisitom2", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "Requisitom2Error", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal Requisitom2 { get; set; }

    [Display(Name = "Necessariom2", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal Necessariom2 {
      get {
        return this.Frota * this.Requisitom2;
      }
    }

    [Display(Name = "Minimom2", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal Minimom2 { get; set; }

    [Display(Name = "Disponivelm2", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "Disponivelm2Error", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal Disponivelm2 { get; set; }

    // Navigation Properites
    public virtual EInstalacao EInstalacao { get; set; }
  }
}
