using System;
using System.ComponentModel.DataAnnotations;

using Bll;
using Dto.Models;
using UI.Properties;

namespace UI.Models {
  public class TCategoriaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "Denominacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DenominacaoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(32)]
    public string Denominacao { get; set; }

    [Display(Name = "Gratuidade", ResourceType = typeof(Properties.Resources))]
    public bool Gratuidade { get; set; }

    [Display(Name = "Rateio", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? Rateio { get; set; }

    [Display(Name = "Tarifa", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Currency)]
    public decimal? Tarifa {
      get {
        using Services<TarifaMod> tarifas = new Services<TarifaMod>();
        return tarifas.GetById(this.Id)?.Tarifa;
      }
    }

    public string TarifaCap {
      get {
        return (!this.Gratuidade) ? string.Format("{0:C}", Tarifa) : Resources.GratuidadeCap;
      }
    }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
  }
}
