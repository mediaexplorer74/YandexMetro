// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.Common
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

namespace Yandex.StringUtils
{
  public static class Common
  {
    public const string WhiteSpace = " ";

    public static string AppendWhiteSpace(this string text) => string.IsNullOrEmpty(text) ? text : text.Trim() + " ";

    public static string UppercaseFirst(this string s) => string.IsNullOrEmpty(s) ? string.Empty : char.ToUpper(s[0]).ToString() + s.Substring(1);
  }
}
