using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Veiculo {
    public Veiculo() {
      this.Viagens = new HashSet<Viagem>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Numero { get; set; }
    public string Cor { get; set; }
    public int Classe { get; set; }
    public int? Categoria { get; set; }
    public string Placa { get; set; }
    public string Renavam { get; set; }
    public string Antt { get; set; }
    public DateTime? Inicio { get; set; }
    public bool Inativo { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual CVeiculo CVeiculo { get; set; }
    public virtual Empresa Empresa { get; set; }    
    public virtual Chassi Chassi { get; set; }
    public virtual Carroceria Carroceria { get; set; }

    public virtual ICollection<Viagem> Viagens { get; set; }
  }
}
