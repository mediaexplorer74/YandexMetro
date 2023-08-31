// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.Interfaces.IDistanceFormatterUtil
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

namespace Yandex.StringUtils.Interfaces
{
  public interface IDistanceFormatterUtil
  {
    string GetDistanceString(double distance, bool almostEquals);

    AnnotatedValue<double> GetDistance(double distance);
  }
}
