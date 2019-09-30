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
  public class OfertasController : Controller {
    private OfertaService ofertas = new OfertaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<OfertaViewModel, Oferta>().ReverseMap();
                                          }).CreateMapper();

    // GET: Ofertas
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.ofertas = new OfertaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<OfertaViewModel>>(await ofertas.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Ofertas/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Oferta oferta = await ofertas.GetFirstAsync(f => f.Id == id);
      if (oferta == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<OfertaViewModel>(oferta);
      return View(viewModel);
    }

    // GET: Ofertas/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name");
      }
      ViewBag.Mes = new SelectList(new Mes().GetAll(), "Id", "Name");
      using (TCategoriaService tCategorias = new TCategoriaService(user.ID)) {
        ViewBag.Categoria = new SelectList(tCategorias.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao}), "Id", "Name");
      }
      return View();
    }

    // POST: Ofertas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(OfertaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Mes = new SelectList(new Mes().GetAll(), "Id", "Name", viewModel.Mes);
      using (TCategoriaService tCategorias = new TCategoriaService(user.ID)) {
        ViewBag.Categoria = new SelectList(await tCategorias.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao}), "Id", "Name", viewModel.Categoria);
      }

      try {
        if (ModelState.IsValid) {
          Oferta oferta = mapper.Map<Oferta>(viewModel);
          await ofertas.Insert(oferta);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Ofertas/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Oferta oferta = await ofertas.GetByIdAsync(id);
      if (oferta == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<OfertaViewModel>(oferta);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Mes = new SelectList(new Mes().GetAll(), "Id", "Name", viewModel.Mes);

      using (Services<Linha> linhas = new Services<Linha>()) {
        int empresaId = linhas.GetById(viewModel.LinhaId).EmpresaId;

        using (Services<TCategoria> tCategorias = new Services<TCategoria>()) {
          ViewBag.Categoria = new SelectList(await tCategorias.GetSelectAsync(
              q => new { Id = q.Id.ToString(), Name = q.Denominacao },
              q => q.EmpresaId == empresaId), "Id", "Name", viewModel.Categoria);
        }
      }
      return View(viewModel);
    }

    // POST: Ofertas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(OfertaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Mes = new SelectList(new Mes().GetAll(), "Id", "Name", viewModel.Mes);

      int empresaId = new Services<Linha>().GetById(viewModel.LinhaId).EmpresaId;
      using (Services<TCategoria> tCategorias = new Services<TCategoria>()) {
        ViewBag.Categoria = new SelectList(await tCategorias.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao},
            q => q.EmpresaId == empresaId), "Id", "Name", viewModel.Categoria);
      }

      try {
        if (ModelState.IsValid) {
          Oferta oferta = mapper.Map<Oferta>(viewModel);
          await ofertas.Update(oferta);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Ofertas/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Oferta oferta = await ofertas.GetFirstAsync(f => f.Id == id);
      if (oferta == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<OfertaViewModel>(oferta);
      return View(viewModel);
    }

    // POST: Ofertas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Oferta oferta = await ofertas.GetByIdAsync(id);
      if (oferta != null) {
        await ofertas.Delete(oferta);
      }
      return RedirectToAction(nameof(Index));
    }

    public JsonResult GetTCategorias(int id) {      
      HashSet<SelectBox> result = new HashSet<SelectBox>();

      using (Services<Linha> linhas = new Services<Linha>()) {
        int empresaId = linhas.GetById(id).EmpresaId;
        using (Services<TCategoria> tCategorias = new Services<TCategoria>()) {
          foreach (TCategoria item in tCategorias.GetQuery(q => q.EmpresaId == empresaId)) {
            result.Add(new SelectBox() { Id = item.Id.ToString(), Name = item.Denominacao });
          }
        }
      }
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (ofertas != null)) {
        ofertas.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
