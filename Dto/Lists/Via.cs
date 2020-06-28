using System.Collections.Generic;

namespace Dto.Lists {
  public static class Via {
    public static IDictionary<int, string> Items = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Sem pavimentação" },
        { 2, "Pavimentação em rocha (paralelepípedos)" },
        { 3, "Pavimentação em concreto" },
        { 4, "Pavimentação asfáltica" }
    };
  }
}
