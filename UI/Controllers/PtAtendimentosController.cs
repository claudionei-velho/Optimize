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
  public class PtAtendimentosController : Controller {
    private PtAtendimentoService atPontos = new PtAtendimentoService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<PtAtendimentoViewModel, PtAtendimento>().ReverseMap();
                                          }).CreateMapper();

    // GET: PtAtendimentos
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.atPontos = new PtAtendimentoService(user.ID);

      var viewModel = mapper.Map<IEnumerable<PtAtendimentoViewModel>>(await atPontos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: PtAtendimentos
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<PtAtendimentoViewModel>>(
                          await atPontos.GetAllAsync(q => q.AtendimentoId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: PtAtendimentos/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PtAtendimento atPonto = await atPontos.GetFirstAsync(p => p.Id == id);
      if (atPonto == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PtAtendimentoViewModel>(atPonto);
      return View(viewModel);
    }

    // GET: PtAtendimentos/Create
    public ActionResult Create(int? id = null) {
      var viewModel = new PtAtendimentoViewModel {
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
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(pontos.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name");
      }
      return View(viewModel);
    }

    // POST: PtAtendimentos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(PtAtendimentoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.AtendimentoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name", viewModel.PontoId);
      }

      try {
        if (ModelState.IsValid) {
          PtAtendimento atPonto = mapper.Map<PtAtendimento>(viewModel);
          await atPontos.Insert(atPonto);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: PtAtendimentos/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PtAtendimento atPonto = await atPontos.GetByIdAsync(id);
      if (atPonto == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PtAtendimentoViewModel>(atPonto);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.AtendimentoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name", viewModel.PontoId);
      }
      return View(viewModel);
    }

    // POST: PtAtendimentos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(PtAtendimentoViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (AtendimentoService atendimentos = new AtendimentoService(user.ID)) {
        ViewBag.AtendimentoId = new SelectList(await atendimentos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.AtendimentoId);
      }
      ViewBag.Sentido = new SelectList(Sentido.GetAll(), "Id", "Name", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name", viewModel.PontoId);
      }

      try {
        if (ModelState.IsValid) {
          PtAtendimento atPonto = mapper.Map<PtAtendimento>(viewModel);
          await atPontos.Update(atPonto);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: PtAtendimentos/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PtAtendimento atPonto = await atPontos.GetFirstAsync(p => p.Id == id);
      if (atPonto == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PtAtendimentoViewModel>(atPonto);
      return View(viewModel);
    }

    // POST: PtAtendimentos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      PtAtendimento atPonto = await atPontos.GetByIdAsync(id);
      if (atPonto != null) {
        await atPontos.Delete(atPonto);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (atPontos != null)) {
        atPontos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
