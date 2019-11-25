using System;
using System.ComponentModel.DataAnnotations;

using Bll;
using Dto.Models;

namespace UI.Models {
  public class CLinhaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "ClassLinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "ClassLinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int ClassLinhaId { get; set; }

    [Display(Name = "Descricao", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.MultilineText), StringLength(512)]
    public string Descricao {
      get {
        using Services<ClassLinha> classLinhas = new Services<ClassLinha>();
        return classLinhas.GetFirst(t => t.Id == this.ClassLinhaId).Descricao;
      }
    }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual ClassLinha ClassLinha { get; set; }
  }
}
