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
  public class MapasLinhaController : Controller {
    private MapaLinhaService mapas = new MapaLinhaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<MapaLinhaViewModel, MapaLinha>().ReverseMap();
                                          }).CreateMapper();

    // GET: MapasLinha
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.mapas = new MapaLinhaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<MapaLinhaViewModel>>(await mapas.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: MapasLinha/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      MapaLinha mapa = await mapas.GetFirstAsync(m => m.Id == id);
      if (mapa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<MapaLinhaViewModel>(mapa);
      return View(viewModel);
    }

    // GET: MapasLinha/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao }), "Id", "Name");
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name");
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(atendimentos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao }), "Id", "Name");
      }
      return View();
    }

    // POST: MapasLinha/Create
    [HttpPost]
    public async Task<ActionResult> Create(MapaLinhaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.AtendimentoId);
      }

      try {
        if (ModelState.IsValid) {
          MapaLinha mapa = mapper.Map<MapaLinha>(viewModel);
          await mapas.Insert(mapa);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: MapasLinha/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      MapaLinha mapa = await mapas.GetByIdAsync(id);
      if (mapa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<MapaLinhaViewModel>(mapa);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.AtendimentoId);
      }
      return View(viewModel);
    }

    // POST: MapasLinha/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(MapaLinhaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.AtendimentoId);
      }

      try {
        if (ModelState.IsValid) {
          MapaLinha mapa = mapper.Map<MapaLinha>(viewModel);
          await mapas.Update(mapa);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: MapasLinha/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      MapaLinha mapa = await mapas.GetFirstAsync(m => m.Id == id);
      if (mapa == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<MapaLinhaViewModel>(mapa);
      return View(viewModel);
    }

    // POST: MapasLinha/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      MapaLinha mapa = await mapas.GetByIdAsync(id);
      if (mapa != null) {
        await mapas.Delete(mapa);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (mapas != null)) {
        mapas.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
