// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.Plurals.RussianPlurals
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using Yandex.StringUtils.Plurals.Interfaces;

namespace Yandex.StringUtils.Plurals
{
  public class RussianPlurals : IPlurals
  {
    public string GetPluralForm(string[] forms, long count)
    {
      int index = count % 10L != 1L || count % 100L == 11L ? (count % 10L < 2L || count % 10L > 4L || count % 100L >= 10L && count % 100L < 20L ? 2 : 1) : 0;
      return forms[index];
    }
  }
}
