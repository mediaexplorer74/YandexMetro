// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Interfaces.IContentCompressor
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

namespace Yandex.WebUtils.Interfaces
{
  public interface IContentCompressor
  {
    string ContentType { get; }

    byte[] Compress(string value);
  }
}
