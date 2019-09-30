using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class PesquisaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "Identificacao", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(64)]
    public string Identificacao { get; set; }

    [Display(Name = "DataInicio", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DataInicioError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime Inicio { get; set; }

    [Display(Name = "DataTermino", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DataTerminoError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime Termino { get; set; }

    [Display(Name = "TerminalId", ResourceType = typeof(Properties.Resources))]
    public int? TerminalId { get; set; }

    [Display(Name = "TroncoId", ResourceType = typeof(Properties.Resources))]
    public int? TroncoId { get; set; }

    [Display(Name = "CorredorId", ResourceType = typeof(Properties.Resources))]
    public int? CorredorId { get; set; }

    [Display(Name = "OperacaoId", ResourceType = typeof(Properties.Resources))]
    public int? OperacaoId { get; set; }

    [Display(Name = "Classificacao", ResourceType = typeof(Properties.Resources))]
    public int? Classificacao { get; set; }

    [Display(Name = "Interna", ResourceType = typeof(Properties.Resources))]
    public bool Interna { get; set; }

    [Display(Name = "Fornecedor", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string Fornecedor { get; set; }

    [Display(Name = "Contrato", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Contrato { get; set; }

    [Display(Name = "Uteis", ResourceType = typeof(Properties.Resources))]
    public bool Uteis { get; set; }

    [Display(Name = "Sabados", ResourceType = typeof(Properties.Resources))]
    public bool Sabados { get; set; }

    [Display(Name = "Domingos", ResourceType = typeof(Properties.Resources))]
    public bool Domingos { get; set; }

    [Display(Name = "Responsavel", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string Responsavel { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual Terminal Terminal { get; set; }
    public virtual Tronco Tronco { get; set; }
    public virtual Corredor Corredor { get; set; }
    public virtual Operacao Operacao { get; set; }
    public virtual CLinha CLinha { get; set; }
  }
}
