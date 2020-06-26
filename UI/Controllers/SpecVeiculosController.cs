using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using OfficeOpenXml;

using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Properties;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class SpecVeiculosController : Controller {
    private SpecVeiculoService veiculos = new SpecVeiculoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<SpecVeiculoViewModel, SpecVeiculo>().ReverseMap();
                                          }).CreateMapper();

    // GET: Veiculos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      SpecVeiculo veiculo = await veiculos.GetFirstAsync(v => v.Id == id);
      if (veiculo == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<SpecVeiculoViewModel>(veiculo);

      return View(viewModel);
    }

    public async Task<ActionResult> Export() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.veiculos = new SpecVeiculoService(user.ID);

      using (ExcelPackage excel = new ExcelPackage()) {
        var workSheet = excel.Workbook.Worksheets.Add("Plan1");

        // Header Section
        int row = 1;
        workSheet.Cells[row, 1].Value = Resources.Id;
        workSheet.Cells[row, 2].Value = Resources.EmpresaId;
        workSheet.Cells[row, 3].Value = Resources.Numero;
        workSheet.Cells[row, 4].Value = Resources.Cor;
        workSheet.Cells[row, 5].Value = Resources.Classe;
        workSheet.Cells[row, 6].Value = Resources.Categoria;
        workSheet.Cells[row, 7].Value = Resources.Placa;
        workSheet.Cells[row, 8].Value = Resources.Renavam;
        workSheet.Cells[row, 9].Value = Resources.Antt;
        workSheet.Cells[row, 10].Value = Resources.InicioOperacao;
        workSheet.Cells[row, 11].Value = Resources.Fabricante;
        workSheet.Cells[row, 12].Value = Resources.Modelo;
        workSheet.Cells[row, 13].Value = Resources.ChassiNo;
        workSheet.Cells[row, 14].Value = Resources.Ano;
        workSheet.Cells[row, 15].Value = Resources.Aquisicao;
        workSheet.Cells[row, 16].Value = Resources.Fornecedor;
        workSheet.Cells[row, 17].Value = Resources.NotaFiscal;
        workSheet.Cells[row, 18].Value = Resources.Valor;
        workSheet.Cells[row, 19].Value = Resources.ChaveNfe;
        workSheet.Cells[row, 20].Value = Resources.MotorId;
        workSheet.Cells[row, 21].Value = Resources.Potencia;
        workSheet.Cells[row, 22].Value = Resources.PosMotor;
        workSheet.Cells[row, 23].Value = Resources.EixosFrente;
        workSheet.Cells[row, 24].Value = Resources.EixosTras;
        workSheet.Cells[row, 25].Value = Resources.PneusFrente;
        workSheet.Cells[row, 26].Value = Resources.PneusTras;
        workSheet.Cells[row, 27].Value = Resources.TransmiteId;
        workSheet.Cells[row, 28].Value = Resources.DirecaoId;
        workSheet.Cells[row, 29].Value = Resources.Fabricante;
        workSheet.Cells[row, 30].Value = Resources.Modelo;
        workSheet.Cells[row, 31].Value = Resources.Referencia;
        workSheet.Cells[row, 32].Value = Resources.Ano;
        workSheet.Cells[row, 33].Value = Resources.Aquisicao;
        workSheet.Cells[row, 34].Value = Resources.Fornecedor;
        workSheet.Cells[row, 35].Value = Resources.NotaFiscal;
        workSheet.Cells[row, 36].Value = Resources.Valor;
        workSheet.Cells[row, 37].Value = Resources.ChaveNfe;
        workSheet.Cells[row, 38].Value = Resources.Encarrocamento;
        workSheet.Cells[row, 39].Value = Resources.QuemEncarroca;
        workSheet.Cells[row, 40].Value = Resources.NotaEncarroca;
        workSheet.Cells[row, 41].Value = Resources.ValorEncarroca;
        workSheet.Cells[row, 42].Value = Resources.Portas;
        workSheet.Cells[row, 43].Value = Resources.Assentos;
        workSheet.Cells[row, 44].Value = Resources.Capacidade;
        workSheet.Cells[row, 45].Value = Resources.Piso;
        workSheet.Cells[row, 46].Value = Resources.EscapeV;
        workSheet.Cells[row, 47].Value = Resources.EscapeH;
        workSheet.Cells[row, 48].Value = Resources.Catraca;
        workSheet.Cells[row, 49].Value = Resources.PortaIn;
        workSheet.Cells[row, 50].Value = Resources.SaidaFrente;
        workSheet.Cells[row, 51].Value = Resources.SaidaMeio;
        workSheet.Cells[row, 52].Value = Resources.SaidaTras;
        workSheet.Cells[row, 53].Value = Resources.EtariaId;
        workSheet.Cells[row, 54].Value = Resources.Years;
        workSheet.Cells[row, 55].Value = Resources.Cadastro;

        // Detail Section
        foreach (var item in mapper.Map<IEnumerable<SpecVeiculoViewModel>>(await veiculos.GetAllAsync())) {
          workSheet.Cells[++row, 1].Value = item.Id;
          workSheet.Cells[row, 2].Value = item.Empresa.Fantasia;
          workSheet.Cells[row, 3].Value = item.Numero;
          workSheet.Cells[row, 4].Value = item.Cor;
          workSheet.Cells[row, 5].Value = item.CVeiculo.Classe;
          workSheet.Cells[row, 6].Value = item.CategoriaCap;
          workSheet.Cells[row, 7].Value = item.Placa;
          workSheet.Cells[row, 8].Value = item.Renavam;
          workSheet.Cells[row, 9].Value = item.Antt;
          workSheet.Cells[row, 10].Value = string.Format("{0:d}", item.Inicio);
          workSheet.Cells[row, 11].Value = item.ChassiFabricante;
          workSheet.Cells[row, 12].Value = item.ChassiModelo;
          workSheet.Cells[row, 13].Value = item.ChassiNo;
          workSheet.Cells[row, 14].Value = item.ChassiAno;
          workSheet.Cells[row, 15].Value = string.Format("{0:d}", item.ChassiAquisicao);
          workSheet.Cells[row, 16].Value = item.ChassiFornecedor;
          workSheet.Cells[row, 17].Value = item.ChassiNota;
          workSheet.Cells[row, 18].Value = string.Format("{0:C}", item.ChassiValor);
          workSheet.Cells[row, 19].Value = item.ChassiChaveNfe;
          workSheet.Cells[row, 20].Value = (item.MotorId != null) ? item.Motor.Denominacao : null;
          workSheet.Cells[row, 21].Value = item.Potencia;
          workSheet.Cells[row, 22].Value = item.PosMotorCap;
          workSheet.Cells[row, 23].Value = item.EixosFrente;
          workSheet.Cells[row, 24].Value = item.EixosTras;
          workSheet.Cells[row, 25].Value = item.PneusFrente;
          workSheet.Cells[row, 26].Value = item.PneusTras;
          workSheet.Cells[row, 27].Value = item.TransmiteCap;
          workSheet.Cells[row, 28].Value = item.DirecaoCap;
          workSheet.Cells[row, 29].Value = item.CarroceriaFabricante;
          workSheet.Cells[row, 30].Value = item.CarroceriaModelo;
          workSheet.Cells[row, 31].Value = item.Referencia;
          workSheet.Cells[row, 32].Value = item.CarroceriaAno;
          workSheet.Cells[row, 33].Value = string.Format("{0:d}", item.CarroceriaAquisicao);
          workSheet.Cells[row, 34].Value = item.CarroceriaFornecedor;
          workSheet.Cells[row, 35].Value = item.CarroceriaNota;
          workSheet.Cells[row, 36].Value = string.Format("{0:C}", item.CarroceriaValor);
          workSheet.Cells[row, 37].Value = item.CarroceriaChaveNfe;
          workSheet.Cells[row, 38].Value = string.Format("{0:d}", item.Encarrocamento);
          workSheet.Cells[row, 39].Value = item.QuemEncarroca;
          workSheet.Cells[row, 40].Value = item.NotaEncarroca;
          workSheet.Cells[row, 41].Value = string.Format("{0:C}", item.ValorEncarroca);
          workSheet.Cells[row, 42].Value = item.Portas;
          workSheet.Cells[row, 43].Value = item.Assentos;
          workSheet.Cells[row, 44].Value = item.Capacidade;
          workSheet.Cells[row, 45].Value = item.Piso;
          workSheet.Cells[row, 46].Value = item.EscapeV;
          workSheet.Cells[row, 47].Value = item.EscapeH;
          workSheet.Cells[row, 48].Value = item.CatracaCap;
          workSheet.Cells[row, 49].Value = item.PortaInCap;
          workSheet.Cells[row, 50].Value = item.SaidaFrente;
          workSheet.Cells[row, 51].Value = item.SaidaMeio;
          workSheet.Cells[row, 52].Value = item.SaidaTras;
          workSheet.Cells[row, 53].Value = (item.EtariaId != null) ? item.FxEtaria.Denominacao : null;
          workSheet.Cells[row, 54].Value = item.Idade;
          workSheet.Cells[row, 55].Value = string.Format("{0:G}", item.Cadastro);
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
      if (disposing && (veiculos != null)) {
        veiculos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
