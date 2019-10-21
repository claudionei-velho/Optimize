﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
  public class PtLinhasController : Controller {
    private PtLinhaService lPontos = new PtLinhaService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<PtLinhaViewModel, PtLinha>().ReverseMap();
                                          }).CreateMapper();

    // GET: PtLinhas
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.lPontos = new PtLinhaService(user.ID);

      var viewModel = mapper.Map<IEnumerable<PtLinhaViewModel>>(await lPontos.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    // GET: PtLinhas
    public async Task<ActionResult> Filter(int? id, int page = 1) {
      var viewModel = mapper.Map<IEnumerable<PtLinhaViewModel>>(
                          await lPontos.GetAllAsync(q => q.LinhaId == id));
      return View(viewModel.ToPagedList(page, 16));
    }

    // GET: PtLinhas/Details/5
    public async Task<ActionResult> Details(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PtLinha ptlinha = await lPontos.GetFirstAsync(p => p.Id == id);
      if (ptlinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PtLinhaViewModel>(ptlinha);
      return View(viewModel);
    }

    // GET: PtLinhas/Create
    public ActionResult Create(int? id) {
      var viewModel = new PtLinhaViewModel {
        LinhaId = id.GetValueOrDefault()
      };

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name");
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(pontos.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name");
      }
      using (Services<PtOrigem> origens = new Services<PtOrigem>()) {
        ViewBag.OrigemId = new SelectList(origens.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao },
            q => (q.LinhaId == viewModel.LinhaId) && q.Sentido.Equals("AB"),
            q => q.OrderBy(p => p.Id)), "Id", "Name");
      }
      using (Services<PtDestino> destinos = new Services<PtDestino>()) {
        ViewBag.DestinoId = new SelectList(destinos.GetSelect(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao },
            q => (q.LinhaId == viewModel.LinhaId) && q.Sentido.Equals("AB"),
            q => q.OrderBy(p => p.Id)), "Id", "Name");
      }
      ViewBag.Fluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name");

      return View(viewModel);
    }

    // POST: PtLinhas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(PtLinhaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name", viewModel.PontoId);
      }
      using (Services<PtOrigem> origens = new Services<PtOrigem>()) {
        ViewBag.OrigemId = new SelectList(await origens.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao },
            q => (q.LinhaId == viewModel.LinhaId) && q.Sentido.Equals("AB"),
            q => q.OrderBy(p => p.Id)), "Id", "Name", viewModel.OrigemId);
      }
      using (Services<PtDestino> destinos = new Services<PtDestino>()) {
        ViewBag.DestinoId = new SelectList(await destinos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao },
            q => (q.LinhaId == viewModel.LinhaId) && q.Sentido.Equals("AB"),
            q => q.OrderBy(p => p.Id)), "Id", "Name", viewModel.DestinoId);
      }
      ViewBag.Fluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.Fluxo);

      try {
        if (ModelState.IsValid) {
          PtLinha ptlinha = mapper.Map<PtLinha>(viewModel);
          await lPontos.Insert(ptlinha);
        }
        return RedirectToAction("Filter", new { id = viewModel.LinhaId });
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: PtLinhas/Edit/5
    public async Task<ActionResult> Edit(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PtLinha ptlinha = await lPontos.GetByIdAsync(id);
      if (ptlinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PtLinhaViewModel>(ptlinha);

      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new { 
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name", viewModel.PontoId);
      }
      using (Services<PtOrigem> origens = new Services<PtOrigem>()) {
        ViewBag.OrigemId = new SelectList(await origens.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao },
            q => (q.LinhaId == viewModel.LinhaId) && q.Sentido.Equals(viewModel.Sentido) && (q.Id != viewModel.Id),
            q => q.OrderBy(p => p.Id)), "Id", "Name", viewModel.OrigemId);
      }
      using (Services<PtDestino> destinos = new Services<PtDestino>()) {
        ViewBag.DestinoId = new SelectList(await destinos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao },
            q => (q.LinhaId == viewModel.LinhaId) && q.Sentido.Equals(viewModel.Sentido) && (q.Id != viewModel.Id),
            q => q.OrderBy(p => p.Id)), "Id", "Name", viewModel.DestinoId);
      }
      ViewBag.Fluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.Fluxo);

      return View(viewModel);
    }

    // POST: PtLinhas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(PtLinhaViewModel viewModel) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      using (LinhaService linhas = new LinhaService(user.ID)) {
        ViewBag.LinhaId = new SelectList(linhas.GetSelect(
            q => new {
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Denominacao
            }), "Id", "Name", viewModel.LinhaId);
      }
      ViewBag.Sentido = new SelectList(new Sentido().GetAll(), "Id", "Name", viewModel.Sentido);
      using (PontoService pontos = new PontoService(user.ID)) {
        ViewBag.PontoId = new SelectList(await pontos.GetSelectAsync(
            q => new { 
              Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao
            }), "Id", "Name", viewModel.PontoId);
      }
      using (Services<PtOrigem> origens = new Services<PtOrigem>()) {
        ViewBag.OrigemId = new SelectList(await origens.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao },
            q => (q.LinhaId == viewModel.LinhaId) && q.Sentido.Equals(viewModel.Sentido) && (q.Id != viewModel.Id),
            q => q.OrderBy(p => p.Id)), "Id", "Name", viewModel.OrigemId);
      }
      using (Services<PtDestino> destinos = new Services<PtDestino>()) {
        ViewBag.DestinoId = new SelectList(await destinos.GetSelectAsync(
            q => new { Id = q.Id.ToString(), Name = q.Prefixo + " | " + q.Identificacao },
            q => (q.LinhaId == viewModel.LinhaId) && q.Sentido.Equals(viewModel.Sentido) && (q.Id != viewModel.Id),
            q => q.OrderBy(p => p.Id)), "Id", "Name", viewModel.DestinoId);
      }
      ViewBag.Fluxo = new SelectList(new Fluxo().GetAll(), "Id", "Name", viewModel.Fluxo);

      try {
        if (ModelState.IsValid) {
          PtLinha ptlinha = mapper.Map<PtLinha>(viewModel);
          await lPontos.Update(ptlinha);
        }
        return RedirectToAction(nameof(Index));
      }
      catch {
        return View(viewModel);
      }
    }

    // GET: PtLinhas/Delete/5
    public async Task<ActionResult> Delete(int? id) {
      if (id == null) {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      PtLinha ptlinha = await lPontos.GetFirstAsync(p => p.Id == id);
      if (ptlinha == null) {
        return HttpNotFound();
      }
      var viewModel = mapper.Map<PtLinhaViewModel>(ptlinha);
      return View(viewModel);
    }

    // POST: PtLinhas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id) {
      PtLinha ptlinha = await lPontos.GetByIdAsync(id);
      if (ptlinha != null) {
        await lPontos.Delete(ptlinha);
      }
      return RedirectToAction(nameof(Index));
    }

    public JsonResult GetOrigens(int id, string go, int? pk = null) {
      HashSet<SelectBox> result = new HashSet<SelectBox>();

      Expression<Func<PtOrigem, bool>> applyFilter = q => (q.LinhaId == id) && q.Sentido.Equals(go);
      if (pk.HasValue) {
        applyFilter = q => (q.LinhaId == id) && q.Sentido.Equals(go) && (q.Id != pk.Value);
      }

      using (Services<PtOrigem> ptOrigens = new Services<PtOrigem>()) {
        foreach (PtOrigem item in ptOrigens.GetQuery(applyFilter, q => q.OrderBy(p => p.Id))) {
          result.Add(new SelectBox() {
            Id = item.Id.ToString(), Name = item.Prefixo + " | " + item.Identificacao
          });
        }
      }
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    public JsonResult GetDestinos(int id, string go, int? pk = null) {
      HashSet<SelectBox> result = new HashSet<SelectBox>();

      Expression<Func<PtDestino, bool>> applyFilter = q => (q.LinhaId == id) && q.Sentido.Equals(go);
      if (pk.HasValue) {
        applyFilter = q => (q.LinhaId == id) && q.Sentido.Equals(go) && (q.Id != pk.Value);
      }

      using (Services<PtDestino> ptDestinos = new Services<PtDestino>()) {
        foreach (PtDestino item in ptDestinos.GetQuery(applyFilter, q => q.OrderBy(p => p.Id))) {
          result.Add(new SelectBox() {
            Id = item.Id.ToString(), Name = item.Prefixo + " | " + item.Identificacao
          });
        }
      }
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (lPontos != null)) {
        lPontos.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}