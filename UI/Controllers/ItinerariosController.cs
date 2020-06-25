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
  public class ItinerariosController : Controller {
    private ItinerarioService itinerarios = new ItinerarioService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<ItinerarioViewModel, Itinerario>().ReverseMap();
                                          }).CreateMapper();

    // GET: Itinerarios
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.itinerarios = new ItinerarioService(user.ID);

      var viewModel = mapper.Map<IEnumerable<ItinerarioViewModel>>(await itinerarios.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Itinerarios
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<ItinerarioViewModel>>(
                          await itinerarios.GetAllAsync(q => q.LinhaId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: Itinerarios/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Itinerario itinerario = await itinerarios.GetFirstAsync(i => i.Id == id);
      if (itinerario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ItinerarioViewModel>(itinerario);
      return View(viewModel);
    }

    // GET: Itinerarios/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name");
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value");
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(vias.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      ViewBag.CondicaoId = new SelectList(Condicao.Items.Where(p => p.Key > 0).ToList(), "Key", "Value");

      return View();
    }

    // POST: Itinerarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ItinerarioViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value", viewModel.Sentido);
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(await vias.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PavimentoId);
      }
      ViewBag.CondicaoId = new SelectList(Condicao.Items.Where(p => p.Key > 0).ToList(), "Key", "Value", viewModel.CondicaoId);

      try {
        if (ModelState.IsValid) {
          Itinerario itinerario = mapper.Map<Itinerario>(viewModel);
          await itinerarios.Insert(itinerario);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Itinerarios/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Itinerario itinerario = await itinerarios.GetByIdAsync(id);
      if (itinerario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ItinerarioViewModel>(itinerario);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value", viewModel.Sentido);
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(await vias.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PavimentoId);
      }
      ViewBag.CondicaoId = new SelectList(Condicao.Items.Where(p => p.Key > 0).ToList(), "Key", "Value", viewModel.CondicaoId);

      return View(viewModel);
    }

    // POST: Itinerarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(ItinerarioViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(Sentido.Items.ToList(), "Key", "Value", viewModel.Sentido);
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(await vias.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PavimentoId);
      }
      ViewBag.CondicaoId = new SelectList(Condicao.Items.Where(p => p.Key > 0).ToList(), "Key", "Value", viewModel.CondicaoId);

      try {
        if (ModelState.IsValid) {
          Itinerario itinerario = mapper.Map<Itinerario>(viewModel);
          await itinerarios.Update(itinerario);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Itinerarios/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Itinerario itinerario = await itinerarios.GetFirstAsync(i => i.Id == id);
      if (itinerario == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ItinerarioViewModel>(itinerario);
      return View(viewModel);
    }

    // POST: Itinerarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Itinerario itinerario = await itinerarios.GetByIdAsync(id);
      if (itinerario != null) {
        await itinerarios.Delete(itinerario);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (itinerarios != null)) {
        itinerarios.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
