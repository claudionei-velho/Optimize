using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public static class Transmissao {
    public static Dictionary<int, string> Data = new Dictionary<int, string> {
        { 1, "Manual" },
        { 2, "Automática" }
    };

    public static IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.ToList();
    }
  }
}
