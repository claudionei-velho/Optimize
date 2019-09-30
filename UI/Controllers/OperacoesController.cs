using System.Collections.Generic;
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
  public class OperacoesController : Controller {
    private OperacaoService operacoes = new OperacaoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<OperacaoViewModel, Operacao>().ReverseMap();
                                          }).CreateMapper();

    // GET: Operacoes
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.operacoes = new OperacaoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<OperacaoViewModel>>(await operacoes.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Operacoes/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Operacao operacao = await operacoes.GetFirstAsync(p => p.Id == id);
      if (operacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<OperacaoViewModel>(operacao);
      return View(viewModel);
    }

    // GET: Operacoes/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      using (Services<OperLinha> operLinhas = new Services<OperLinha>()) {
        ViewBag.OperLinhaId = new SelectList(operLinhas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      return View();
    }

    // POST: Operacoes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(OperacaoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<OperLinha> operLinhas = new Services<OperLinha>()) {
        ViewBag.OperLinhaId = new SelectList(await operLinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.OperLinhaId);
      }

      try {
        if (ModelState.IsValid) {
          Operacao operacao = mapper.Map<Operacao>(viewModel);
          await operacoes.Insert(operacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Operacoes/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Operacao operacao = await operacoes.GetByIdAsync(id);
      if (operacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<OperacaoViewModel>(operacao);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<OperLinha> operLinhas = new Services<OperLinha>()) {
        ViewBag.OperLinhaId = new SelectList(await operLinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.OperLinhaId);
      }
      return View(viewModel);
    }

    // POST: Operacoes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(OperacaoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<OperLinha> operLinhas = new Services<OperLinha>()) {
        ViewBag.OperLinhaId = new SelectList(await operLinhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.OperLinhaId);
      }

      try {
        if (ModelState.IsValid) {
          Operacao operacao = mapper.Map<Operacao>(viewModel);
          await operacoes.Update(operacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Operacoes/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Operacao operacao = await operacoes.GetFirstAsync(p => p.Id == id);
      if (operacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<OperacaoViewModel>(operacao);
      return View(viewModel);
    }

    // POST: Operacoes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Operacao operacao = await operacoes.GetByIdAsync(id);
      if (operacao != null) {
        await operacoes.Delete(operacao);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (operacoes != null)) {
        operacoes.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
