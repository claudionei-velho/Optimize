using System;
using System.ComponentModel.DataAnnotations;

using Bll.Lists;
using Dto.Models;

namespace UI.Models {
  public class ChassiViewModel {
    [Key]
    [Display(Name = "VeiculoId", ResourceType = typeof(Properties.Resources))]
    public int VeiculoId { get; set; }

    [Display(Name = "Fabricante", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string Fabricante { get; set; }

    [Display(Name = "Modelo", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string Modelo { get; set; }

    [Display(Name = "ChassiNo", ResourceType = typeof(Properties.Resources))]
    [Required, StringLength(32)]
    public string ChassiNo { get; set; }

    [Display(Name = "Ano", ResourceType = typeof(Properties.Resources))]
    public int? Ano { get; set; }

    [Display(Name = "Aquisicao", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Aquisicao { get; set; }

    [Display(Name = "Fornecedor", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string Fornecedor { get; set; }

    [Display(Name = "NotaFiscal", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string NotaFiscal { get; set; }

    [Display(Name = "Valor", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.00##}", ApplyFormatInEditMode = true)]
    public decimal? Valor { get; set; }

    [Display(Name = "ChaveNfe", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string ChaveNfe { get; set; }

    [Display(Name = "MotorId", ResourceType = typeof(Properties.Resources))]
    public int? MotorId { get; set; }

    [Display(Name = "Potencia", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Potencia { get; set; }

    [Display(Name = "PosMotor", ResourceType = typeof(Properties.Resources))]
    public int? PosMotor { get; set; }

    public string PosMotorCap {
      get {
        return Posicao.Data[this.PosMotor ?? 0];
      }
    }

    [Display(Name = "EixosFrente", ResourceType = typeof(Properties.Resources))]
    public byte EixosFrente { get; set; }

    [Display(Name = "EixosTras", ResourceType = typeof(Properties.Resources))]
    public byte EixosTras { get; set; }

    [Display(Name = "PneusFrente", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string PneusFrente { get; set; }

    [Display(Name = "PneusTras", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string PneusTras { get; set; }

    [Display(Name = "TransmiteId", ResourceType = typeof(Properties.Resources))]
    public int? TransmiteId { get; set; }

    public string TransmiteCap {
      get {
        return Transmissao.Data[this.TransmiteId ?? 1];
      }
    }

    [Display(Name = "DirecaoId", ResourceType = typeof(Properties.Resources))]
    public int? DirecaoId { get; set; }

    public string DirecaoCap {
      get {
        return Direcao.Data[this.DirecaoId ?? 1];
      }
    }

    // Navigation Properties
    public virtual Motor Motor { get; set; }
    public virtual Veiculo Veiculo { get; set; }
  }
}
