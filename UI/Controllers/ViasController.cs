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
  public class ViasController : Controller {
    private readonly Services<Via> vias = new Services<Via>();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<ViaViewModel, Via>().ReverseMap();
                                          }).CreateMapper();

    // GET: Vias
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<ViaViewModel>>(await vias.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Vias/Create
    public ActionResult Create() {
      return View();
    }

    // POST: Vias/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ViaViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          Via via = mapper.Map<Via>(viewModel);
          await vias.Insert(via);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Vias/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Via via = await vias.GetByIdAsync(id);
      if (via == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ViaViewModel>(via);
      return View(viewModel);
    }

    // POST: Vias/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(ViaViewModel viewModel) {
      try {
        if (ModelState.IsValid) {
          Via via = mapper.Map<Via>(viewModel);
          await vias.Update(via);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Vias/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Via via = await vias.GetByIdAsync(id);
      if (via == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ViaViewModel>(via);
      return View(viewModel);
    }

    // POST: Vias/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Via via = await vias.GetByIdAsync(id);
      if (via != null) {
        await vias.Delete(via);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (vias != null)) {
        vias.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
