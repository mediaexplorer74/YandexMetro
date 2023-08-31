// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Behavior.AttachedPropertyBindingProvider
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Windows.Data;
using System.Windows.Markup;

namespace Yandex.Maps.Behavior
{
  internal static class AttachedPropertyBindingProvider
  {
    public static Binding GetBinding(string propertyName) => (Binding) XamlReader.Load("<Binding xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:ymc=\"clr-namespace:Yandex.Maps;assembly=Yandex.Maps\" Path=\"(ymc:" + propertyName + ")\" />");
  }
}
