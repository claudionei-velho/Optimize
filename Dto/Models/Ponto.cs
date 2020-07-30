using System;
using System.Collections.Generic;

namespace Dto.Models {
  public class Ponto {
    public Ponto() {
      this.Referentes = new HashSet<Adjacencia>();
      this.Adjacentes = new HashSet<Adjacencia>();
      this.PInicioMatriz = new HashSet<MatrizH>();
      this.PTerminoMatriz = new HashSet<MatrizH>();
      this.PtAtendimentos = new HashSet<PtAtendimento>();
      this.PtLinhas = new HashSet<PtLinha>();
      this.PtOrigens = new HashSet<PtOrigem>();
      this.PtDestinos = new HashSet<PtDestino>();
      this.PtTroncos = new HashSet<PtTronco>();

      this.PontosInicio = new HashSet<Referencia>();
      this.PontosTermino = new HashSet<Referencia>();
      this.VetoresInicio = new HashSet<Vetor>();
      this.VetoresTermino = new HashSet<Vetor>();
      this.ArcosInicio = new HashSet<Arco>();
      this.ArcosTermino = new HashSet<Arco>();
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
    public bool Garagem { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }

    public virtual ICollection<Adjacencia> Referentes { get; set; }
    public virtual ICollection<Adjacencia> Adjacentes { get; set; }
    public virtual ICollection<MatrizH> PInicioMatriz { get; set; }
    public virtual ICollection<MatrizH> PTerminoMatriz { get; set; }
    public virtual ICollection<PtAtendimento> PtAtendimentos { get; set; }
    public virtual ICollection<PtLinha> PtLinhas { get; set; }
    public virtual ICollection<PtOrigem> PtOrigens { get; set; }
    public virtual ICollection<PtDestino> PtDestinos { get; set; }
    public virtual ICollection<PtTronco> PtTroncos { get; set; }
    public virtual ICollection<Referencia> PontosInicio { get; set; }
    public virtual ICollection<Referencia> PontosTermino { get; set; }
    public virtual ICollection<Vetor> VetoresInicio { get; set; }
    public virtual ICollection<Vetor> VetoresTermino { get; set; }
    public virtual ICollection<Arco> ArcosInicio { get; set; }
    public virtual ICollection<Arco> ArcosTermino { get; set; }
  }
}
