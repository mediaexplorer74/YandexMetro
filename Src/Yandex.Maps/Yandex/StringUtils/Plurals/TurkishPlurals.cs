// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.Plurals.TurkishPlurals
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.StringUtils.Plurals.Interfaces;

namespace Yandex.StringUtils.Plurals
{
  internal class TurkishPlurals : IPlurals
  {
    public string GetPluralForm(string[] forms, long count) => forms[0];
  }
}
