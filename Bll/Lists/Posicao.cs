using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public static class Posicao {
    public static Dictionary<int, string> Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Dianteiro" },
        { 2, "Central" },
        { 3, "Traseiro" }
    };

    public static IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.Where(p => p.Key > 0).ToList();
    }
  }
}
