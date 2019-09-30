using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Ponto {
    public Ponto() {
      this.PtAtendimentos = new HashSet<PtAtendimento>();
      this.PtLinhas = new HashSet<PtLinha>();
      this.PtOrigens = new HashSet<PtOrigem>();
      this.PtDestinos = new HashSet<PtDestino>();
      this.PtTroncos = new HashSet<PtTronco>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Prefixo { get; set; }
    public string Identificacao { get; set; }
    public string Endereco { get; set; }
    public string EnderecoRef { get; set; }
    public int? Cep { get; set; }
    public string Bairro { get; set; }
    public string Municipio { get; set; }
    public string UfId { get; set; }
    public bool Intercambio { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }

    public virtual ICollection<PtAtendimento> PtAtendimentos { get; set; }
    public virtual ICollection<PtLinha> PtLinhas { get; set; }
    public virtual ICollection<PtOrigem> PtOrigens { get; set; }
    public virtual ICollection<PtDestino> PtDestinos { get; set; }
    public virtual ICollection<PtTronco> PtTroncos { get; set; }
  }
}
