using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll.Services;
using Dto.Lists;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class ReferenciasController : Controller {
    private readonly ReferenciaService referencias = new ReferenciaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                              cfg.CreateMap<ReferenciaViewModel, Referencia>().ReverseMap();
                                          }).CreateMapper();

    // GET: Referencias
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<ReferenciaViewModel>>(await referencias.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Referencias
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<ReferenciaViewModel>>(
                          await referencias.GetAllAsync(q => q.LinhaId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: Referencias/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Referencia referencia = await referencias.GetFirstAsync(i => i.Id == id);
      if (referencia == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ReferenciaViewModel>(referencia);
      return View(viewModel);
    }

    // GET: Referencias/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao }), "Id", "Name");
      }
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(atendimentos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao }), "Id", "Name");
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value");
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PInicioId = new SelectList(pontos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao }), "Id", "Name");
        ViewBag.PTerminoId = new SelectList(pontos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao }), "Id", "Name");
      }
      return View();
    }

    // POST: Referencias/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ReferenciaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao }
        ), "Id", "Name", viewModel.LinhaId);
      }
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(atendimentos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao }
        ), "Id", "Name", viewModel.AtendimentoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PInicioId = new SelectList(pontos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao }
        ), "Id", "Name", viewModel.PInicioId);
        ViewBag.PTerminoId = new SelectList(pontos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao }
        ), "Id", "Name", viewModel.PTerminoId);
      }

      try {
        if (ModelState.IsValid) {
          Referencia referencia = mapper.Map<Referencia>(viewModel);
          await referencias.Insert(referencia);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Referencias/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Referencia referencia = await referencias.GetByIdAsync(id);
      if (referencia == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ReferenciaViewModel>(referencia);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao }
        ), "Id", "Name", viewModel.LinhaId);
      }
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(atendimentos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao }
        ), "Id", "Name", viewModel.AtendimentoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PInicioId = new SelectList(pontos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao }
        ), "Id", "Name", viewModel.PInicioId);
        ViewBag.PTerminoId = new SelectList(pontos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao }
        ), "Id", "Name", viewModel.PTerminoId);
      }
      return View(viewModel);
    }

    // POST: Referencias/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(ReferenciaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao }
        ), "Id", "Name", viewModel.LinhaId);
      }
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(atendimentos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao }
        ), "Id", "Name", viewModel.AtendimentoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PInicioId = new SelectList(pontos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao }
        ), "Id", "Name", viewModel.PInicioId);
        ViewBag.PTerminoId = new SelectList(pontos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao }
        ), "Id", "Name", viewModel.PTerminoId);
      }

      try {
        if (ModelState.IsValid) {
          Referencia referencia = mapper.Map<Referencia>(viewModel);
          await referencias.Update(referencia);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Referencias/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Referencia referencia = await referencias.GetFirstAsync(i => i.Id == id);
      if (referencia == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ReferenciaViewModel>(referencia);
      return View(viewModel);
    }

    // POST: Referencias/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Referencia referencia = await referencias.GetByIdAsync(id);
      if (referencia != null) {
        await referencias.Delete(referencia);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (referencias != null)) {
        referencias.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
