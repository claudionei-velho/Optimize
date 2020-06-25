using System.Collections.Generic;

namespace Dto.Lists {
  public static class Semana {
    public static IDictionary<int, string> Items = new Dictionary<int, string> {
        { 1, "Domingo" },
        { 2, "Segunda-feira" },
        { 3, "Terça-feira" },
        { 4, "Quarta-feira" },
        { 5, "Quinta-feira" },
        { 6, "Sexta-feira" },
        { 7, "Sábado" }
    };
  }
}
