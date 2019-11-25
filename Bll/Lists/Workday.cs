using System;
using System.Collections.Generic;

namespace Bll.Lists {
  public class Workday {
    public Dictionary<int, string> Data;
    public Dictionary<int, string> Short;

    public Workday() {
      Data = new Dictionary<int, string> {
        { 0, string.Empty },
        { 1, "Dias Úteis" },
        { 2, "Sábados" },
        { 3, "Dom./Feriados" }
      };

      Short = new Dictionary<int, string> {
        { 1, "Úteis" },
        { 2, "Sáb." },
        { 3, "Dom." }
      };
    }

    public IEnumerable<dynamic> GetAll() {
      List<dynamic> result = new List<dynamic>();
      foreach (var item in Data) {
        if (item.Key > 0) {
          result.Add(new { Id = item.Key.ToString(), Name = item.Value });
        }
      }
      return result;
    }

    public int GetWorkday(DateTime dateRef) {
      int result = dateRef.DayOfWeek switch {
        DayOfWeek.Sunday => 3,
        DayOfWeek.Saturday => 2,
        _ => 1,
      };
      return result;
    }
  }
}
