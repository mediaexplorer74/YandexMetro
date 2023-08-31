// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.Common
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.StringUtils
{
  internal static class Common
  {
    public const string WhiteSpace = " ";

    public static string AppendWhiteSpace(this string text) => string.IsNullOrEmpty(text) ? text : text.Trim() + " ";

    public static string UppercaseFirst(this string s) => string.IsNullOrEmpty(s) ? string.Empty : char.ToUpper(s[0]).ToString() + s.Substring(1);
  }
}
