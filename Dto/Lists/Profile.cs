using System.Collections.Generic;

namespace Dto.Lists {
  public static class Profile {
    public static IDictionary<int, string> Items = new Dictionary<int, string> {
        { 1, "Executivo" },
        { 2, "Gerencial" },
        { 3, "Operacional" }
    };
  }
}
