using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;
using OfficeOpenXml;

using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Properties;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class FrotaEtariasController : Controller {
    private FrotaEtariaService fxEtarias = new FrotaEtariaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<FrotaEtariaViewModel, FrotaEtaria>().ReverseMap();
                                          }).CreateMapper();

    // GET: FrotaEtarias
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.fxEtarias = new FrotaEtariaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<FrotaEtariaViewModel>>(await fxEtarias.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: FrotaEtarias/Details/5
    public async Task<ActionResult> Details(int? id, int? pid) {
      if ((id == null) || (pid == null)) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      FrotaEtaria frotaEtaria = await fxEtarias.GetFirstAsync(f => f.EmpresaId == id && f.EtariaId == pid);
      if (frotaEtaria == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<FrotaEtariaViewModel>(frotaEtaria);

      return View(viewModel);
    }

    public ActionResult Export() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.fxEtarias = new FrotaEtariaService(user.ID);

      using (ExcelPackage excel = new ExcelPackage()) {
        var workSheet = excel.Workbook.Worksheets.Add("Plan1");

        // Header Section
        int row = 1;
        workSheet.Cells[row, 1].Value = Resources.EmpresaId;
        workSheet.Cells[row, 2].Value = Resources.EtariaId;
        workSheet.Cells[row, 3].Value = Resources.Years;
        workSheet.Cells[row, 4].Value = Resources.Micro;
        workSheet.Cells[row, 5].Value = Resources.Mini;
        workSheet.Cells[row, 6].Value = Resources.Midi;
        workSheet.Cells[row, 7].Value = Resources.Basico;
        workSheet.Cells[row, 8].Value = Resources.Padron;
        workSheet.Cells[row, 9].Value = Resources.Especial;
        workSheet.Cells[row, 10].Value = Resources.Articulado;
        workSheet.Cells[row, 11].Value = Resources.BiArticulado;
        workSheet.Cells[row, 12].Value = Resources.Frota;
        workSheet.Cells[row, 13].Value = Resources.EqvIdade;

        // Detail Section
        foreach (FrotaEtaria item in fxEtarias.GetQuery()) {
          workSheet.Cells[++row, 1].Value = item.Empresa.Fantasia;
          workSheet.Cells[row, 2].Value = item.FxEtaria.Denominacao;
          workSheet.Cells[row, 3].Value = item.FxEtaria.Minimo;
          workSheet.Cells[row, 4].Value = item.Micro;
          workSheet.Cells[row, 5].Value = item.Mini;
          workSheet.Cells[row, 6].Value = item.Midi;
          workSheet.Cells[row, 7].Value = item.Basico;
          workSheet.Cells[row, 8].Value = item.Padron;
          workSheet.Cells[row, 9].Value = item.Especial;
          workSheet.Cells[row, 10].Value = item.Articulado;
          workSheet.Cells[row, 11].Value = item.BiArticulado;
          workSheet.Cells[row, 12].Value = item.Frota;
          workSheet.Cells[row, 13].Value = item.EqvIdade;
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
      if (disposing && (fxEtarias != null)) {
        fxEtarias.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
