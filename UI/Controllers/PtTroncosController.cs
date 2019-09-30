using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll.Lists;
using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class PtTroncosController : Controller {
    private PtTroncoService trPontos = new PtTroncoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<PtTroncoViewModel, PtTronco>().ReverseMap();
                                          }).CreateMapper();

    // GET: PtTroncos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.trPontos = new PtTroncoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<PtTroncoViewModel>>(await trPontos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: PtTroncos
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<PtTroncoViewModel>>(
                          await trPontos.GetAllAsync(q => q.TroncoId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: PtTroncos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PtTronco trPonto = await trPontos.GetFirstAsync(p => p.Id == id);
      if (trPonto == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PtTroncoViewModel>(trPonto);
      return View(viewModel);
    }

    // GET: PtTroncos/Create
    public ActionResult Create(int? id = null) {
      var viewModel = new PtTroncoViewModel {
        TroncoId = id.GetValueOrDefault()
      };

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(troncos.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.TroncoId);
      }
      ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name");
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(pontos.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name");
      }
      return View(viewModel);
    }

    // POST: PtTroncos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(PtTroncoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.TroncoId);
      }
      ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name", viewModel.PontoId);
      }

      try {
        if (ModelState.IsValid) {
          PtTronco trPonto = mapper.Map<PtTronco>(viewModel);
          await trPontos.Insert(trPonto);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: PtTroncos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PtTronco trPonto = await trPontos.GetByIdAsync(id);
      if (trPonto == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PtTroncoViewModel>(trPonto);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.TroncoId);
      }
      ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name", viewModel.PontoId);
      }
      return View(viewModel);
    }

    // POST: PtTroncos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(PtTroncoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.TroncoId);
      }
      ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name", viewModel.PontoId);
      }

      try {
        if (ModelState.IsValid) {
          PtTronco trPonto = mapper.Map<PtTronco>(viewModel);
          await trPontos.Update(trPonto);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: PtTroncos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PtTronco trPonto = await trPontos.GetFirstAsync(p => p.Id == id);
      if (trPonto == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PtTroncoViewModel>(trPonto);
      return View(viewModel);
    }

    // POST: PtTroncos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      PtTronco trPonto = await trPontos.GetByIdAsync(id);
      if (trPonto != null) {
        await trPontos.Delete(trPonto);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (trPontos != null)) {
        trPontos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
