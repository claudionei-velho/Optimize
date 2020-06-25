using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public static class Semana {
    public static Dictionary<int, string> Data = new Dictionary<int, string> {
        { 1, "Domingo" },
        { 2, "Segunda-feira" },
        { 3, "Terça-feira" },
        { 4, "Quarta-feira" },
        { 5, "Quinta-feira" },
        { 6, "Sexta-feira" },
        { 7, "Sábado" }
    };

    public static IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.ToList();
    }
  }
}
