// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Utility.ResourcesHelper
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Windows;

namespace Y.UI.Common.Utility
{
  public static class ResourcesHelper
  {
    public static T Get<T>(string key) => (T) Application.Current.Resources[(object) key];
  }
}
