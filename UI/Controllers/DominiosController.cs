using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Dto.Models;
using UI.Models;

namespace UI.Controllers {
  [Authorize]
  public class DominiosController : Controller {
    private readonly Services<Dominio> dominios = new Services<Dominio>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<DominioViewModel, Dominio>().ReverseMap();
                                          }).CreateMapper();

    // GET: Dominios
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<DominioViewModel>>(await dominios.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Dominios/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Dominio dominio = await dominios.GetByIdAsync(id);
      if (dominio == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<DominioViewModel>(dominio);
      return View(viewModel);
    }

    // GET: Dominios/Create
    public ActionResult Create() {
      return View();
    }

    // POST: Dominios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(DominioViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          Dominio dominio = mapper.Map<Dominio>(viewModel);
          await dominios.Insert(dominio);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Dominios/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Dominio dominio = await dominios.GetByIdAsync(id);
      if (dominio == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<DominioViewModel>(dominio);
      return View(viewModel);
    }

    // POST: Dominios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(DominioViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          Dominio dominio = mapper.Map<Dominio>(viewModel);
          await dominios.Update(dominio);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Dominios/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Dominio dominio = await dominios.GetByIdAsync(id);
      if (dominio == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<DominioViewModel>(dominio);
      return View(viewModel);
    }

    // POST: Dominios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Dominio dominio = await dominios.GetByIdAsync(id);
      if (dominio != null) {
        await dominios.Delete(dominio);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (dominios != null)) {
        dominios.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
