using System.Collections.Generic;

namespace Bll.Lists {
  public class Semana {
    public Dictionary<int, string> Data;

    public Semana() {
      Data = new Dictionary<int, string> {
        { 1, "Domingo" },
        { 2, "Segunda-feira" },
        { 3, "Terça-feira" },
        { 4, "Quarta-feira" },
        { 5, "Quinta-feira" },
        { 6, "Sexta-feira" },
        { 7, "Sábado" }
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
