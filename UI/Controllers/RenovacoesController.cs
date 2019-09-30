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
  public class RenovacoesController : Controller {
    private RenovacaoService renovacoes = new RenovacaoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<RenovacaoViewModel, Renovacao>().ReverseMap();
                                          }).CreateMapper();

    // GET: Renovacoes
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.renovacoes = new RenovacaoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<RenovacaoViewModel>>(await renovacoes.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Horarios
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<RenovacaoViewModel>>(
                          await renovacoes.GetAllAsync(q => q.LinhaId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: Renovacoes/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Renovacao renovacao = await renovacoes.GetFirstAsync(r => r.Id == id);
      if (renovacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<RenovacaoViewModel>(renovacao);
      return View(viewModel);
    }

    // GET: Renovacoes/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name");
      }
      ViewBag.Mes = new SelectList(new Mes().GetAll(), "Id", "Name");
      ViewBag.DiaId = new SelectList(new Workday().GetAll(), "Id", "Name");

      return View();
    }

    // POST: Renovacoes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(RenovacaoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Mes = new SelectList(new Mes().GetAll(), "Id", "Name", viewModel.Mes);
      ViewBag.DiaId = new SelectList(new Workday().GetAll(), "Id", "Name", viewModel.DiaId);

      try {
        if (ModelState.IsValid) {
          Renovacao renovacao = mapper.Map<Renovacao>(viewModel);
          await renovacoes.Insert(renovacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Renovacoes/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Renovacao renovacao = await renovacoes.GetByIdAsync(id);
      if (renovacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<RenovacaoViewModel>(renovacao);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Mes = new SelectList(new Mes().GetAll(), "Id", "Name", viewModel.Mes);
      ViewBag.DiaId = new SelectList(new Workday().GetAll(), "Id", "Name", viewModel.DiaId);

      return View(viewModel);
    }

    // POST: Renovacoes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(RenovacaoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Mes = new SelectList(new Mes().GetAll(), "Id", "Name", viewModel.Mes);
      ViewBag.DiaId = new SelectList(new Workday().GetAll(), "Id", "Name", viewModel.DiaId);

      try {
        if (ModelState.IsValid) {
          Renovacao renovacao = mapper.Map<Renovacao>(viewModel);
          await renovacoes.Update(renovacao);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Renovacoes/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Renovacao renovacao = await renovacoes.GetFirstAsync(r => r.Id == id);
      if (renovacao == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<RenovacaoViewModel>(renovacao);
      return View(viewModel);
    }

    // POST: Renovacoes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Renovacao renovacao = await renovacoes.GetByIdAsync(id);
      if (renovacao != null) {
        await renovacoes.Delete(renovacao);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (renovacoes != null)) {
        renovacoes.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
