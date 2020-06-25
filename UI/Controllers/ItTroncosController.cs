using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Bll.Services;
using Dto.Lists;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class ItTroncosController : Controller {
    private ItTroncoService itTroncos = new ItTroncoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<ItTroncoViewModel, ItTronco>().ReverseMap();
                                          }).CreateMapper();

    // GET: ItTroncos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.itTroncos = new ItTroncoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<ItTroncoViewModel>>(await itTroncos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: ItTroncos
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<ItTroncoViewModel>>(
                          await itTroncos.GetAllAsync(q => q.TroncoId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: ItTroncos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ItTronco itTronco = await itTroncos.GetFirstAsync(t => t.Id == id);
      if (itTronco == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ItTroncoViewModel>(itTronco);
      return View(viewModel);
    }

    // GET: ItTroncos/Create
    public ActionResult Create(int? id = null) {
      var viewModel = new ItTroncoViewModel {
        TroncoId = id.GetValueOrDefault()
      };

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(troncos.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.TroncoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value");
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(vias.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      ViewBag.CondicaoId = new SelectList(Condicao.Items.Where(p => p.Key > 0).ToList(), "Key", "Value");

      return View(viewModel);
    }

    // POST: ItTroncos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ItTroncoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.TroncoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value", viewModel.Sentido);
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(await vias.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PavimentoId);
      }
      ViewBag.CondicaoId = new SelectList(Condicao.Items.Where(p => p.Key > 0).ToList(), "Key", "Value", viewModel.CondicaoId);

      try {
        if (ModelState.IsValid) {
          ItTronco itTronco = mapper.Map<ItTronco>(viewModel);
          await itTroncos.Insert(itTronco);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: ItTroncos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ItTronco itTronco = await itTroncos.GetByIdAsync(id);
      if (itTronco == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ItTroncoViewModel>(itTronco);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.TroncoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value", viewModel.Sentido);
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(await vias.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PavimentoId);
      }
      ViewBag.CondicaoId = new SelectList(Condicao.Items.Where(p => p.Key > 0).ToList(), "Key", "Value", viewModel.CondicaoId);

      return View(viewModel);
    }

    // POST: ItTroncos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(ItTroncoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.TroncoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value", viewModel.Sentido);
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(await vias.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PavimentoId);
      }
      ViewBag.CondicaoId = new SelectList(Condicao.Items.Where(p => p.Key > 0).ToList(), "Key", "Value", viewModel.CondicaoId);

      try {
        if (ModelState.IsValid) {
          ItTronco itTronco = mapper.Map<ItTronco>(viewModel);
          await itTroncos.Update(itTronco);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: ItTroncos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ItTronco itTronco = await itTroncos.GetFirstAsync(t => t.Id == id);
      if (itTronco == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ItTroncoViewModel>(itTronco);
      return View(viewModel);
    }

    // POST: ItTroncos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      ItTronco itTronco = await itTroncos.GetByIdAsync(id);
      if (itTronco != null) {
        await itTroncos.Delete(itTronco);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (itTroncos != null)) {
        itTroncos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
