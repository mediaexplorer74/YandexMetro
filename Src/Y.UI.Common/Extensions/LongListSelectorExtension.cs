// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Extensions.LongListSelectorExtension
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Y.UI.Common.Extensions
{
  public static class LongListSelectorExtension
  {
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof (ICommand), typeof (LongListSelectorExtension), new PropertyMetadata((object) null, new PropertyChangedCallback(LongListSelectorExtension.OnCommandChanged)));

    public static ICommand GetCommand(LongListSelector selector) => (ICommand) ((DependencyObject) selector).GetValue(LongListSelectorExtension.CommandProperty);

    public static void SetCommand(LongListSelector selector, ICommand value) => ((DependencyObject) selector).SetValue(LongListSelectorExtension.CommandProperty, (object) value);

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is LongListSelector longListSelector))
        throw new ArgumentException("You must set the Command attached property on an element that derives from LongListSelector.");
      if (e.OldValue is ICommand)
        longListSelector.SelectionChanged -= new SelectionChangedEventHandler(LongListSelectorExtension.OnSelectionChanged);
      if (!(e.NewValue is ICommand))
        return;
      longListSelector.SelectionChanged += new SelectionChangedEventHandler(LongListSelectorExtension.OnSelectionChanged);
    }

    private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      LongListSelector selector = sender as LongListSelector;
      LongListSelectorExtension.GetCommand(selector)?.Execute(selector.SelectedItem);
    }
  }
}
