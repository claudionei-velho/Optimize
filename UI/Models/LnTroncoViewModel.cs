using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class LnTroncoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "TroncoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "TroncoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int TroncoId { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [ScaffoldColumn(false)]
    public DateTime Cadastro { get; set; }

    // Navigation Properties
    public virtual Tronco Tronco { get; set; }
    public virtual Linha Linha { get; set; }
  }
}
