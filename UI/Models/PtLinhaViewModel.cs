using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class PtLinhaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

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

    [Display(Name = "OrigemId", ResourceType = typeof(Properties.Resources))]
    public int? OrigemId { get; set; }

    [Display(Name = "DestinoId", ResourceType = typeof(Properties.Resources))]
    public int? DestinoId { get; set; }

    [Display(Name = "PontoFluxo", ResourceType = typeof(Properties.Resources))]
    public int? Fluxo { get; set; }

    public string FluxoCap {
      get {
        return Dto.Lists.Fluxo.Items[Fluxo ?? 0];
      }
    }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Linha Linha { get; set; }
    public virtual Ponto Ponto { get; set; }
    public virtual PtOrigem Origem { get; set; }
    public virtual PtDestino Destino { get; set; }
  }
}
