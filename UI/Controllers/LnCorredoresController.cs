using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using AutoMapper;
using PagedList;

using Bll;
using Bll.Lists;
using Bll.Services;
using Dto.Models;
using UI.Models;
using UI.Security;

namespace UI.Controllers {
  [Authorize]
  public class LnCorredoresController : Controller {
    private LnCorredorService lCorredores = new LnCorredorService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<LnCorredorViewModel, LnCorredor>().ReverseMap();
                                          }).CreateMapper();

    // GET: LnCorredores
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.lCorredores = new LnCorredorService(user.ID);

      var viewModel = mapper.Map<IEnumerable<LnCorredorViewModel>>(await lCorredores.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: LnCorredores
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<LnCorredorViewModel>>(
                          await lCorredores.GetAllAsync(q => q.CorredorId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: LnCorredores/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnCorredor lCorredor = await lCorredores.GetFirstAsync(l => l.Id == id);
      if (lCorredor == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnCorredorViewModel>(lCorredor);
      return View(viewModel);
    }

    // GET: LnCorredores/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (CorredorService corredores = new CorredorService(user.ID)) {
        ViewBag.CorredorId = new SelectList(corredores.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name");
      }
      ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name");

      return View();
    }

    // POST: LnCorredores/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(LnCorredorViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (CorredorService corredores = new CorredorService(user.ID)) {
        ViewBag.CorredorId = new SelectList(await corredores.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.CorredorId);
      }
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name", viewModel.Sentido);

      try {
        if (ModelState.IsValid) {
          LnCorredor lCorredor = mapper.Map<LnCorredor>(viewModel);
          await lCorredores.Insert(lCorredor);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: LnCorredores/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnCorredor lCorredor = await lCorredores.GetByIdAsync(id);
      if (lCorredor == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnCorredorViewModel>(lCorredor);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;      
      using (CorredorService corredores = new CorredorService(user.ID)) {
        ViewBag.CorredorId = new SelectList(await corredores.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.CorredorId);
      }
      using (Services<Corredor> corredores = new Services<Corredor>()) {
        int empresaId = corredores.GetById(viewModel.CorredorId).EmpresaId;

        using (Services<Linha> linhas = new Services<Linha>()) {
          ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
              q => new {
                Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
              },
              q => q.EmpresaId == empresaId), "Id", "Name", viewModel.LinhaId);
        }
        ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name", viewModel.Sentido);
      }
      return View(viewModel);
    }

    // POST: LnCorredores/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(LnCorredorViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (CorredorService corredores = new CorredorService(user.ID)) {
        ViewBag.CorredorId = new SelectList(await corredores.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.CorredorId);
      }
      using (Services<Linha> linhas = new Services<Linha>()) {
        int empresaId;
        using (Services<Corredor> corredores = new Services<Corredor>()) {
          empresaId = corredores.GetById(viewModel.CorredorId).EmpresaId;
        }
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            },
            q => q.EmpresaId == empresaId), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name", viewModel.Sentido);

      try {
        if (ModelState.IsValid) {
          LnCorredor lCorredor = mapper.Map<LnCorredor>(viewModel);
          await lCorredores.Update(lCorredor);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: LnCorredores/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      LnCorredor lCorredor = await lCorredores.GetFirstAsync(l => l.Id == id);
      if (lCorredor == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<LnCorredorViewModel>(lCorredor);
      return View(viewModel);
    }

    // POST: LnCorredores/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      LnCorredor lCorredor = await lCorredores.GetByIdAsync(id);
      if (lCorredor != null) {
        await lCorredores.Delete(lCorredor);
      }
      return RedirectToAction(nameof(Index));
    }

    public JsonResult GetLinhas(int id) {      
      HashSet<SelectBox> result = new HashSet<SelectBox>();
      using (Services<Corredor> corredores = new Services<Corredor>()) {
        int empresaId = corredores.GetById(id).EmpresaId;

        using (Services<Linha> linhas = new Services<Linha>()) {
          foreach (Linha item in linhas.GetQuery(q => q.EmpresaId == empresaId)) {
            result.Add(new SelectBox() {
              Id = item.Id.ToString(), Name = item.Prefixo + " | " + item.Denominacao
            });
          }
        }
      }
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (lCorredores != null)) {
        lCorredores.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
