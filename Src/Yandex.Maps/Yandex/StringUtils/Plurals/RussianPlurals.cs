// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.Plurals.RussianPlurals
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.StringUtils.Plurals.Interfaces;

namespace Yandex.StringUtils.Plurals
{
  internal class RussianPlurals : IPlurals
  {
    public string GetPluralForm(string[] forms, long count)
    {
      int index = count % 10L != 1L || count % 100L == 11L ? (count % 10L < 2L || count % 10L > 4L || count % 100L >= 10L && count % 100L < 20L ? 2 : 1) : 0;
      return forms[index];
    }
  }
}
