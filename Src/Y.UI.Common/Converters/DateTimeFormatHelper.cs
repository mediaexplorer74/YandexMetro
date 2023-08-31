// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Converters.DateTimeFormatHelper
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Y.UI.Common.Resources.RelativeTimeConverter;

namespace Y.UI.Common.Converters
{
  internal static class DateTimeFormatHelper
  {
    private const double Hour = 60.0;
    private const double Day = 1440.0;
    private const string SingleMeridiemDesignator = "t";
    private const string DoubleMeridiemDesignator = "tt";
    private static DateTimeFormatInfo formatInfo_GetSuperShortTime = (DateTimeFormatInfo) null;
    private static DateTimeFormatInfo formatInfo_GetMonthAndDay = (DateTimeFormatInfo) null;
    private static DateTimeFormatInfo formatInfo_GetShortTime = (DateTimeFormatInfo) null;
    private static object lock_GetSuperShortTime = new object();
    private static object lock_GetMonthAndDay = new object();
    private static object lock_GetShortTime = new object();
    private static readonly Regex rxMonthAndDay = new Regex("(d{1,2}[^A-Za-z]M{1,3})|(M{1,3}[^A-Za-z]d{1,2})");
    private static readonly Regex rxSeconds = new Regex("([^A-Za-z]s{1,2})");

    public static int GetRelativeDayOfWeek(DateTime dt) => (dt.DayOfWeek - ControlResources.Culture.DateTimeFormat.FirstDayOfWeek + 7) % 7;

    public static bool IsFutureDateTime(DateTime relative, DateTime given) => relative < given;

    public static bool IsAnOlderYear(DateTime relative, DateTime given) => relative.Year > given.Year;

    public static bool IsAnOlderWeek(DateTime relative, DateTime given) => DateTimeFormatHelper.IsAtLeastOneWeekOld(relative, given) || DateTimeFormatHelper.GetRelativeDayOfWeek(given) > DateTimeFormatHelper.GetRelativeDayOfWeek(relative);

    public static bool IsAtLeastOneWeekOld(DateTime relative, DateTime given) => (double) (int) (relative - given).TotalMinutes >= 10080.0;

    public static bool IsPastDayOfWeek(DateTime relative, DateTime given) => DateTimeFormatHelper.GetRelativeDayOfWeek(relative) > DateTimeFormatHelper.GetRelativeDayOfWeek(given);

    public static bool IsPastDayOfWeekWithWindow(DateTime relative, DateTime given) => DateTimeFormatHelper.IsPastDayOfWeek(relative, given) && (double) (int) (relative - given).TotalMinutes > 180.0;

    public static bool IsCurrentCultureJapanese() => ControlResources.Culture.Name.StartsWith("ja", StringComparison.OrdinalIgnoreCase);

    public static bool IsCurrentCultureKorean() => ControlResources.Culture.Name.StartsWith("ko", StringComparison.OrdinalIgnoreCase);

    public static bool IsCurrentCultureTurkish() => ControlResources.Culture.Name.StartsWith("tr", StringComparison.OrdinalIgnoreCase);

    public static bool IsCurrentCultureHungarian() => ControlResources.Culture.Name.StartsWith("hu", StringComparison.OrdinalIgnoreCase);

    public static bool IsCurrentUICultureFrench() => ControlResources.Culture.Name.Equals("fr-FR", StringComparison.Ordinal);

    public static string GetAbbreviatedDay(DateTime dt) => DateTimeFormatHelper.IsCurrentCultureJapanese() || DateTimeFormatHelper.IsCurrentCultureKorean() ? "(" + dt.ToString("ddd", (IFormatProvider) CultureInfo.CurrentCulture) + ")" : dt.ToString("ddd", (IFormatProvider) CultureInfo.CurrentCulture);

    public static string GetSuperShortTime(DateTime dt)
    {
      if (DateTimeFormatHelper.formatInfo_GetSuperShortTime == null)
      {
        lock (DateTimeFormatHelper.lock_GetSuperShortTime)
        {
          StringBuilder stringBuilder = new StringBuilder(string.Empty);
          DateTimeFormatHelper.formatInfo_GetSuperShortTime = (DateTimeFormatInfo) CultureInfo.CurrentCulture.DateTimeFormat.Clone();
          stringBuilder.Append(DateTimeFormatHelper.formatInfo_GetSuperShortTime.LongTimePattern);
          string oldValue = DateTimeFormatHelper.rxSeconds.Match(stringBuilder.ToString()).Value;
          stringBuilder.Replace(" ", string.Empty);
          stringBuilder.Replace(oldValue, string.Empty);
          if (!DateTimeFormatHelper.IsCurrentCultureJapanese() && !DateTimeFormatHelper.IsCurrentCultureKorean() && !DateTimeFormatHelper.IsCurrentCultureHungarian())
            stringBuilder.Replace("tt", "t");
          DateTimeFormatHelper.formatInfo_GetSuperShortTime.ShortTimePattern = stringBuilder.ToString();
        }
      }
      return dt.ToString("t", (IFormatProvider) DateTimeFormatHelper.formatInfo_GetSuperShortTime).ToLowerInvariant();
    }

    public static string GetMonthAndDay(DateTime dt)
    {
      if (DateTimeFormatHelper.formatInfo_GetMonthAndDay == null)
      {
        lock (DateTimeFormatHelper.lock_GetMonthAndDay)
        {
          StringBuilder stringBuilder = new StringBuilder(string.Empty);
          DateTimeFormatHelper.formatInfo_GetMonthAndDay = (DateTimeFormatInfo) CultureInfo.CurrentCulture.DateTimeFormat.Clone();
          stringBuilder.Append(DateTimeFormatHelper.rxMonthAndDay.Match(DateTimeFormatHelper.formatInfo_GetMonthAndDay.ShortDatePattern).Value);
          if (stringBuilder.ToString().Contains("."))
            stringBuilder.Append(".");
          DateTimeFormatHelper.formatInfo_GetMonthAndDay.ShortDatePattern = stringBuilder.ToString();
        }
      }
      return dt.ToString("d", (IFormatProvider) DateTimeFormatHelper.formatInfo_GetMonthAndDay);
    }

    public static string GetShortDate(DateTime dt) => dt.ToString("d", (IFormatProvider) CultureInfo.CurrentCulture);

    public static string GetShortTime(DateTime dt)
    {
      if (DateTimeFormatHelper.formatInfo_GetShortTime == null)
      {
        lock (DateTimeFormatHelper.lock_GetShortTime)
        {
          StringBuilder stringBuilder = new StringBuilder(string.Empty);
          DateTimeFormatHelper.formatInfo_GetShortTime = (DateTimeFormatInfo) CultureInfo.CurrentCulture.DateTimeFormat.Clone();
          stringBuilder.Append(DateTimeFormatHelper.formatInfo_GetSuperShortTime.LongTimePattern);
          string oldValue = DateTimeFormatHelper.rxSeconds.Match(stringBuilder.ToString()).Value;
          stringBuilder.Replace(oldValue, string.Empty);
          DateTimeFormatHelper.formatInfo_GetShortTime.ShortTimePattern = stringBuilder.ToString();
        }
      }
      return dt.ToString("t", (IFormatProvider) DateTimeFormatHelper.formatInfo_GetShortTime);
    }
  }
}
