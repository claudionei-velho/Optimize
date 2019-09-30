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
  public class FViagensController : Controller {
    private readonly FViagemService fViagens = new FViagemService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<FViagemViewModel, FViagem>().ReverseMap();
                                          }).CreateMapper();

    // GET: FViagens
    public async Task<ActionResult> Index(int? page) {     
      var viewModel = mapper.Map<IEnumerable<FViagemViewModel>>(await fViagens.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: FViagens
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<FViagemViewModel>>(
                          await fViagens.GetAllAsync(q => q.ViagemId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: FViagens/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      FViagem fViagem = await fViagens.GetFirstAsync(f => f.Id == id);
      if (fViagem == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<FViagemViewModel>(fViagem);
      return View(viewModel);
    }

    // GET: FViagens/Create
    public ActionResult Create(int? id = null) {
      var viewModel = new FViagemViewModel {
        ViagemId = id.GetValueOrDefault()
      };

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (ViagemService viagens = new ViagemService()) {
        Viagem viagem = viagens.GetFirst(v => v.Id == id);

        using (PtLinhaService pontos = new PtLinhaService(user.ID)) {
          ViewBag.PontoId = new SelectList(pontos.GetSelect(
              q => new {
                Id = q.Id.ToString(), Name = q.Ponto.Prefixo + " | " + q.Ponto.Identificacao
              }, q => q.LinhaId == viagem.LnPesquisa.LinhaId), "Id", "Name");
        }
      }
      return View(viewModel);
    }

    // POST: FViagens/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(FViagemViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      Viagem viagem = new ViagemService().GetFirst(v => v.Id == viewModel.ViagemId);
      using (PtLinhaService pontos = new PtLinhaService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Ponto.Prefixo + " | " + q.Ponto.Identificacao
            }, q => q.LinhaId == viagem.LnPesquisa.LinhaId), "Id", "Name", viewModel.PontoId);
      }

      try {
        if (ModelState.IsValid) {
          FViagem fViagem = mapper.Map<FViagem>(viewModel);
          await fViagens.Insert(fViagem);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: FViagens/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      FViagem fViagem = await fViagens.GetByIdAsync(id);
      if (fViagem == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<FViagemViewModel>(fViagem);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (ViagemService viagens = new ViagemService()) {
        Viagem viagem = viagens.GetFirst(v => v.Id == viewModel.ViagemId);

        using (PtLinhaService pontos = new PtLinhaService(user.ID)) {
          ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
              q => new {
                Id = q.Id.ToString(), Name = q.Ponto.Prefixo + " | " + q.Ponto.Identificacao
              }, q => q.LinhaId == viagem.LnPesquisa.LinhaId), "Id", "Name", viewModel.PontoId);
        }
      }
      return View(viewModel);
    }

    // POST: FViagens/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(FViagemViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      Viagem viagem = new ViagemService().GetFirst(v => v.Id == viewModel.ViagemId);
      using (PtLinhaService pontos = new PtLinhaService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Ponto.Prefixo + " | " + q.Ponto.Identificacao
            }, q => q.LinhaId == viagem.LnPesquisa.LinhaId), "Id", "Name", viewModel.PontoId);
      }

      try {
        if (ModelState.IsValid) {
          FViagem fViagem = mapper.Map<FViagem>(viewModel);
          await fViagens.Update(fViagem);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: FViagens/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      FViagem fViagem = await fViagens.GetFirstAsync(f => f.Id == id);
      if (fViagem == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<FViagemViewModel>(fViagem);
      return View(viewModel);
    }

    // POST: FViagens/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      FViagem fViagem = await fViagens.GetByIdAsync(id);
      if (fViagem != null) {
        await fViagens.Delete(fViagem);
      }
      return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (fViagens != null)) {
        fViagens.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
