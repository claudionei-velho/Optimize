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
  public class TotalViagensController : Controller {
    private TotalViagemService totalViagens = new TotalViagemService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<TotalViagemViewModel, TotalViagem>().ReverseMap();
                                          }).CreateMapper();

    // GET: TotalViagens
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.totalViagens = new TotalViagemService(user.ID);

      var viewModel = mapper.Map<IEnumerable<TotalViagemViewModel>>(await totalViagens.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    public async Task<ActionResult> Details(int? id, int? did, int? pid, string go) {
      if ((id == null) || (did == null) || (pid == null) || (go == null)) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      TotalViagem totalViagem = await totalViagens.GetFirstAsync(
          d => d.LinhaId == id && d.DiaId == did && d.PeriodoId == pid && d.Sentido.Equals(go));
      if (totalViagem == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TotalViagemViewModel>(totalViagem);
      return View(viewModel);
    }

    public ActionResult Export() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.totalViagens = new TotalViagemService(user.ID);

      using (ExcelPackage excel = new ExcelPackage()) {
        var workSheet = excel.Workbook.Worksheets.Add("Plan1");

        // Header Section
        int row = 1;
        workSheet.Cells[row, 1].Value = Resources.EmpresaId;
        workSheet.Cells[row, 2].Value = Resources.LinhaId;
        workSheet.Cells[row, 3].Value = Resources.DiaId;
        workSheet.Cells[row, 4].Value = Resources.PeriodoId;
        workSheet.Cells[row, 5].Value = Resources.Sentido;
        workSheet.Cells[row, 6].Value = Resources.HoraInicio;
        workSheet.Cells[row, 7].Value = Resources.HoraTermino;
        workSheet.Cells[row, 8].Value = Resources.Duracao;
        workSheet.Cells[row, 9].Value = Resources.Ciclo;
        workSheet.Cells[row, 10].Value = Resources.TotalViagens;
        workSheet.Cells[row, 11].Value = Resources.TotalAtendimentos;
        workSheet.Cells[row, 12].Value = Resources.Intervalo;
        workSheet.Cells[row, 13].Value = Resources.Veiculos;
        workSheet.Cells[row, 14].Value = Resources.KmDia;
        workSheet.Cells[row, 15].Value = Resources.KmSemana;
        workSheet.Cells[row, 16].Value = Resources.KmMes;

        // Detail Section
        Workday workDay = new Workday();
        foreach (TotalViagem item in totalViagens.GetQuery()) {
          workSheet.Cells[++row, 1].Value = item.Linha.Empresa.Fantasia; 
          workSheet.Cells[row, 2].Value = item.Linha.Denominacao;
          workSheet.Cells[row, 3].Value = workDay.Data[item.DiaId];
          workSheet.Cells[row, 4].Value = item.PrLinha.EPeriodo.Denominacao;
          workSheet.Cells[row, 5].Value = item.Sentido;
          workSheet.Cells[row, 6].Value = string.Format("{0:t}", item.Inicio);
          workSheet.Cells[row, 7].Value = string.Format("{0:t}", item.Termino);
          workSheet.Cells[row, 8].Value = item.Duracao;
          workSheet.Cells[row, 9].Value = item.Ciclo;
          workSheet.Cells[row, 10].Value = item.QtdViagens;
          workSheet.Cells[row, 11].Value = item.QtdAtendimentos;
          workSheet.Cells[row, 12].Value = item.IntervaloP;
          workSheet.Cells[row, 13].Value = item.VeiculosP;
          workSheet.Cells[row, 14].Value = item.KmDia;
          workSheet.Cells[row, 15].Value = item.KmSemana;
          workSheet.Cells[row, 16].Value = item.KmMes;
        }

        using (var memoryStream = new MemoryStream()) {
          Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
          Response.AddHeader("content-disposition", $"attachment; filename={Guid.NewGuid().ToString()}.xlsx");
          excel.SaveAs(memoryStream);
          memoryStream.WriteTo(Response.OutputStream);
          Response.Flush();
          Response.End();
        }
      }
      return View();
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (totalViagens != null)) {
        totalViagens.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
