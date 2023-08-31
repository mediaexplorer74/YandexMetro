// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.CropUtil
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.StringUtils
{
  internal class CropUtil
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
