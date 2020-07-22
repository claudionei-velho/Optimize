﻿using System;
using System.ComponentModel.DataAnnotations;

namespace UI.Models {
  public class VetorViewModel {
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public int DiaId { get; set; }
    public TimeSpan Inicio { get; set; }
    public int? PInicioId { get; set; }
    public TimeSpan? Termino { get; set; }
    public int? PTerminoId { get; set; }

    [ScaffoldColumn(false)]
    public DateTime? Cadastro { get; set; }
  }
}