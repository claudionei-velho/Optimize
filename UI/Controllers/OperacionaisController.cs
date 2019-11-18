using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using OfficeOpenXml;
using PagedList;

using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Properties;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class OperacionaisController : Controller {
    private OperacionalService operacionais = new OperacionalService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<OperacionalViewModel, Operacional>().ReverseMap();
                                          }).CreateMapper();

    // GET: Operacionais
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.operacionais = new OperacionalService(user.ID);

      var viewModel = mapper.Map<IEnumerable<OperacionalViewModel>>(await operacionais.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    public ActionResult Export() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.operacionais = new OperacionalService(user.ID);

      using (ExcelPackage excel = new ExcelPackage()) {
        var workSheet = excel.Workbook.Worksheets.Add("Plan1");

        // Header Section
        int row = 1;
        workSheet.Cells[row, 1].Value = Resources.EmpresaId;
        workSheet.Cells[row, 2].Value = Resources.Prefixo;
        workSheet.Cells[row, 3].Value = Resources.Denominacao;
        workSheet.Cells[row, 4].Value = Resources.Sentido;
        workSheet.Cells[row, 5].Value = Resources.DiaId;
        workSheet.Cells[row, 6].Value = Resources.Funcao;
        workSheet.Cells[row, 7].Value = Resources.Extensao;
        workSheet.Cells[row, 8].Value = Resources.ViagensUtil;
        workSheet.Cells[row, 9].Value = Resources.PercursoUtil;
        workSheet.Cells[row, 10].Value = Resources.InicioUtil;
        workSheet.Cells[row, 11].Value = Resources.ViagensSab;
        workSheet.Cells[row, 12].Value = Resources.PercursoSab;
        workSheet.Cells[row, 13].Value = Resources.InicioSab;
        workSheet.Cells[row, 14].Value = Resources.ViagensDom;
        workSheet.Cells[row, 15].Value = Resources.PercursoDom;
        workSheet.Cells[row, 16].Value = Resources.InicioDom;

        // Detail Section
        foreach (Operacional item in operacionais.GetQuery()) {
          workSheet.Cells[++row, 1].Value = item.Linha.Empresa.Fantasia;
          workSheet.Cells[row, 2].Value = item.Prefixo;
          workSheet.Cells[row, 3].Value = item.Denominacao;
          workSheet.Cells[row, 4].Value = item.Sentido;
          workSheet.Cells[row, 5].Value = item.DiaOperacao;
          workSheet.Cells[row, 6].Value = item.Funcao;
          workSheet.Cells[row, 7].Value = item.Extensao;
          workSheet.Cells[row, 8].Value = item.ViagensUtil;
          workSheet.Cells[row, 9].Value = item.PercursoUtil;
          workSheet.Cells[row, 10].Value = item.InicioUtil;
          workSheet.Cells[row, 11].Value = item.ViagensSab;
          workSheet.Cells[row, 12].Value = item.PercursoSab;
          workSheet.Cells[row, 13].Value = item.InicioSab;
          workSheet.Cells[row, 14].Value = item.ViagensDom;
          workSheet.Cells[row, 15].Value = item.PercursoDom;
          workSheet.Cells[row, 16].Value = item.InicioDom;
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
      if (disposing && (operacionais != null)) {
        operacionais.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
