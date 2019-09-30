using System.Data.Entity;
using System.Threading.Tasks;

using Dal;
using Dto.Models;

namespace Bll.Services {
  public class LoginService : BaseValidator<Usuario> {
    public async Task<Usuario> Authenticate(string login, string password) {
      using (DataContext context = new DataContext()) {
        Usuario usuario = await context.Set<Usuario>().AsNoTracking().FirstOrDefaultAsync(
                                    u => u.Login.Equals(login) && u.Senha.Equals(password) && u.Ativo);
        if (usuario == null) {
          throw new OptimizerException(new ErrorField("Password", "Usuário e/ou senha incorretos."));
        }
        return usuario;
      }
    }
  }
}
