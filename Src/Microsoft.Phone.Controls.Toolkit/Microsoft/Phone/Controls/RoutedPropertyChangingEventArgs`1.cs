// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.RoutedPropertyChangingEventArgs`1
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Properties;
using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
  public class RoutedPropertyChangingEventArgs<T> : RoutedEventArgs
  {
    private bool _cancel;

    public DependencyProperty Property { get; private set; }

    public T OldValue { get; private set; }

    public T NewValue { get; set; }

    public bool IsCancelable { get; private set; }

    public bool Cancel
    {
      get => this._cancel;
      set
      {
        if (this.IsCancelable)
          this._cancel = value;
        else if (value)
          throw new InvalidOperationException(Resources.RoutedPropertyChangingEventArgs_CancelSet_InvalidOperation);
      }
    }

    public bool InCoercion { get; set; }

    public RoutedPropertyChangingEventArgs(
      DependencyProperty property,
      T oldValue,
      T newValue,
      bool isCancelable)
    {
      this.Property = property;
      this.OldValue = oldValue;
      this.NewValue = newValue;
      this.IsCancelable = isCancelable;
      this.Cancel = false;
    }
  }
}
