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
  public class PontosController : Controller {
    private PontoService pontos = new PontoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<PontoViewModel, Ponto>().ReverseMap();
                                          }).CreateMapper();

    // GET: Pontos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.pontos = new PontoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<PontoViewModel>>(await pontos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Pontos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Ponto ponto = await pontos.GetFirstAsync(p => p.Id == id);
      if (ponto == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PontoViewModel>(ponto);
      return View(viewModel);
    }

    // GET: Pontos/Create
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

    // POST: Pontos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(PontoViewModel viewModel) {
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
          Ponto ponto = mapper.Map<Ponto>(viewModel);
          await pontos.Insert(ponto);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Pontos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Ponto ponto = await pontos.GetByIdAsync(id);
      if (ponto == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PontoViewModel>(ponto);

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

    // POST: Pontos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(PontoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService()) {
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
          Ponto ponto = mapper.Map<Ponto>(viewModel);
          await pontos.Update(ponto);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Pontos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Ponto ponto = await pontos.GetFirstAsync(p => p.Id == id);
      if (ponto == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PontoViewModel>(ponto);
      return View(viewModel);
    }

    // POST: Pontos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Ponto ponto = await pontos.GetByIdAsync(id);
      if (ponto != null) {
        await pontos.Delete(ponto);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (pontos != null)) {
        pontos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
