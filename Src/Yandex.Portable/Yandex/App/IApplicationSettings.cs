// Decompiled with JetBrains decompiler
// Type: Yandex.App.IApplicationSettings
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

namespace Yandex.App
{
  public interface IApplicationSettings
  {
    bool TryGetValue<T>(string key, out T value);

    object this[string key] { get; set; }

    void Add(string key, object value);

    bool Remove(string key);

    bool ContainsKey(string key);
  }
}
