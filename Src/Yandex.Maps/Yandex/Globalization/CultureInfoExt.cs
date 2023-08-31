// Decompiled with JetBrains decompiler
// Type: Yandex.Globalization.CultureInfoExt
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Globalization;

namespace Yandex.Globalization
{
  internal static class CultureInfoExt
  {
    public static string GetSpecificName(this CultureInfo culture) => culture.IsNeutralCulture ? culture.TextInfo.CultureName : culture.Name;
  }
}
