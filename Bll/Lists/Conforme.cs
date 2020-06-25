using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public static class Conforme {
    public static Dictionary<int, string> Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Disponível" },
        { 2, "Indisponível" },
        { 3, "Terceirizado" }
    };

    public static IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.Where(p => p.Key > 0).ToList();
    }
  }
}
