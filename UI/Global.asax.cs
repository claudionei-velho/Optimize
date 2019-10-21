﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

using UI.Security;

namespace UI {
  public class MvcApplication : HttpApplication {
    protected void Application_Start() {
      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }

    protected void Application_AuthenticateRequest() {
      HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
      if (cookie != null) {
        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
        MvcUser user = new MvcUser(new FormsIdentity(ticket), new string[] { "Admin" });

        string[] userData = ticket.UserData.Split('|');
        user.ID = Convert.ToInt32(userData[0]);
        user.User = userData[1];

        HttpContext.Current.User = user;
      }
    }
  }
}