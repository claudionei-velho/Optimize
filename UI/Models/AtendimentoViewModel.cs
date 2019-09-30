using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class AtendimentoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Display(Name = "Prefixo", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PrefixoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Prefixo { get; set; }

    [Display(Name = "AtendimentoDenominacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "AtendimentoDenominacaoError", 
              ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.MultilineText), StringLength(128)]
    public string Denominacao { get; set; }

    [Display(Name = "Uteis", ResourceType = typeof(Properties.Resources))]
    public bool Uteis { get; set; }

    [Display(Name = "Sabados", ResourceType = typeof(Properties.Resources))]
    public bool Sabados { get; set; }

    [Display(Name = "Domingos", ResourceType = typeof(Properties.Resources))]
    public bool Domingos { get; set; }

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
        decimal? _result = (this.ExtensaoAB ?? 0) + (this.ExtensaoBA ?? 0);
        return (_result != 0M) ? _result : null;
      }
    }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
  }
}
