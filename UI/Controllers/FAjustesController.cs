using System.Collections.Generic;
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
  public class FAjustesController : Controller {
    private FAjusteService fAjustes = new FAjusteService();
    private readonly IMapper mapper = new MapperConfiguration(cfg => {
                                            cfg.CreateMap<FAjusteViewModel, FAjuste>().ReverseMap();
                                          }).CreateMapper();

    // GET: FAjustes
    public async Task<ActionResult> Index(int? page) {
      MvcUser user = System.Web.HttpContext.Current.User as MvcUser;
      this.fAjustes = new FAjusteService(user.ID);

      var viewModel = mapper.Map<IEnumerable<FAjusteViewModel>>(await fAjustes.GetAllAsync());
      return View(viewModel.ToPagedList(page ?? 1, 16));
    }

    protected override void Dispose(bool disposing) {
      if (disposing && (fAjustes != null)) {
        fAjustes.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
