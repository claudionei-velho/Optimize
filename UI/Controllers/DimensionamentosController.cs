using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using OfficeOpenXml;
using PagedList;

using Bll.Lists;
using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Properties;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class DimensionamentosController : Controller {
    private DimensionamentoService dimensionamento = new DimensionamentoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<DimensionamentoViewModel, Dimensionamento>().ReverseMap();
                                          }).CreateMapper();

    // GET: Dimensionamentos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.dimensionamento = new DimensionamentoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<DimensionamentoViewModel>>(await dimensionamento.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    public ActionResult Export() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.dimensionamento = new DimensionamentoService(user.ID);

      using (ExcelPackage excel = new ExcelPackage()) {
        var workSheet = excel.Workbook.Worksheets.Add("Plan1");

        // Header Section
        int row = 1;
        workSheet.Cells[row, 1].Value = Resources.PesquisaId;
        workSheet.Cells[row, 2].Value = Resources.LinhaId;
        workSheet.Cells[row, 3].Value = Resources.DiaId;
        workSheet.Cells[row, 4].Value = Resources.PeriodoId;
        workSheet.Cells[row, 5].Value = Resources.Sentido;
        workSheet.Cells[row, 6].Value = Resources.QtdViagens;
        workSheet.Cells[row, 7].Value = Resources.HoraInicio;
        workSheet.Cells[row, 8].Value = Resources.HoraTermino;
        workSheet.Cells[row, 9].Value = Resources.Duracao;
        workSheet.Cells[row, 10].Value = Resources.Ociosidade;
        workSheet.Cells[row, 11].Value = Resources.Passageiros;
        workSheet.Cells[row, 12].Value = Resources.Ajustado;
        workSheet.Cells[row, 13].Value = Resources.Critica;
        workSheet.Cells[row, 14].Value = Resources.CriticaAjuste;
        workSheet.Cells[row, 15].Value = Resources.Media;
        workSheet.Cells[row, 16].Value = Resources.MediaAjuste;
        workSheet.Cells[row, 17].Value = Resources.Intervalo;
        workSheet.Cells[row, 18].Value = Resources.Fluxo;
        workSheet.Cells[row, 19].Value = Resources.FluxoAjuste;
        workSheet.Cells[row, 20].Value = Resources.LotacaoE;
        workSheet.Cells[row, 21].Value = Resources.PrognosticoE;
        workSheet.Cells[row, 22].Value = Resources.IntervaloE;
        workSheet.Cells[row, 23].Value = Resources.LotacaoP;
        workSheet.Cells[row, 24].Value = Resources.PrognosticoP;
        workSheet.Cells[row, 25].Value = Resources.IntervaloP;
        workSheet.Cells[row, 26].Value = Resources.Veiculos;
        workSheet.Cells[row, 27].Value = Resources.CicloAB;
        workSheet.Cells[row, 28].Value = Resources.CicloBA;
        workSheet.Cells[row, 29].Value = Resources.VeiculosE;
        workSheet.Cells[row, 30].Value = Resources.VeiculosP;
        workSheet.Cells[row, 31].Value = Resources.ExtensaoSentido;
        workSheet.Cells[row, 32].Value = Resources.KmTotal;
        workSheet.Cells[row, 33].Value = Resources.KmTotalE;
        workSheet.Cells[row, 34].Value = Resources.KmTotalP;

        // Detail Section
        Workday workDay = new Workday();
        foreach (Dimensionamento item in dimensionamento.GetQuery()) {
          workSheet.Cells[++row, 1].Value = item.Pesquisa.Identificacao;
          workSheet.Cells[row, 2].Value = item.Linha.Denominacao;
          workSheet.Cells[row, 3].Value = workDay.Data[item.DiaId];
          workSheet.Cells[row, 4].Value = item.PrLinha.EPeriodo.Denominacao;
          workSheet.Cells[row, 5].Value = item.Sentido;
          workSheet.Cells[row, 6].Value = item.QtdViagens;
          workSheet.Cells[row, 7].Value = string.Format("{0:t}", item.Inicio);
          workSheet.Cells[row, 8].Value = string.Format("{0:t}", item.Termino);
          workSheet.Cells[row, 9].Value = item.Duracao;
          workSheet.Cells[row, 10].Value = item.Ociosidade;
          workSheet.Cells[row, 11].Value = item.Passageiros;
          workSheet.Cells[row, 12].Value = item.Ajustado;
          workSheet.Cells[row, 13].Value = item.Critica;
          workSheet.Cells[row, 14].Value = item.CriticaAjuste;
          workSheet.Cells[row, 15].Value = item.Media;
          workSheet.Cells[row, 16].Value = item.MediaAjuste;
          workSheet.Cells[row, 17].Value = item.Intervalo;
          workSheet.Cells[row, 18].Value = item.Fluxo;
          workSheet.Cells[row, 19].Value = item.FluxoAjuste;
          workSheet.Cells[row, 20].Value = item.LotacaoE;
          workSheet.Cells[row, 21].Value = item.PrognosticoE;
          workSheet.Cells[row, 22].Value = item.IntervaloE;
          workSheet.Cells[row, 23].Value = item.LotacaoP;
          workSheet.Cells[row, 24].Value = item.PrognosticoP;
          workSheet.Cells[row, 25].Value = item.IntervaloP;
          workSheet.Cells[row, 26].Value = item.Veiculos;
          workSheet.Cells[row, 27].Value = item.CicloAB;
          workSheet.Cells[row, 28].Value = item.CicloBA;
          workSheet.Cells[row, 29].Value = item.VeiculosE;
          workSheet.Cells[row, 30].Value = item.VeiculosP;
          workSheet.Cells[row, 31].Value = item.Extensao;
          workSheet.Cells[row, 32].Value = item.KmTotal;
          workSheet.Cells[row, 33].Value = item.KmTotalE;
          workSheet.Cells[row, 34].Value = item.KmTotalP;
        }

        using var memoryStream = new MemoryStream();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("content-disposition", $"attachment; filename={Guid.NewGuid().ToString()}.xlsx");
        excel.SaveAs(memoryStream);
        memoryStream.WriteTo(Response.OutputStream);
        Response.Flush();
        Response.End();
      }
      return View();
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (dimensionamento != null)) {
        dimensionamento.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
