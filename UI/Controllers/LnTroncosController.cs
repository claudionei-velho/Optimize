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
  public class LnTroncosController : Controller {
    private LnTroncoService lTroncos = new LnTroncoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<LnTroncoViewModel, LnTronco>().ReverseMap();
                                          }).CreateMapper();

    // GET: LnTroncos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.lTroncos = new LnTroncoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<LnTroncoViewModel>>(await lTroncos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: LnTroncos
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<LnTroncoViewModel>>(
                          await lTroncos.GetAllAsync(q => q.TroncoId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: LnTroncos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnTronco lTronco = await lTroncos.GetFirstAsync(t => t.Id == id);
      if (lTronco == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnTroncoViewModel>(lTronco);
      return View(viewModel);
    }

    // GET: LnTroncos/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(troncos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name");
      }
      return View();
    }

    // POST: LnTroncos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(LnTroncoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.TroncoId);
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }

      try {
        if (ModelState.IsValid) {
          LnTronco lTronco = mapper.Map<LnTronco>(viewModel);
          await lTroncos.Insert(lTronco);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: LnTroncos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnTronco lTronco = await lTroncos.GetByIdAsync(id);
      if (lTronco == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnTroncoViewModel>(lTronco);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.TroncoId);
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      return View(viewModel);
    }

    // POST: LnTroncos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(LnTroncoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TroncoService troncos = new TroncoService(user.ID)) {
        ViewBag.TroncoId = new SelectList(await troncos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.TroncoId);
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }

      try {
        if (ModelState.IsValid) {
          LnTronco lTronco = mapper.Map<LnTronco>(viewModel);
          await lTroncos.Update(lTronco);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: LnTroncos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnTronco lTronco = await lTroncos.GetFirstAsync(t => t.Id == id);
      if (lTronco == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnTroncoViewModel>(lTronco);
      return View(viewModel);
    }

    // POST: LnTroncos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      LnTronco lTronco = await lTroncos.GetByIdAsync(id);
      if (lTronco != null) {
        await lTroncos.Delete(lTronco);
      }
      return RedirectToAction(nameof(Index));
    }

    public JsonResult GetLinhas(int id) {
      using Services<Tronco> troncos = new Services<Tronco>();

      using Services<Linha> linhas = new Services<Linha>();
      return Json(linhas.GetQuery(q => q.EmpresaId == troncos.GetById(id).EmpresaId)
                      .Select(p => new { p.Id, p.Prefixo, p.Denominacao })
                      .ToDictionary(k => k.Id, k => $"{k.Prefixo} | {k.Denominacao}"), JsonRequestBehavior.AllowGet);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (lTroncos != null)) {
        lTroncos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
