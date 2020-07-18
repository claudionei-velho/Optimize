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
  public class AtendimentosController : Controller {
    private AtendimentoService atendimentos = new AtendimentoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
      cfg.CreateMap<AtendimentoViewModel, Atendimento>().ReverseMap();
    }).CreateMapper();

    // GET: Atendimentos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.atendimentos = new AtendimentoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<AtendimentoViewModel>>(await atendimentos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Atendimentos/Filter
    public async Task<ActionResult> Filter(int? id, int? page) {
      var viewModel = mapper.Map<IEnumerable<AtendimentoViewModel>>(
                          await atendimentos.GetAllAsync(q => q.LinhaId == id));
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: Atendimentos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Atendimento atendimento = await atendimentos.GetFirstAsync(a => a.Id == id);
      if (atendimento == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<AtendimentoViewModel>(atendimento);
      return View(viewModel);
    }

    // GET: Atendimentos/Create
    public ActionResult Create() {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name");
      }
      return View();
    }

    // POST: Atendimentos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(AtendimentoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }

      try {
        if (ModelState.IsValid) {
          Atendimento atendimento = mapper.Map<Atendimento>(viewModel);
          await atendimentos.Insert(atendimento);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Atendimentos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Atendimento atendimento = await atendimentos.GetByIdAsync(id);
      if (atendimento == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<AtendimentoViewModel>(atendimento);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      return View(viewModel);
    }

    // POST: Atendimentos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(AtendimentoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(await linhas.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }

      try {
        if (ModelState.IsValid) {
          Atendimento atendimento = mapper.Map<Atendimento>(viewModel);
          await atendimentos.Update(atendimento);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: Atendimentos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Atendimento atendimento = await atendimentos.GetFirstAsync(a => a.Id == id);
      if (atendimento == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<AtendimentoViewModel>(atendimento);
      return View(viewModel);
    }

    // POST: Atendimentos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      Atendimento atendimento = await atendimentos.GetByIdAsync(id);
      if (atendimento != null) {
        await atendimentos.Delete(atendimento);
      }
      return RedirectToAction(nameof(Index));
    }

    public JsonResult GetAtendimentos(int id) {
      using Services<Atendimento> atendimentos = new Services<Atendimento>();
      return Json(atendimentos.GetQuery(q => q.LinhaId == id,
                                        q => q.OrderBy(p => p.Id)).Select(p => new { p.Id, p.Denominacao })
                      .ToDictionary(k => k.Id, k => k.Denominacao), JsonRequestBehavior.AllowGet);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (atendimentos != null)) {
        atendimentos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
