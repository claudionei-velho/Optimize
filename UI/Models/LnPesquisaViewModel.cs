using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class LnPesquisaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "PesquisaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PesquisaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int PesquisaId { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Display(Name = "Uteis", ResourceType = typeof(Properties.Resources))]
    public bool Uteis { get; set; }

    [Display(Name = "Sabados", ResourceType = typeof(Properties.Resources))]
    public bool Sabados { get; set; }

    [Display(Name = "Domingos", ResourceType = typeof(Properties.Resources))]
    public bool Domingos { get; set; }

    [Display(Name = "Responsavel", ResourceType = typeof(Properties.Resources))]
    public string Responsavel { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual Pesquisa Pesquisa { get; set; }
  }
}
