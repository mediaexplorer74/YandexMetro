// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.CropUtil
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

namespace Yandex.StringUtils
{
  public class CropUtil
  {
    public static string CropString(string text, int maxChars, int maxOffset)
    {
      if (string.IsNullOrEmpty(text) || text.Length <= maxChars)
        return text;
      int length = text.IndexOf(" ", maxChars);
      if (length > 0)
        return text.Substring(0, length) + "…";
      return text.Length >= maxChars + maxOffset ? text.Substring(0, maxChars) + "…" : text;
    }
  }
}
