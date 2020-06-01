using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;
using MigraDoc.Rendering;

using Bll;
using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Reports.Docs;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class LinhasController : Controller {    
    private LinhaService linhas = new LinhaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<LinhaViewModel, Linha>().ReverseMap();
                                          }).CreateMapper();

    // GET: Linhas
    public async Task<ActionResult> Index(string currentFilter, string search, int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.linhas = new LinhaService(user.ID);

      ViewBag.CurrentFilter = currentFilter;
      ViewBag.SearchString = search;

      IEnumerable<Linha> source = await linhas.GetAllAsync();
      if (!string.IsNullOrWhiteSpace(search)) {
        source = (int.Parse(currentFilter)) switch {
          1 => await linhas.GetAllAsync(q => q.Prefixo.Contains(search)),
          _ => await linhas.GetAllAsync(q => q.Denominacao.Contains(search)),
        };
      }
      var viewModel = mapper.Map<IEnumerable<LinhaViewModel>>(source);
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Linhas/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Linha linha = await linhas.GetFirstAsync(l => l.Id == id);
      if (linha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LinhaViewModel>(linha);
      return View(viewModel);
    }

    // GET: Linhas/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      using (Services<EDominio> dominios = new Services<EDominio>()) {
        ViewBag.DominioId = new SelectList(dominios.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Dominio.Denominacao }), "Id", "Name");
      }
      using (Services<Operacao> operacoes = new Services<Operacao>()) {
        ViewBag.OperacaoId = new SelectList(operacoes.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.OperLinha.Denominacao }), "Id", "Name");
      }
      using (Services<CLinha> clinhas = new Services<CLinha>()) {
        ViewBag.Classificacao = new SelectList(clinhas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.ClassLinha.Denominacao }), "Id", "Name");
      }
      return View();
    }

    // POST: Linhas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(LinhaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<EDominio> dominios = new Services<EDominio>()) {
        ViewBag.DominioId = new SelectList(await dominios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Dominio.Denominacao }), "Id", "Name", viewModel.DominioId);
      }
      using (Services<Operacao> operacoes = new Services<Operacao>()) {
        ViewBag.OperacaoId = new SelectList(await operacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.OperLinha.Denominacao }), "Id", "Name", viewModel.OperacaoId);
      }
      using (Services<CLinha> clinhas = new Services<CLinha>()) {
        ViewBag.Classificacao = new SelectList(await clinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.ClassLinha.Denominacao }), "Id", "Name", viewModel.Classificacao);
      }

      try {
        if (ModelState.IsValid) {
          Linha linha = mapper.Map<Linha>(viewModel);
          await linhas.Insert(linha);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Linhas/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Linha linha = await linhas.GetByIdAsync(id);
      if (linha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LinhaViewModel>(linha);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<EDominio> dominios = new Services<EDominio>()) {
        ViewBag.DominioId = new SelectList(await dominios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Dominio.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.DominioId);
      }
      using (Services<Operacao> operacoes = new Services<Operacao>()) {
        ViewBag.OperacaoId = new SelectList(await operacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.OperLinha.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.OperacaoId);
      }
      using (Services<CLinha> clinhas = new Services<CLinha>()) {
        ViewBag.Classificacao = new SelectList(await clinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.ClassLinha.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.Classificacao);
      }
      return View(viewModel);
    }

    // POST: Linhas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(LinhaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<EDominio> dominios = new Services<EDominio>()) {
        ViewBag.DominioId = new SelectList(await dominios.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Dominio.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.DominioId);
      }
      using (Services<Operacao> operacoes = new Services<Operacao>()) {
        ViewBag.OperacaoId = new SelectList(await operacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.OperLinha.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.OperacaoId);
      }
      using (Services<CLinha> clinhas = new Services<CLinha>()) {
        ViewBag.Classificacao = new SelectList(await clinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.ClassLinha.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.Classificacao);
      }

      try {
        if (ModelState.IsValid) {
          Linha linha = mapper.Map<Linha>(viewModel);
          await linhas.Update(linha);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Linhas/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Linha linha = await linhas.GetFirstAsync(l => l.Id == id);
      if (linha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LinhaViewModel>(linha);
      return View(viewModel);
    }

    // POST: Linhas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Linha linha = await linhas.GetByIdAsync(id);
      if (linha != null) {
        await linhas.Delete(linha);
      }
      return RedirectToAction(nameof(Index));
    }

    public JsonResult GetDominios(int id) {
      HashSet<SelectBox> result = new HashSet<SelectBox>();

      using (EDominioService eDominio = new EDominioService()) {
        foreach (EDominio item in eDominio.GetQuery(q => q.EmpresaId == id)) {
          result.Add(new SelectBox() { Id = item.Id.ToString(), Name = item.Dominio.Denominacao });
        }
      }
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    public JsonResult GetOperacoes(int id) {
      HashSet<SelectBox> result = new HashSet<SelectBox>();

      using (OperacaoService operacoes = new OperacaoService()) {
        foreach (Operacao item in operacoes.GetQuery(q => q.EmpresaId == id)) {
          result.Add(new SelectBox() { Id = item.Id.ToString(), Name = item.OperLinha.Denominacao });
        }
      }
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    public JsonResult GetClasses(int id) {
      HashSet<SelectBox> result = new HashSet<SelectBox>();

      using (CLinhaService cLinhas = new CLinhaService()) {
        foreach (CLinha item in cLinhas.GetQuery(q => q.EmpresaId == id)) {
          result.Add(new SelectBox() { Id = item.Id.ToString(), Name = item.ClassLinha.Denominacao });
        }
      }
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    public ActionResult PreviewFichaTecnica(int? id) {
      Expression<Func<Linha, bool>> filter = q => (q.EmpresaId == 18) && (q.EDominio.DominioId == 1);
      if (id.HasValue) {
        filter = q => q.Id == id.Value;
      }

      PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer {
        Document = new FichaTecnicaReport(filter).CreateDocument()
      };

      string fileName = $"{Path.GetTempPath()}{Guid.NewGuid()}.pdf";
      pdfRenderer.RenderDocument();
      pdfRenderer.PdfDocument.Save(fileName);

      return File(fileName, "application/pdf");
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (linhas != null)) {
        linhas.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
