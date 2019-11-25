using System;
using System.ComponentModel.DataAnnotations;

using Bll;
using Dto.Models;

namespace UI.Models {
  public class OperacaoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "OperLinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "OperLinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int OperLinhaId { get; set; }

    [Display(Name = "Descricao", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.MultilineText), StringLength(512)]
    public string Descricao {
      get {
        using Services<OperLinha> operLinhas = new Services<OperLinha>();
        return operLinhas.GetFirst(t => t.Id == this.OperLinhaId).Descricao;
      }
    }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual OperLinha OperLinha { get; set; }
  }
}
