using System;
using System.Collections.Generic;
using System.IO;
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
  public class SinoticosController : Controller {
    private SinoticoService qSinotico = new SinoticoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<SinoticoViewModel, Sinotico>().ReverseMap();
                                          }).CreateMapper();

    // GET: Sinoticos
    public ActionResult Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.qSinotico = new SinoticoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<SinoticoViewModel>>(qSinotico.GetAll());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    public ActionResult Export() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.qSinotico = new SinoticoService(user.ID);

      using (ExcelPackage excel = new ExcelPackage()) {
        var workSheet = excel.Workbook.Worksheets.Add("Plan1");

        // Header Section
        int row = 1;
        workSheet.Cells[row, 1].Value = Resources.LinhaId;
        workSheet.Cells[row, 2].Value = Resources.DiaId;
        workSheet.Cells[row, 3].Value = Resources.SinoticoId;
        workSheet.Cells[row, 4].Value = Resources.Unidade;
        workSheet.Cells[row, 5].Value = Resources.IndiceAtual;
        workSheet.Cells[row, 6].Value = Resources.DimensionaE;
        workSheet.Cells[row, 7].Value = Resources.EvolucaoE;
        workSheet.Cells[row, 8].Value = Resources.DimensionaP;
        workSheet.Cells[row, 9].Value = Resources.EvolucaoP;

        // Detail Section
        foreach (Sinotico item in qSinotico.GetQuery()) {
          workSheet.Cells[++row, 1].Value = item.Linha.Denominacao;
          workSheet.Cells[row, 2].Value = Workday.Data[item.DiaId];
          workSheet.Cells[row, 3].Value = item.ISinotico.Denominacao;
          workSheet.Cells[row, 4].Value = item.ISinotico.Unidade;
          workSheet.Cells[row, 5].Value = item.IndiceAtual;
          workSheet.Cells[row, 6].Value = item.DimensionaE;
          workSheet.Cells[row, 7].Value = item.EvolucaoE;
          workSheet.Cells[row, 8].Value = item.DimensionaP;
          workSheet.Cells[row, 9].Value = item.EvolucaoP;
        }

        using var memoryStream = new MemoryStream();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("content-disposition", $"attachment; filename={Guid.NewGuid()}.xlsx");
        excel.SaveAs(memoryStream);
        memoryStream.WriteTo(Response.OutputStream);
        Response.Flush();
        Response.End();
      }
      return View();
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (qSinotico != null)) {
        qSinotico.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
