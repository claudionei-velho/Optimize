using System.Collections.Generic;

namespace Bll.Lists {
  public class Profile {
    public Dictionary<int, string> Data;

    public Profile() {
      Data = new Dictionary<int, string> {
        { 1, "Executivo" },
        { 2, "Gerencial" },
        { 3, "Operacional" }
      };
    }

    public IEnumerable<dynamic> GetAll() {
      List<dynamic> result = new List<dynamic>();
      foreach (var item in Data) {
        result.Add(new { Id = item.Key.ToString(), Name = item.Value });
      }
      return result;
    }
  }
}
