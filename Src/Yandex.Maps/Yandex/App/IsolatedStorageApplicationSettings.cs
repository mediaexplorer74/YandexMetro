// Decompiled with JetBrains decompiler
// Type: Yandex.App.IsolatedStorageApplicationSettings
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.IO.IsolatedStorage;
using Yandex.DevUtils;

namespace Yandex.App
{
  internal class IsolatedStorageApplicationSettings : IApplicationSettings
  {
    public bool TryGetValue(string key, out object value)
    {
      if (!DesignerProperties.IsInDesignMode)
        return IsolatedStorageSettings.ApplicationSettings.TryGetValue<object>(key, ref value);
      value = (object) null;
      return false;
    }

    public bool TryGetValue<T>(string key, out T value)
    {
      object obj;
      bool flag = this.TryGetValue(key, out obj);
      value = !flag ? default (T) : (T) obj;
      return flag;
    }

    public object this[string key]
    {
      get => DesignerProperties.IsInDesignMode ? (object) null : IsolatedStorageSettings.ApplicationSettings[key];
      set
      {
        if (DesignerProperties.IsInDesignMode)
          return;
        IsolatedStorageSettings.ApplicationSettings[key] = value;
      }
    }

    public void Add(string key, object value)
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      IsolatedStorageSettings.ApplicationSettings.Add(key, value);
    }

    public bool Remove(string key) => !DesignerProperties.IsInDesignMode && IsolatedStorageSettings.ApplicationSettings.Remove(key);

    public bool ContainsKey(string key) => !DesignerProperties.IsInDesignMode && IsolatedStorageSettings.ApplicationSettings.Contains(key);
  }
}
