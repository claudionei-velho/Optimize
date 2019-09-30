using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class TCategoriasController : Controller {
    private TCategoriaService tCategorias = new TCategoriaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<TCategoriaViewModel, TCategoria>().ReverseMap();
                                          }).CreateMapper();

    // GET: TCategorias
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.tCategorias = new TCategoriaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<TCategoriaViewModel>>(await tCategorias.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: TCategorias/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      TCategoria tcategoria = await tCategorias.GetFirstAsync(c => c.Id == id);
      if (tcategoria == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TCategoriaViewModel>(tcategoria);
      return View(viewModel);
    }

    // GET: TCategorias/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(empresas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name");
      }
      return View();
    }

    // POST: TCategorias/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(TCategoriaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }

      try {
        if (ModelState.IsValid) {
          if (viewModel.Gratuidade) {
            viewModel.Rateio = null;
          }
          else {
            if (!viewModel.Rateio.HasValue) {
              viewModel.Rateio = 100;
            }
          }
          TCategoria tcategoria = mapper.Map<TCategoria>(viewModel);
          await tCategorias.Insert(tcategoria);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: TCategorias/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      TCategoria tcategoria = await tCategorias.GetByIdAsync(id);
      if (tcategoria == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TCategoriaViewModel>(tcategoria);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }
      return View(viewModel);
    }

    // POST: TCategorias/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(TCategoriaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EmpresaService empresas = new EmpresaService(user.ID)) {
        ViewBag.EmpresaId = new SelectList(await empresas.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Fantasia }), "Id", "Name", viewModel.EmpresaId);
      }

      try {
        if (ModelState.IsValid) {
          if (viewModel.Gratuidade) {
            viewModel.Rateio = null;
          }
          else {
            if (!viewModel.Rateio.HasValue) {
              viewModel.Rateio = 100;
            }
          }
          TCategoria tcategoria = mapper.Map<TCategoria>(viewModel);
          await tCategorias.Update(tcategoria);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: TCategorias/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      TCategoria tcategoria = await tCategorias.GetFirstAsync(c => c.Id == id);
      if (tcategoria == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TCategoriaViewModel>(tcategoria);
      return View(viewModel);
    }

    // POST: TCategorias/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      TCategoria tcategoria = await tCategorias.GetByIdAsync(id);
      if (tcategoria != null) {
        await tCategorias.Delete(tcategoria);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (tCategorias != null)) {
        tCategorias.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
