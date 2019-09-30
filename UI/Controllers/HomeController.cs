using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Bll.Services;
using Dto.Models;
using UI.Extensions;
using UI.Models;

namespace UI.Controllers {
  public class HomeController : Controller {
    public ActionResult Index() {
      return View();
    }

    public ActionResult Login() {
      ViewBag.Message = "Login";

      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginViewModel usuario, string returnUrl) {
      if (!ModelState.IsValid) {
        return View(usuario);
      }

      try {
        Usuario usuarios = await new LoginService().Authenticate(usuario.Login, usuario.Senha);
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            1, usuario.Login, DateTime.Now, DateTime.Now.AddMinutes(60), true, usuarios.Id + "|" + usuarios.Nome);
        string ticketEncrypt = FormsAuthentication.Encrypt(ticket);

        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketEncrypt) {
          Expires = DateTime.Now.AddMinutes(60)
        };
        Response.Cookies.Add(cookie);
        if (string.IsNullOrWhiteSpace(returnUrl)) {
          return RedirectToAction("Index");
        }
        return Redirect(returnUrl);
      }
      catch (OptimizerException ex) {
        ModelState.AddModelException(ex);
      }
      return View();
    }

    public ActionResult Logout() {
      FormsAuthentication.SignOut();
      return RedirectToAction("Index");
    }

    public ActionResult About() {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact() {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}
