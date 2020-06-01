using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class LinhaViewModel {
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
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(128)]
    public string Denominacao { get; set; }

    [Display(Name = "Viagem", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.MultilineText), StringLength(256)]
    public string Viagem { get; set; }

    [Display(Name = "Uteis", ResourceType = typeof(Properties.Resources))]
    public bool Uteis { get; set; }

    [Display(Name = "Sabados", ResourceType = typeof(Properties.Resources))]
    public bool Sabados { get; set; }

    [Display(Name = "Domingos", ResourceType = typeof(Properties.Resources))]
    public bool Domingos { get; set; }

    [Display(Name = "DominioId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DominioIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int DominioId { get; set; }

    [Display(Name = "OperacaoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "OperacaoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int OperacaoId { get; set; }

    [Display(Name = "Classificacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "ClassificacaoError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int Classificacao { get; set; }

    [Display(Name = "Captacao", ResourceType = typeof(Properties.Resources))]
    public bool Captacao { get; set; }

    [Display(Name = "Transporte", ResourceType = typeof(Properties.Resources))]
    public bool Transporte { get; set; }

    [Display(Name = "Distribuicao", ResourceType = typeof(Properties.Resources))]
    public bool Distribuicao { get; set; }

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

    [Display(Name = "LoteId", ResourceType = typeof(Properties.Resources))]
    public int? LoteId { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual EDominio EDominio { get; set; }
    public virtual Operacao Operacao { get; set; }
    public virtual CLinha CLinha { get; set; }
    public virtual Lote Lote { get; set; }
  }
}
