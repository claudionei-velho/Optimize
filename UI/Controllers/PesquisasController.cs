using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class PesquisasController : Controller {
    private PesquisaService pesquisas = new PesquisaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
      cfg.CreateMap<PesquisaViewModel, Pesquisa>().ReverseMap();
    }).CreateMapper();

    // GET: Pesquisas
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.pesquisas = new PesquisaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<PesquisaViewModel>>(await pesquisas.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Pesquisas/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Pesquisa pesquisa = await pesquisas.GetFirstAsync(p => p.Id == id);
      if (pesquisa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PesquisaViewModel>(pesquisa);
      return View(viewModel);
    }

    // GET: Pesquisas/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(terminais.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(troncos.GetSelect(
            q => new {
              Id = q.Id.ToString(),
              Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name");
      }
      using (CorredorService corredores = new CorredorService(user.ID)) {
        ViewBag.CorredorId = new SelectList(corredores.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      using (OperacaoService operacoes = new OperacaoService(user.ID)) {
        ViewBag.OperacaoId = new SelectList(operacoes.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.OperLinha.Denominacao }), "Id", "Name");
      }
      using (CLinhaService cLinhas = new CLinhaService(user.ID)) {
        ViewBag.Classificacao = new SelectList(cLinhas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.ClassLinha.Denominacao }), "Id", "Name");
      }
      return View();
    }

    // POST: Pesquisas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(PesquisaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(await terminais.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.TerminalId);
      }
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(),
              Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.TroncoId);
      }
      using (CorredorService corredores = new CorredorService(user.ID)) {
        ViewBag.CorredorId = new SelectList(await corredores.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.CorredorId);
      }
      using (OperacaoService operacoes = new OperacaoService(user.ID)) {
        ViewBag.OperacaoId = new SelectList(await operacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.OperLinha.Denominacao }), "Id", "Name", viewModel.OperacaoId);
      }
      using (CLinhaService cLinhas = new CLinhaService(user.ID)) {
        ViewBag.Classificacao = new SelectList(await cLinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.ClassLinha.Denominacao }), "Id", "Name", viewModel.Classificacao);
      }

      try {
        if (ModelState.IsValid) {
          Pesquisa pesquisa = mapper.Map<Pesquisa>(viewModel);
          await pesquisas.Insert(pesquisa);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Pesquisas/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Pesquisa pesquisa = await pesquisas.GetByIdAsync(id);
      if (pesquisa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PesquisaViewModel>(pesquisa);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(await terminais.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.TerminalId);
      }
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.TroncoId);
      }
      using (CorredorService corredores = new CorredorService(user.ID)) {
        ViewBag.CorredorId = new SelectList(await corredores.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.CorredorId);
      }
      using (OperacaoService operacoes = new OperacaoService(user.ID)) {
        ViewBag.OperacaoId = new SelectList(await operacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.OperLinha.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.OperacaoId);
      }
      using (CLinhaService cLinhas = new CLinhaService(user.ID)) {
        ViewBag.Classificacao = new SelectList(await cLinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.ClassLinha.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.Classificacao);
      }
      return View(viewModel);
    }

    // POST: Pesquisas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(PesquisaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(await terminais.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.TerminalId);
      }
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.TroncoId);
      }
      using (CorredorService corredores = new CorredorService(user.ID)) {
        ViewBag.CorredorId = new SelectList(await corredores.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.CorredorId);
      }
      using (OperacaoService operacoes = new OperacaoService(user.ID)) {
        ViewBag.OperacaoId = new SelectList(await operacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.OperLinha.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.OperacaoId);
      }
      using (CLinhaService cLinhas = new CLinhaService(user.ID)) {
        ViewBag.Classificacao = new SelectList(await cLinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.ClassLinha.Denominacao },
            q => q.EmpresaId == viewModel.EmpresaId), "Id", "Name", viewModel.Classificacao);
      }

      try {
        if (ModelState.IsValid) {
          Pesquisa pesquisa = mapper.Map<Pesquisa>(viewModel);
          await pesquisas.Update(pesquisa);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Pesquisas/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Pesquisa pesquisa = await pesquisas.GetFirstAsync(p => p.Id == id);
      if (pesquisa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PesquisaViewModel>(pesquisa);
      return View(viewModel);
    }

    // POST: Pesquisas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Pesquisa pesquisa = await pesquisas.GetByIdAsync(id);
      if (pesquisa != null) {
        await pesquisas.Delete(pesquisa);
      }
      return RedirectToAction(nameof(Index));
    }

    public JsonResult GetTerminais(int id) {
      using Services<Terminal> terminais = new Services<Terminal>();
      return Json(terminais.GetQuery(q => q.EmpresaId == id).Select(p => new { p.Id, p.Denominacao })
                      .ToDictionary(k => k.Id, k => k.Denominacao), JsonRequestBehavior.AllowGet);
    }

    public JsonResult GetTroncos(int id) {
      using Services<Tronco> troncos = new Services<Tronco>();
      return Json(troncos.GetQuery(q => q.EmpresaId == id)
                      .Select(p => new { p.Id, p.Prefixo, p.Denominacao })
                      .ToDictionary(k => k.Id, k => $"{k.Prefixo} | {k.Denominacao}"), JsonRequestBehavior.AllowGet);
    }

    public JsonResult GetCorredores(int id) {
      using Services<Corredor> corredores = new Services<Corredor>();
      return Json(corredores.GetQuery(q => q.EmpresaId == id)
                      .Select(p => new { p.Id, p.Prefixo, p.Denominacao })
                      .ToDictionary(k => k.Id, k => $"{k.Prefixo} | {k.Denominacao}"), JsonRequestBehavior.AllowGet);
    }

    public JsonResult GetOperacoes(int id) {
      using OperacaoService operacoes = new OperacaoService();
      return Json(operacoes.GetQuery(q => q.EmpresaId == id).Select(p => new { p.Id, p.OperLinha.Denominacao })
                      .ToDictionary(k => k.Id, k => k.Denominacao), JsonRequestBehavior.AllowGet);
    }

    public JsonResult GetClasses(int id) {
      using CLinhaService cLinhas = new CLinhaService();
      return Json(cLinhas.GetQuery(q => q.EmpresaId == id).Select(p => new { p.Id, p.ClassLinha.Denominacao })
                      .ToDictionary(k => k.Id, k => k.Denominacao), JsonRequestBehavior.AllowGet);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (pesquisas != null)) {
        pesquisas.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
