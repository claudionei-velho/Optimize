using System;
using System.Collections.Generic;
using System.Linq;

namespace Bll.Lists {
  public static class Workday {
    public static Dictionary<int, string> Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Dias Úteis" },
        { 2, "Sábados" },
        { 3, "Dom./Feriados" }
    };

    public static Dictionary<int, string> Short = new Dictionary<int, string> {
        { 1, "Úteis" },
        { 2, "Sáb." },
        { 3, "Dom." }
    };

    public static IEnumerable<KeyValuePair<int, string>> GetAll() {
      return Data.Where(p => p.Key > 0).ToList();
    }

    public static int GetWorkday(DateTime dateRef) {
      int result = dateRef.DayOfWeek switch {
        DayOfWeek.Sunday => 3,
        DayOfWeek.Saturday => 2,
        _ => 1,
      };
      return result;
    }
  }
}
