using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public static class Sentido {
    public static Dictionary<string, string> Data = new Dictionary<string, string> {
        { "AB", "A -> B" },
        { "BA", "B -> A" }
    };

    public static IEnumerable<KeyValuePair<string, string>> GetAll() {
      return Data.ToList();
    }
  }
}
