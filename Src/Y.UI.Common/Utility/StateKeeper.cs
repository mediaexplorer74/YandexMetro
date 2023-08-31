// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Utility.StateKeeper
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using Microsoft.Phone.Shell;
using System.Collections.Generic;

namespace Y.UI.Common.Utility
{
  public static class StateKeeper
  {
    public static void Save<T>(T state, string key)
    {
      IDictionary<string, object> state1 = PhoneApplicationService.Current.State;
      if (state1.ContainsKey(key))
        state1.Remove(key);
      string str = Json.Serialize<T>(state);
      state1.Add(key, (object) str);
    }

    public static T Load<T>(string key)
    {
      IDictionary<string, object> state = PhoneApplicationService.Current.State;
      return state.ContainsKey(key) ? Json.Deserialize<T>(state[key] as string) : default (T);
    }
  }
}
