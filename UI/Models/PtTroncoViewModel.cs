using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class PtTroncoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "TroncoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "TroncoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int TroncoId { get; set; }

    [Display(Name = "Sentido", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "SentidoError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public string Sentido { get; set; }

    [Display(Name = "PontoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PontoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int PontoId { get; set; }

    public string PontoCap {
      get {
        return $"{Ponto.Prefixo} : {Ponto.Identificacao}";
      }
    }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Tronco Tronco { get; set; }
    public virtual Ponto Ponto { get; set; }
  }
}
