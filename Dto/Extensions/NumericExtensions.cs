using System;

namespace Dto.Extensions {
  public static class NumericExtensions {
    public static int SafeDivision(int? Numerator, int Denominator) {
      try {
        return (Numerator ?? 0) / Denominator;
      }
      catch (DivideByZeroException) {
        return 0;
      }
    }

    public static decimal SafeDivision(decimal? Numerator, decimal Denominator) {
      try {
        return (Numerator ?? 0) / Denominator;
      }
      catch (DivideByZeroException) {
        return 0;
      }
    }
  }
}
