using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dto.Models {
  public class Linha {
    private readonly char[] charsToTrim = new char[] { ' ', ';', '/' };

    public Linha() {
      this.Atendimentos = new HashSet<Atendimento>();
      this.FAjustes = new HashSet<FAjuste>();
      this.Horarios = new HashSet<Horario>();
      this.Itinerarios = new HashSet<Itinerario>();
      this.LnCorredores = new HashSet<LnCorredor>();
      this.LnPesquisas = new HashSet<LnPesquisa>();
      this.Ofertas = new HashSet<Oferta>();
      this.PrLinhas = new HashSet<PrLinha>();
      this.PtLinhas = new HashSet<PtLinha>();
      this.Referencias = new HashSet<Referencia>();
      this.Renovacoes = new HashSet<Renovacao>();
      this.TotalViagens = new HashSet<TotalViagem>();
      this.Dimensionamentos = new HashSet<Dimensionamento>();
      this.PtOrigens = new HashSet<PtOrigem>();
      this.PtDestinos = new HashSet<PtDestino>();
      this.MapasLinha = new HashSet<MapaLinha>();
      this.Sinoticos = new HashSet<Sinotico>();
      this.LnTerminais = new HashSet<LnTerminal>();
      this.LnTroncos = new HashSet<LnTronco>();

      // Reports (Database Views)
      this.Operacionais = new HashSet<Operacional>();
      this.PlanOperacionais = new HashSet<PlanOperacional>();
      this.PeriodosTipicos = new HashSet<PeriodoTipico>();
      this.ViagensLinha = new HashSet<ViagemLinha>();
      this.ItinerariosDistinct = new HashSet<ItinerarioDistinct>();
      this.ViagensHora = new HashSet<ViagemHora>();
      this.DemandasMes = new HashSet<DemandaMes>();
      this.DemandasMod = new HashSet<DemandaMod>();
      this.DemandasAno = new HashSet<DemandaAno>();
    }

    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Prefixo { get; set; }
    public string Denominacao { get; set; }
    public string Viagem { get; set; }
    public bool Uteis { get; set; }
    public bool Sabados { get; set; }
    public bool Domingos { get; set; }
    public bool Escolar { get; set; }

    [NotMapped]
    public string DiasOp {
      get {        
        StringBuilder aux = new StringBuilder();
        if (this.Uteis) {
          aux.Append("Dias Úteis; ");
        }
        if (this.Sabados) {
          aux.Append("Sábados; ");
        }
        if (this.Domingos) {
          aux.Append("Domingos");
        }
        return aux.ToString().Trim(charsToTrim);
      }
    }

    public int DominioId { get; set; }
    public int OperacaoId { get; set; }
    public int Classificacao { get; set; }
    public bool Captacao { get; set; }
    public bool Transporte { get; set; }
    public bool Distribuicao { get; set; }

    [NotMapped]
    public string Funcoes {
      get {
        StringBuilder aux = new StringBuilder();
        if (this.Captacao) {
          aux.Append("Captação; ");
        }
        if (this.Transporte) {
          aux.Append("Transporte; ");
        }
        if (this.Distribuicao) {
          aux.Append("Distribuição");
        }
        return aux.ToString().Trim(charsToTrim);
      }
    }

    public decimal? ExtensaoAB { get; set; }
    public decimal? ExtensaoBA { get; set; }

    [NotMapped]
    public decimal? Extensao {
      get {
        return this.ExtensaoAB + this.ExtensaoBA;
      }
    }

    public int? LoteId { get; set; }
    public DateTime? Cadastro { get; set; }

    // Navigation Properties
    public virtual Empresa Empresa { get; set; }
    public virtual EDominio EDominio { get; set; }
    public virtual Operacao Operacao { get; set; }
    public virtual CLinha CLinha { get; set; }
    public virtual Lote Lote { get; set; }

    public virtual ICollection<Atendimento> Atendimentos { get; set; }
    public virtual ICollection<FAjuste> FAjustes { get; set; }
    public virtual ICollection<Horario> Horarios { get; set; }
    public virtual ICollection<Itinerario> Itinerarios { get; set; }
    public virtual ICollection<LnCorredor> LnCorredores { get; set; }
    public virtual ICollection<LnPesquisa> LnPesquisas { get; set; }
    public virtual ICollection<Oferta> Ofertas { get; set; }
    public virtual ICollection<PrLinha> PrLinhas { get; set; }
    public virtual ICollection<PtLinha> PtLinhas { get; set; }
    public virtual ICollection<Referencia> Referencias { get; set; }
    public virtual ICollection<Renovacao> Renovacoes { get; set; }
    public virtual ICollection<TotalViagem> TotalViagens { get; set; }
    public virtual ICollection<Dimensionamento> Dimensionamentos { get; set; }
    public virtual ICollection<PtOrigem> PtOrigens { get; set; }
    public virtual ICollection<PtDestino> PtDestinos { get; set; }
    public virtual ICollection<MapaLinha> MapasLinha { get; set; }
    public virtual ICollection<Sinotico> Sinoticos { get; set; }
    public virtual ICollection<LnTerminal> LnTerminais { get; set; }
    public virtual ICollection<LnTronco> LnTroncos { get; set; }

    // Reports (Database Views)
    public virtual ICollection<Operacional> Operacionais { get; set; }
    public virtual ICollection<PlanOperacional> PlanOperacionais { get; set; }
    public virtual ICollection<PeriodoTipico> PeriodosTipicos { get; set; }
    public virtual ICollection<ViagemLinha> ViagensLinha { get; set; }
    public virtual ICollection<ItinerarioDistinct> ItinerariosDistinct { get; set; }
    public virtual ICollection<ViagemHora> ViagensHora { get; set; }
    public virtual ICollection<DemandaMes> DemandasMes { get; set; }
    public virtual ICollection<DemandaMod> DemandasMod { get; set; }
    public virtual ICollection<DemandaAno> DemandasAno { get; set; }
  }
}
