using System.ComponentModel.DataAnnotations;
using System.Web;
using Dto.Models;

namespace UI.Models {
  public class MapaLinhaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Display(Name = "Sentido", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "SentidoError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public string Sentido { get; set; }

    [Display(Name = "AtendimentoId", ResourceType = typeof(Properties.Resources))]
    public int? AtendimentoId { get; set; }

    [Display(Name = "Descricao", ResourceType = typeof(Properties.Resources)), StringLength(64)]
    public string Descricao { get; set; }

    [Display(Name = "Arquivo", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "ArquivoError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public HttpPostedFileBase File { get; set; }

    // public string Arquivo { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual Atendimento Atendimento { get; set; }
  }
}
