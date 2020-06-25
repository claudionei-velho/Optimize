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
  public class ItAtendimentosController : Controller {
    private ItAtendimentoService itAtendimentos = new ItAtendimentoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<ItAtendimentoViewModel, ItAtendimento>().ReverseMap();
                                          }).CreateMapper();

    // GET: ItAtendimentos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.itAtendimentos = new ItAtendimentoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<ItAtendimentoViewModel>>(await itAtendimentos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: ItAtendimentos
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<ItAtendimentoViewModel>>(
                          await itAtendimentos.GetAllAsync(q => q.AtendimentoId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: ItAtendimentos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ItAtendimento itAtendimento = await itAtendimentos.GetFirstAsync(i => i.Id == id);
      if (itAtendimento == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ItAtendimentoViewModel>(itAtendimento);
      return View(viewModel);
    }

    // GET: ItAtendimentos/Create
    public ActionResult Create(int? id = null) {
      var viewModel = new ItAtendimentoViewModel {
        AtendimentoId = id.GetValueOrDefault()
      };

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(atendimentos.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.AtendimentoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name");
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(vias.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name");
      }
      ViewBag.CondicaoId = new SelectList(Condicao.GetAll(), "Id", "Name");

      return View(viewModel);
    }

    // POST: ItAtendimentos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ItAtendimentoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(),  Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.AtendimentoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(await vias.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PavimentoId);
      }
      ViewBag.CondicaoId = new SelectList(Condicao.GetAll(), "Id", "Name", viewModel.CondicaoId);

      try {
        if (ModelState.IsValid) {
          ItAtendimento itAtendimento = mapper.Map<ItAtendimento>(viewModel);
          await itAtendimentos.Insert(itAtendimento);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: ItAtendimentos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ItAtendimento itAtendimento = await itAtendimentos.GetByIdAsync(id);
      if (itAtendimento == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ItAtendimentoViewModel>(itAtendimento);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.AtendimentoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(await vias.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PavimentoId);
      }
      ViewBag.CondicaoId = new SelectList(Condicao.GetAll(), "Id", "Name", viewModel.CondicaoId);

      return View(viewModel);
    }

    // POST: ItAtendimentos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(ItAtendimentoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.AtendimentoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (Services<Via> vias = new Services<Via>()) {
        ViewBag.PavimentoId = new SelectList(await vias.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Denominacao }), "Id", "Name", viewModel.PavimentoId);
      }
      ViewBag.CondicaoId = new SelectList(Condicao.GetAll(), "Id", "Name", viewModel.CondicaoId);

      try {
        if (ModelState.IsValid) {
          ItAtendimento itAtendimento = mapper.Map<ItAtendimento>(viewModel);
          await itAtendimentos.Update(itAtendimento);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: ItAtendimentos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ItAtendimento itAtendimento = await itAtendimentos.GetFirstAsync(i => i.Id == id);
      if (itAtendimento == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<ItAtendimentoViewModel>(itAtendimento);
      return View(viewModel);
    }

    // POST: ItAtendimentos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      ItAtendimento itAtendimento = await itAtendimentos.GetByIdAsync(id);
      if (itAtendimento != null) {
        await itAtendimentos.Delete(itAtendimento);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (itAtendimentos != null)) {
        itAtendimentos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
