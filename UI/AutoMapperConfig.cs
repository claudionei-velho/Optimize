using AutoMapper;
using AutoMapper.Configuration;

using Dto.Models;
using UI.Models;

namespace UI {
  public class AutomapperConfig {
    public static void RegisterMappings() {
      var cfg = new MapperConfigurationExpression();

      cfg.CreateMap<EmpresaViewModel, Empresa>().ReverseMap();
      cfg.CreateMap<PaisViewModel, Pais>().ReverseMap();
      cfg.CreateMap<UfViewModel, Uf>().ReverseMap();
      cfg.CreateMap<LinhaViewModel, Linha>().ReverseMap();
      cfg.CreateMap<CLinhaViewModel, CLinha>().ReverseMap();
      cfg.CreateMap<DominioViewModel, Dominio>().ReverseMap();
      cfg.CreateMap<PeriodoViewModel, Periodo>().ReverseMap();
      cfg.CreateMap<EDominioViewModel, EDominio>().ReverseMap();
      cfg.CreateMap<EPeriodoViewModel, EPeriodo>().ReverseMap();
      cfg.CreateMap<OperacaoViewModel, Operacao>().ReverseMap();
      cfg.CreateMap<UsuarioViewModel, Usuario>().ReverseMap();
      cfg.CreateMap<PontoViewModel, Ponto>().ReverseMap();
      cfg.CreateMap<CorredorViewModel, Corredor>().ReverseMap();
      cfg.CreateMap<TerminalViewModel, Terminal>().ReverseMap();
      cfg.CreateMap<TServicoViewModel, TServico>().ReverseMap();
      cfg.CreateMap<TCategoriaViewModel, TCategoria>().ReverseMap();
      cfg.CreateMap<TarifaViewModel, Tarifa>().ReverseMap();
      cfg.CreateMap<AtendimentoViewModel, Atendimento>().ReverseMap();
      cfg.CreateMap<HorarioViewModel, Horario>().ReverseMap();
      cfg.CreateMap<FAjusteViewModel, FAjuste>().ReverseMap();
      cfg.CreateMap<VeiculoViewModel, Veiculo>().ReverseMap();
      cfg.CreateMap<VeiculoAttViewModel, VeiculoAtt>().ReverseMap();
      cfg.CreateMap<SpecVeiculoViewModel, SpecVeiculo>().ReverseMap();
      cfg.CreateMap<ChassiViewModel, Chassi>().ReverseMap();
      cfg.CreateMap<CarroceriaViewModel, Carroceria>().ReverseMap();
      cfg.CreateMap<CVeiculoViewModel, CVeiculo>().ReverseMap();
      cfg.CreateMap<PesquisaViewModel, Pesquisa>().ReverseMap();
      cfg.CreateMap<OfertaViewModel, Oferta>().ReverseMap();
      cfg.CreateMap<ItinerarioViewModel, Itinerario>().ReverseMap();
      cfg.CreateMap<RenovacaoViewModel, Renovacao>().ReverseMap();
      cfg.CreateMap<PtLinhaViewModel, PtLinha>().ReverseMap();
      cfg.CreateMap<PrLinhaViewModel, PrLinha>().ReverseMap();
      cfg.CreateMap<LnPesquisaViewModel, LnPesquisa>().ReverseMap();
      cfg.CreateMap<LnCorredorViewModel, LnCorredor>().ReverseMap();
      cfg.CreateMap<ViagemViewModel, Viagem>().ReverseMap();
      cfg.CreateMap<ViaViewModel, Via>().ReverseMap();
      cfg.CreateMap<OcupacaoViewModel, Ocupacao>().ReverseMap();
      cfg.CreateMap<ItAtendimentoViewModel, ItAtendimento>().ReverseMap();
      cfg.CreateMap<PtAtendimentoViewModel, PtAtendimento>().ReverseMap();
      cfg.CreateMap<TotalViagemViewModel, TotalViagem>().ReverseMap();
      cfg.CreateMap<ClassLinhaViewModel, ClassLinha>().ReverseMap();
      cfg.CreateMap<OperLinhaViewModel, OperLinha>().ReverseMap();
      cfg.CreateMap<DimensionamentoViewModel, Dimensionamento>().ReverseMap();
      cfg.CreateMap<EUsuarioViewModel, EUsuario>().ReverseMap();
      cfg.CreateMap<ISinoticoViewModel, ISinotico>().ReverseMap();
      cfg.CreateMap<SinoticoViewModel, Sinotico>().ReverseMap();
      cfg.CreateMap<CustoViewModel, Custo>().ReverseMap();
      cfg.CreateMap<LnTerminalViewModel, LnTerminal>().ReverseMap();
      cfg.CreateMap<TroncoViewModel, Tronco>().ReverseMap();
      cfg.CreateMap<ItTroncoViewModel, ItTronco>().ReverseMap();
      cfg.CreateMap<PtTroncoViewModel, PtTronco>().ReverseMap();
      cfg.CreateMap<LnTroncoViewModel, LnTronco>().ReverseMap();
      cfg.CreateMap<FInstalacaoViewModel, FInstalacao>().ReverseMap();
      cfg.CreateMap<InstalacaoViewModel, Instalacao>().ReverseMap();
      cfg.CreateMap<EInstalacaoViewModel, EInstalacao>().ReverseMap();
      cfg.CreateMap<AGaragemViewModel, AGaragem>().ReverseMap();
      cfg.CreateMap<FrotaEtariaViewModel, FrotaEtaria>().ReverseMap();
      cfg.CreateMap<ECVeiculoViewModel, ECVeiculo>().ReverseMap();
      cfg.CreateMap<OperacionalViewModel, Operacional>().ReverseMap();

      Mapper.Initialize(cfg);
    }
  }
}
