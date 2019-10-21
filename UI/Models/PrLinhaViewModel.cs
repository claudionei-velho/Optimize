﻿using System;
using System.ComponentModel.DataAnnotations;

using Bll.Lists;
using Dto.Models;

namespace UI.Models {
  public class PrLinhaViewModel {
    [Key]
    public int Id { get; set; }

    [Display(Name = "LinhaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "LinhaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int LinhaId { get; set; }

    [Display(Name = "EPeriodoId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "EPeriodoIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int PeriodoId { get; set; }

    [Display(Name = "DiaId", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "DiaIdError", ErrorMessageResourceType = typeof(Properties.Resources))]
    public int DiaId { get; set; }

    public string DiaIdName {
      get {
        return new Workday().Data[DiaId];
      }
    }

    [Display(Name = "HoraInicio", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "HoraInicioError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan Inicio { get; set; }

    [Display(Name = "HoraTermino", ResourceType = typeof(Properties.Resources))]
    [Required(ErrorMessageResourceName = "HoraTerminoError", ErrorMessageResourceType = typeof(Properties.Resources))]
    [DataType(DataType.Time)]
    public TimeSpan Termino { get; set; }

    [Display(Name = "CicloAB", ResourceType = typeof(Properties.Resources))]
    public int? CicloAB { get; set; }

    [Display(Name = "CicloBA", ResourceType = typeof(Properties.Resources))]
    public int? CicloBA { get; set; }

    [Display(Name = "CVeiculoId", ResourceType = typeof(Properties.Resources))]
    public int? CVeiculoId { get; set; }

    [Display(Name = "OcupacaoId", ResourceType = typeof(Properties.Resources))]
    public int? OcupacaoId { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual CVeiculo CVeiculo { get; set; }
    public virtual EPeriodo EPeriodo { get; set; }
    public virtual Linha Linha { get; set; }
    public virtual Ocupacao Ocupacao { get; set; }
  }
}