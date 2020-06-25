using System;
using System.ComponentModel.DataAnnotations;

using Dto.Lists;
using Dto.Models;

namespace UI.Models {
  public class SpecVeiculoViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "EmpresaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EmpresaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int EmpresaId { get; set; }

    [Display(Name = "Numero", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "NumeroError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Numero { get; set; }

    [Display(Name = "Cor", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "CorError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(32)]
    public string Cor { get; set; }

    [Display(Name = "Classe", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "ClasseError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int Classe { get; set; }

    [Display(Name = "Categoria", ResourceType = typeof(Properties.Resources))]
    public int? Categoria { get; set; }

    public string CategoriaCap {
      get {
        return Dto.Lists.Categoria.Items[this.Categoria ?? 0];
      }
    }

    [Display(Name = "Placa", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PlacaError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Placa { get; set; }

    [Display(Name = "Renavam", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "RenavamError",
              ErrorMessageResourceType = typeof(Properties.Resources)), StringLength(16)]
    public string Renavam { get; set; }

    [Display(Name = "Antt", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string Antt { get; set; }

    [Display(Name = "InicioOperacao", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Inicio { get; set; }

    [Display(Name = "Fabricante", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string ChassiFabricante { get; set; }

    [Display(Name = "Modelo", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string ChassiModelo { get; set; }

    [Display(Name = "ChassiNo", ResourceType = typeof(Properties.Resources))]
    [Required, StringLength(32)]
    public string ChassiNo { get; set; }

    [Display(Name = "Ano", ResourceType = typeof(Properties.Resources))]
    public int? ChassiAno { get; set; }

    [Display(Name = "Aquisicao", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? ChassiAquisicao { get; set; }

    [Display(Name = "Fornecedor", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string ChassiFornecedor { get; set; }

    [Display(Name = "NotaFiscal", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string ChassiNota { get; set; }

    [Display(Name = "Valor", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.00##}", ApplyFormatInEditMode = true)]
    public decimal? ChassiValor { get; set; }

    [Display(Name = "ChaveNfe", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string ChassiChaveNfe { get; set; }

    [Display(Name = "MotorId", ResourceType = typeof(Properties.Resources))]
    public int? MotorId { get; set; }

    [Display(Name = "Potencia", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Potencia { get; set; }

    [Display(Name = "PosMotor", ResourceType = typeof(Properties.Resources))]
    public int? PosMotor { get; set; }

    public string PosMotorCap {
      get {
        return Posicao.Items[this.PosMotor ?? 0];
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
        return Transmissao.Items[this.TransmiteId ?? 1];
      }
    }

    [Display(Name = "DirecaoId", ResourceType = typeof(Properties.Resources))]
    public int? DirecaoId { get; set; }

    public string DirecaoCap {
      get {
        return Direcao.Items[this.DirecaoId ?? 1];
      }
    }

    [Display(Name = "Fabricante", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string CarroceriaFabricante { get; set; }

    [Display(Name = "Modelo", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string CarroceriaModelo { get; set; }

    [Display(Name = "Referencia", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Referencia { get; set; }

    [Display(Name = "Ano", ResourceType = typeof(Properties.Resources))]
    public int? CarroceriaAno { get; set; }

    [Display(Name = "Aquisicao", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? CarroceriaAquisicao { get; set; }

    [Display(Name = "Fornecedor", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string CarroceriaFornecedor { get; set; }

    [Display(Name = "NotaFiscal", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string CarroceriaNota { get; set; }

    [Display(Name = "Valor", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.00##}", ApplyFormatInEditMode = true)]
    public decimal? CarroceriaValor { get; set; }

    [Display(Name = "ChaveNfe", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string CarroceriaChaveNfe { get; set; }

    [Display(Name = "Encarrocamento", ResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Encarrocamento { get; set; }

    [Display(Name = "QuemEncarroca", ResourceType = typeof(Properties.Resources))]
    [StringLength(64)]
    public string QuemEncarroca { get; set; }

    [Display(Name = "NotaEncarroca", ResourceType = typeof(Properties.Resources))]
    [StringLength(16)]
    public string NotaEncarroca { get; set; }

    [Display(Name = "ValorEncarroca", ResourceType = typeof(Properties.Resources))]
    [DisplayFormat(DataFormatString = "{0:#,##0.00##}", ApplyFormatInEditMode = true)]
    public decimal? ValorEncarroca { get; set; }

    [Display(Name = "Portas", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PortasError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public byte Portas { get; set; }

    [Display(Name = "Assentos", ResourceType = typeof(Properties.Resources))]
    public byte? Assentos { get; set; }

    [Display(Name = "Capacidade", ResourceType = typeof(Properties.Resources))]
    public byte? Capacidade { get; set; }

    [Display(Name = "Piso", ResourceType = typeof(Properties.Resources))]
    [StringLength(32)]
    public string Piso { get; set; }

    [Display(Name = "EscapeV", ResourceType = typeof(Properties.Resources))]
    public bool EscapeV { get; set; }

    [Display(Name = "EscapeH", ResourceType = typeof(Properties.Resources))]
    public bool EscapeH { get; set; }

    [Display(Name = "Catraca", ResourceType = typeof(Properties.Resources))]
    public int? Catraca { get; set; }

    public string CatracaCap {
      get {
        return Posicao.Items[this.Catraca ?? 0];
      }
    }

    [Display(Name = "PortaIn", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "PortaInError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int PortaIn { get; set; }

    public string PortaInCap {
      get {
        return Posicao.Items[this.PortaIn];
      }
    }

    [Display(Name = "SaidaFrente", ResourceType = typeof(Properties.Resources))]
    public bool SaidaFrente { get; set; }

    [Display(Name = "SaidaMeio", ResourceType = typeof(Properties.Resources))]
    public bool SaidaMeio { get; set; }

    [Display(Name = "SaidaTras", ResourceType = typeof(Properties.Resources))]
    public bool SaidaTras { get; set; }

    [Display(Name = "EtariaId", ResourceType = typeof(Properties.Resources))]
    public int? EtariaId { get; set; }

    public int? Idade { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual CVeiculo CVeiculo { get; set; }
    public virtual Empresa Empresa { get; set; }
    public virtual Motor Motor { get; set; }
    public virtual FxEtaria FxEtaria { get; set; }
  }
}
