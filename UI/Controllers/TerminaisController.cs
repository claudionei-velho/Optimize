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
  public class TerminaisController : Controller {    
    private TerminalService terminais = new TerminalService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<TerminalViewModel, Terminal>().ReverseMap();
                                          }).CreateMapper();

    // GET: Terminais
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.terminais = new TerminalService(user.ID);

      var viewModel = mapper.Map<IEnumerable<TerminalViewModel>>(await terminais.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Terminais/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Terminal terminal = await terminais.GetFirstAsync(t => t.Id == id);
      if (terminal == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TerminalViewModel>(terminal);
      return View(viewModel);
    }

    // GET: Terminais/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(ufs.GetSelect(
            q => new { Id = q.Sigla, Name = q.Estado }, 
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name");
      }
      return View();
    }

    // POST: Terminais/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(TerminalViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(await ufs.GetSelectAsync(
            q => new { Id = q.Sigla, Name = q.Estado }, 
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name", viewModel.UfId);
      }

      try {
        if (ModelState.IsValid) {
          Terminal terminal = mapper.Map<Terminal>(viewModel);
          await terminais.Insert(terminal);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Terminais/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Terminal terminal = await terminais.GetByIdAsync(id);
      if (terminal == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TerminalViewModel>(terminal);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(await ufs.GetSelectAsync(
            q => new { Id = q.Sigla, Name = q.Estado }, 
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name", viewModel.UfId);
      }
      return View(viewModel);
    }

    // POST: Terminais/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(TerminalViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      using (Services<Uf> ufs = new Services<Uf>()) {
        ViewBag.UfId = new SelectList(await ufs.GetSelectAsync(
            q => new { Id = q.Sigla, Name = q.Estado }, 
            orderBy: q => q.OrderBy(p => p.Estado)), "Id", "Name", viewModel.UfId);
      }

      try {
        if (ModelState.IsValid) {
          Terminal terminal = mapper.Map<Terminal>(viewModel);
          await terminais.Update(terminal);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Terminais/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Terminal terminal = await terminais.GetFirstAsync(t => t.Id == id);
      if (terminal == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TerminalViewModel>(terminal);
      return View(viewModel);
    }

    // POST: Terminais/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Terminal terminal = await terminais.GetByIdAsync(id);
      if (terminal != null) {
        await terminais.Delete(terminal);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (terminais != null)) {
        terminais.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
