// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.StateManager
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Windows;
using System.Windows.Controls;

namespace Y.UI.Common
{
  public class StateManager : DependencyObject
  {
    public static readonly DependencyProperty VisualStatePropertyProperty = DependencyProperty.RegisterAttached("VisualStateProperty", typeof (string), typeof (StateManager), new PropertyMetadata((PropertyChangedCallback) ((s, e) =>
    {
      string newValue = (string) e.NewValue;
      if (string.IsNullOrEmpty(newValue))
        return;
      if (!(s is Control control2))
        throw new InvalidOperationException("This attached property only supports types derived from Control.");
      VisualStateManager.GoToState(control2, newValue, true);
    })));

    public static string GetVisualStateProperty(DependencyObject obj) => (string) obj.GetValue(StateManager.VisualStatePropertyProperty);

    public static void SetVisualStateProperty(DependencyObject obj, string value) => obj.SetValue(StateManager.VisualStatePropertyProperty, (object) value);
  }
}
