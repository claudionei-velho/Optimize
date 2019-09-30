namespace Dto.Models {
  public static class CustomCalendar {
    public static readonly int DaysPerWeek = 7;
    public static readonly int WeeksPerYear = 52;
    public static readonly int MonthsPerYear = 12;

    public static int DaysPerYear(int year) {
      return (!IsLeapYear(year)) ? 365 : 366;
    }

    public static int WorkdaysPerYear(int year) {
      return DaysPerYear(year) - (WeeksPerYear * 2);
    }

    private static bool IsLeapYear(int year) {
      return ((year % 400) == 0 || (year % 100) != 0) && (year % 4) == 0;
    }
  }
}
