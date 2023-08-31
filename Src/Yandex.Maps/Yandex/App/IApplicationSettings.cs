// Decompiled with JetBrains decompiler
// Type: Yandex.App.IApplicationSettings
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

namespace Yandex.App
{
  internal interface IApplicationSettings
  {
    bool TryGetValue<T>(string key, out T value);

    object this[string key] { get; set; }

    void Add(string key, object value);

    bool Remove(string key);

    bool ContainsKey(string key);
  }
}
