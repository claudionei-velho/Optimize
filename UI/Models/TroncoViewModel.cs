using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class TroncoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "Prefixo", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PrefixoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Prefixo { get; set; }

    [Display(Name = "Denominacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DenominacaoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(64)]
    public string Denominacao { get; set; }

    [Display(Name = "ExtensaoAB", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? ExtensaoAB { get; set; }

    [Display(Name = "ExtensaoBA", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}", ApplyFormatInEditMode = true)]
    public decimal? ExtensaoBA { get; set; }

    [Display(Name = "Extensao", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.0##}"), Editable(false)]
    public decimal? Extensao {
      get {
        decimal? result = (ExtensaoAB ?? 0) + (ExtensaoBA ?? 0);
        return (result > 0) ? result : null;
      }
    }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
  }
}
