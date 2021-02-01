using System;
using System.Collections.Generic;

namespace Dto.Lists {
  public static class Workday {
    [Flags]
    public enum WorkDays : int {
      Util = 1,
      Sabado = 2,
      Domingo = 3
    };

    public static IDictionary<int, string> Items = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Dias Úteis" },
        { 2, "Sábados" },
        { 3, "Dom./Feriados" }
    };

    public static IDictionary<int, string> Short = new Dictionary<int, string> {
        { 1, "Úteis" },
        { 2, "Sáb." },
        { 3, "Dom." }
    };

    public static int GetWorkday(DateTime dateRef) {
      return dateRef.DayOfWeek switch {
        DayOfWeek.Sunday => 3,
        DayOfWeek.Saturday => 2,
        _ => 1,
      };
    }
  }
}
