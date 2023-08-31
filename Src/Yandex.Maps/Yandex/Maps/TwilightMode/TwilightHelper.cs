// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.TwilightMode.TwilightHelper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Maps.TwilightMode
{
  internal static class TwilightHelper
  {
    public static bool IsNight(
      DateTime now,
      double longitude,
      double latitude,
      double longitudeTimeZone,
      bool useSummerTime)
    {
      DateTime sunRise = TwilightHelper.CalculateSunRise(now, latitude, longitude, longitudeTimeZone, useSummerTime);
      DateTime sunSet = TwilightHelper.CalculateSunSet(now, latitude, longitude, longitudeTimeZone, useSummerTime);
      return now < sunRise || now > sunSet;
    }

    public static bool IsTwilight(
      DateTime now,
      double longitude,
      double latitude,
      double longitudeTimeZone,
      bool useSummerTime)
    {
      DateTime sunRise = TwilightHelper.CalculateSunRise(now, latitude, longitude, longitudeTimeZone, useSummerTime, 0.44);
      DateTime sunSet = TwilightHelper.CalculateSunSet(now, latitude, longitude, longitudeTimeZone, useSummerTime, 0.44);
      return now < sunRise || now > sunSet;
    }

    public static bool IsTwilight2(
      DateTime now,
      double longitude,
      double latitude,
      double longitudeTimeZone,
      bool useSummerTime)
    {
      DateTime sunRise = TwilightHelper.CalculateSunRise(now, latitude, longitude, longitudeTimeZone, useSummerTime, 0.24);
      DateTime sunSet = TwilightHelper.CalculateSunSet(now, latitude, longitude, longitudeTimeZone, useSummerTime, 0.24);
      if (sunRise.TimeOfDay > sunSet.TimeOfDay)
        return false;
      return now < sunRise || now > sunSet;
    }

    public static DateTime CalculateSunRise(
      DateTime date,
      double latitude,
      double longitude,
      double longituteTimeZone,
      bool useSummerTime,
      double additionalSunAngle = 0.0)
    {
      double radian = TwilightHelper.ConvertDegreeToRadian(latitude);
      int dayNumber = TwilightHelper.ExtractDayNumber(date);
      double differenceSunAndLocalTime = TwilightHelper.CalculateDifferenceSunAndLocalTime(dayNumber, longitude, longituteTimeZone, useSummerTime);
      int sunRiseInternal = TwilightHelper.CalculateSunRiseInternal(TwilightHelper.LimitTanSunPosition(TwilightHelper.CalculateTanSunPosition(TwilightHelper.CalculateDeclination(dayNumber), radian) + additionalSunAngle), differenceSunAndLocalTime);
      return TwilightHelper.CreateDateTime(date, sunRiseInternal);
    }

    public static DateTime CalculateSunSet(
      DateTime date,
      double latitude,
      double longitude,
      double longituteTimeZone,
      bool useSummerTime,
      double additionalSunAngle = 0.0)
    {
      double radian = TwilightHelper.ConvertDegreeToRadian(latitude);
      int dayNumber = TwilightHelper.ExtractDayNumber(date);
      double differenceSunAndLocalTime = TwilightHelper.CalculateDifferenceSunAndLocalTime(dayNumber, longitude, longituteTimeZone, useSummerTime);
      int sunSetInternal = TwilightHelper.CalculateSunSetInternal(TwilightHelper.LimitTanSunPosition(TwilightHelper.CalculateTanSunPosition(TwilightHelper.CalculateDeclination(dayNumber), radian) + additionalSunAngle), differenceSunAndLocalTime);
      return TwilightHelper.CreateDateTime(date, sunSetInternal);
    }

    private static double CalculateDeclination(int numberOfDaysSinceFirstOfJanuary) => Math.Asin(-0.39795 * Math.Cos(2.0 * Math.PI * ((double) numberOfDaysSinceFirstOfJanuary + 10.0) / 365.0));

    private static int ExtractDayNumber(DateTime dateTime) => dateTime.DayOfYear;

    private static DateTime CreateDateTime(DateTime dateTime, int timeInMinutes) => dateTime.Date + TimeSpan.FromMinutes((double) timeInMinutes);

    private static int CalculateSunRiseInternal(
      double tanSunPosition,
      double differenceSunAndLocalTime)
    {
      return (int) (720.0 - 720.0 / Math.PI * Math.Acos(-tanSunPosition) - differenceSunAndLocalTime);
    }

    private static int CalculateSunSetInternal(
      double tanSunPosition,
      double differenceSunAndLocalTime)
    {
      return (int) (720.0 + 720.0 / Math.PI * Math.Acos(-tanSunPosition) - differenceSunAndLocalTime);
    }

    private static double CalculateTanSunPosition(
      double declanationOfTheSun,
      double latituteInRadians)
    {
      return TwilightHelper.LimitTanSunPosition(TwilightHelper.CalculateSinSunPosition(declanationOfTheSun, latituteInRadians) / TwilightHelper.CalculateCosSunPosition(declanationOfTheSun, latituteInRadians));
    }

    private static double CalculateCosSunPosition(
      double declanationOfTheSun,
      double latituteInRadians)
    {
      return Math.Cos(latituteInRadians) * Math.Cos(declanationOfTheSun);
    }

    private static double CalculateSinSunPosition(
      double declanationOfTheSun,
      double latituteInRadians)
    {
      return Math.Sin(latituteInRadians) * Math.Sin(declanationOfTheSun);
    }

    private static double CalculateDifferenceSunAndLocalTime(
      int dayNumberOfDateTime,
      double longitude,
      double longituteTimeZone,
      bool useSummerTime)
    {
      double differenceSunAndLocalTime = 7.95204 * Math.Sin(0.01768 * (double) dayNumberOfDateTime + 3.03217) + 9.98906 * Math.Sin(0.03383 * (double) dayNumberOfDateTime + 3.4687) + (longitude - longituteTimeZone) * 4.0;
      if (useSummerTime)
        differenceSunAndLocalTime -= 60.0;
      return differenceSunAndLocalTime;
    }

    private static double LimitTanSunPosition(double tanSunPosition)
    {
      if (tanSunPosition < -1.0)
        tanSunPosition = -1.0;
      if (tanSunPosition > 1.0)
        tanSunPosition = 1.0;
      return tanSunPosition;
    }

    private static double ConvertDegreeToRadian(double degree) => degree * Math.PI / 180.0;
  }
}
