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
  public class TServicosController : Controller {
    private TServicoService tServicos = new TServicoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<TServicoViewModel, TServico>().ReverseMap();
                                          }).CreateMapper();

    // GET: TServicos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.tServicos = new TServicoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<TServicoViewModel>>(await tServicos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: TServicos
    public async Task<ActionResult> Filter(int? id, int? page) {
      var viewModel = mapper.Map<IEnumerable<TServicoViewModel>>(
                          await tServicos.GetAllAsync(q => q.TerminalId == id));
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: TServicos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      TServico tservico = await tServicos.GetFirstAsync(s => s.Id == id);
      if (tservico == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TServicoViewModel>(tservico);
      return View(viewModel);
    }

    // GET: TServicos/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(terminais.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      return View();
    }

    // POST: TServicos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(TServicoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(await terminais.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.TerminalId);
      }

      try {
        if (ModelState.IsValid) {
          TServico tservico = mapper.Map<TServico>(viewModel);
          await tServicos.Insert(tservico);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: TServicos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      TServico tservico = await tServicos.GetByIdAsync(id);
      if (tservico == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TServicoViewModel>(tservico);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(await terminais.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.TerminalId);
      }
      return View(viewModel);
    }

    // POST: TServicos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(TServicoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (TerminalService terminais = new TerminalService(user.ID)) {
        ViewBag.TerminalId = new SelectList(await terminais.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.TerminalId);
      }

      try {
        if (ModelState.IsValid) {
          TServico tservico = mapper.Map<TServico>(viewModel);
          await tServicos.Update(tservico);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: TServicos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      TServico tservico = await tServicos.GetFirstAsync(s => s.Id == id);
      if (tservico == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<TServicoViewModel>(tservico);
      return View(viewModel);
    }

    // POST: TServicos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      TServico tservico = await tServicos.GetByIdAsync(id);
      if (tservico != null) {
        await tServicos.Delete(tservico);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (tServicos != null)) {
        tServicos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
