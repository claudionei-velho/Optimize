using System.Security.Principal;

namespace UI.Security {
  public class MvcUser : GenericPrincipal {
    public MvcUser(IIdentity identity, string[] roles) : base(identity, roles) {
      Roles = roles;
    }

    public int ID { get; set; }
    public string User { get; set; }
    public string[] Roles { get; set; }
  }
}
