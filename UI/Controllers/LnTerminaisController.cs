using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Bll.Lists;
using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class LnTerminaisController : Controller {
    private LnTerminalService lTerminais = new LnTerminalService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<LnTerminalViewModel, LnTerminal>().ReverseMap();
                                          }).CreateMapper();

    // GET: LnTerminais
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.lTerminais = new LnTerminalService(user.ID);

      var viewModel = mapper.Map<IEnumerable<LnTerminalViewModel>>(await lTerminais.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: LnTerminais
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<LnTerminalViewModel>>(
                          await lTerminais.GetAllAsync(q => q.TerminalId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: LnTerminais/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnTerminal lTerminal = await lTerminais.GetFirstAsync(l => l.Id == id);
      if (lTerminal == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnTerminalViewModel>(lTerminal);
      return View(viewModel);
    }

    // GET: LnTerminais/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(terminais.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo }), "Id", "Name");
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name");
      }
      ViewBag.UteisFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name");
      ViewBag.SabadosFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name");
      ViewBag.DomingosFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name");

      return View();
    }

    // POST: LnTerminais/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(LnTerminalViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(await terminais.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo }), "Id", "Name", viewModel.TerminalId);
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.UteisFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.UteisFluxo);
      ViewBag.SabadosFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.SabadosFluxo);
      ViewBag.DomingosFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.DomingosFluxo);

      try {
        if (ModelState.IsValid) {
          LnTerminal lTerminal = mapper.Map<LnTerminal>(viewModel);
          await lTerminais.Insert(lTerminal);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: LnTerminais/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnTerminal lTerminal = await lTerminais.GetByIdAsync(id);
      if (lTerminal == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnTerminalViewModel>(lTerminal);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(await terminais.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo }), "Id", "Name", viewModel.TerminalId);
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.UteisFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.UteisFluxo);
      ViewBag.SabadosFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.SabadosFluxo);
      ViewBag.DomingosFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.DomingosFluxo);

      return View(viewModel);
    }

    // POST: LnTerminais/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(LnTerminalViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;

      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(await terminais.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo }), "Id", "Name", viewModel.TerminalId);
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        int empresaId;
        using (Services<Terminal> terminais = new Services<Terminal>()) {
          empresaId = terminais.GetById(viewModel.TerminalId).EmpresaId;
        }
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao },
            q => q.EmpresaId == empresaId), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.UteisFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.UteisFluxo);
      ViewBag.SabadosFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.SabadosFluxo);
      ViewBag.DomingosFluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.DomingosFluxo);

      try {
        if (ModelState.IsValid) {
          LnTerminal lTerminal = mapper.Map<LnTerminal>(viewModel);
          await lTerminais.Update(lTerminal);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: LnTerminais/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnTerminal lTerminal = await lTerminais.GetFirstAsync(l => l.Id == id);
      if (lTerminal == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnTerminalViewModel>(lTerminal);
      return View(viewModel);
    }

    // POST: LnTerminais/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      LnTerminal lTerminal = await lTerminais.GetByIdAsync(id);
      if (lTerminal != null) {
        await lTerminais.Delete(lTerminal);
      }
      return RedirectToAction(nameof(Index));
    }

    public JsonResult GetLinhas(int id) {      
      HashSet<SelectBox> result = new HashSet<SelectBox>();
      using (Services<Terminal> terminais = new Services<Terminal>()) {
        int empresaId = terminais.GetById(id).EmpresaId;

        using (Services<Linha> linhas = new Services<Linha>()) {
          foreach (Linha item in linhas.GetQuery(q => q.EmpresaId == empresaId)) {
            result.Add(new SelectBox() {
              Id = item.Id.ToString(),
              Name = item.Prefixo + " | " + item.Denominacao
            });
          }
        }
      }
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (lTerminais != null)) {
        lTerminais.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
