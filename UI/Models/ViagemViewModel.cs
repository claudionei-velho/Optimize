using System;
using System.ComponentModel.DataAnnotations;

using Dto.Models;

namespace UI.Models {
  public class ViagemViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Display(Name = "Item", ResourceType = typeof(Properties.Resources))]
    public int? Item { get; set; }

    [Display(Name = "Data", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DataError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime Data { get; set; }

    [Display(Name = "Sentido", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "SentidoError", 
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(2)]
    public string Sentido { get; set; }

    [Display(Name = "HorarioId", ResourceType = typeof(Properties.Resources))]
    public int? HorarioId { get; set; }

    [Display(Name = "PontoId", ResourceType = typeof(Properties.Resources))]
    public int? PontoId { get; set; }

    [Display(Name = "VeiculoId", ResourceType = typeof(Properties.Resources))]
    public int? VeiculoId { get; set; }

    [Display(Name = "Chegada", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan? Chegada { get; set; }

    [Display(Name = "HoraInicio", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "HoraInicioError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan Inicio { get; set; }

    [Display(Name = "TempoIntervalo", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int? Intervalo {
      get {
        TimeSpan diff = Inicio.Subtract(Chegada ?? Inicio);
        if (diff.TotalMinutes < 0) {
          return 1440 + (int)diff.TotalMinutes;
        }
        else if (diff.TotalMinutes > 0) {
          return (int)diff.TotalMinutes;
        }
        else {
          return null;
        }
      }
    }

    [Display(Name = "HoraTermino", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan? Termino { get; set; }

    [Display(Name = "Passageiros", ResourceType = typeof(Properties.Resources))]
    public int? Passageiros { get; set; }

    [Display(Name = "CatracaInicial", ResourceType = typeof(Properties.Resources))]
    public int? Inicial { get; set; }

    [Display(Name = "CatracaFinal", ResourceType = typeof(Properties.Resources))]
    public int? Final { get; set; }

    [Display(Name = "Responsavel", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Responsavel { get; set; }

    [Display(Name = "TotalCatraca", ResourceType = typeof(Properties.Resources))]
    public int? Catraca {
      get {
        int? result = (Final ?? 0) - (Inicial ?? 0);
        return (result > 0) ? result : null;
      }
    }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Horario Horario { get; set; }
    public virtual LnPesquisa LnPesquisa { get; set; }
    public virtual PtLinha PtLinha { get; set; }
    public virtual Veiculo Veiculo { get; set; }
  }
}
