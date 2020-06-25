using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public static class Profile {
    public static Dictionary<int, string> Data = new Dictionary<int, string> {
        { 1, "Executivo" },
        { 2, "Gerencial" },
        { 3, "Operacional" }
    };

    public static IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.ToList();
    }
  }
}
