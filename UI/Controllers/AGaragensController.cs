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
  public class AGaragensController : Controller {
    private readonly AGaragemService aGaragens = new AGaragemService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<AGaragemViewModel, AGaragem>().ReverseMap();
                                          }).CreateMapper();

    // GET: AGaragens
    public async Task<ActionResult> Index(int? page) {
      var viewModel = mapper.Map<IEnumerable<AGaragemViewModel>>(await aGaragens.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: AGaragens/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      AGaragem aGaragem = await aGaragens.GetFirstAsync(g => g.Id == id);
      if (aGaragem == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<AGaragemViewModel>(aGaragem);

      return View(viewModel);
    }

    // GET: AGaragens/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EInstalacaoService eInstalacoes = new EInstalacaoService(user.ID)) {
        ViewBag.InstalacaoId = new SelectList(eInstalacoes.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Instalacao.Denominacao }), "Id", "Name");
      }
      return View();
    }

    // POST: AGaragens/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(AGaragemViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EInstalacaoService eInstalacoes = new EInstalacaoService(user.ID)) {
        ViewBag.InstalacaoId = new SelectList(await eInstalacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Instalacao.Denominacao },
            q => q.PropositoId == 1), "Id", "Name", viewModel.InstalacaoId);
      }

      try {
        if (ModelState.IsValid) {
          AGaragem aGaragem = mapper.Map<AGaragem>(viewModel);
          await aGaragens.Insert(aGaragem);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: AGaragens/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      AGaragem aGaragem = await aGaragens.GetByIdAsync(id);
      if (aGaragem == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<AGaragemViewModel>(aGaragem);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EInstalacaoService eInstalacoes = new EInstalacaoService(user.ID)) {
        ViewBag.InstalacaoId = new SelectList(await eInstalacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Instalacao.Denominacao },
            q => q.PropositoId == 1), "Id", "Name", viewModel.InstalacaoId);
      }
      return View(viewModel);
    }

    // POST: AGaragens/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(AGaragemViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (EInstalacaoService eInstalacoes = new EInstalacaoService(user.ID)) {
        ViewBag.InstalacaoId = new SelectList(await eInstalacoes.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Instalacao.Denominacao },
            q => q.PropositoId == 1), "Id", "Name", viewModel.InstalacaoId);
      }

      try {
        if (ModelState.IsValid) {
          AGaragem aGaragem = mapper.Map<AGaragem>(viewModel);
          await aGaragens.Update(aGaragem);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: AGaragens/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      AGaragem aGaragem = await aGaragens.GetFirstAsync(g => g.Id == id);
      if (aGaragem == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<AGaragemViewModel>(aGaragem);

      return View(viewModel);
    }

    // POST: AGaragens/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      AGaragem aGaragem = await aGaragens.GetByIdAsync(id);
      if (aGaragem != null) {
        await aGaragens.Delete(aGaragem);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (aGaragens != null)) {
        aGaragens.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
