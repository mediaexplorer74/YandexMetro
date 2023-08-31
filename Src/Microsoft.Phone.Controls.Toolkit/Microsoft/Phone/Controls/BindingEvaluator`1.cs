// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.BindingEvaluator`1
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Windows;
using System.Windows.Data;

namespace Microsoft.Phone.Controls
{
  internal class BindingEvaluator<T> : FrameworkElement
  {
    private Binding _binding;
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (T), typeof (BindingEvaluator<T>), new PropertyMetadata((object) default (T)));

    public T Value
    {
      get => (T) ((DependencyObject) this).GetValue(BindingEvaluator<T>.ValueProperty);
      set => ((DependencyObject) this).SetValue(BindingEvaluator<T>.ValueProperty, (object) value);
    }

    public Binding ValueBinding
    {
      get => this._binding;
      set
      {
        this._binding = value;
        this.SetBinding(BindingEvaluator<T>.ValueProperty, this._binding);
      }
    }

    public BindingEvaluator()
    {
    }

    public BindingEvaluator(Binding binding) => this.SetBinding(BindingEvaluator<T>.ValueProperty, binding);

    public void ClearDataContext() => this.DataContext = (object) null;

    public T GetDynamicValue(object o, bool clearDataContext)
    {
      this.DataContext = o;
      T dynamicValue = this.Value;
      if (clearDataContext)
        this.DataContext = (object) null;
      return dynamicValue;
    }

    public T GetDynamicValue(object o)
    {
      this.DataContext = o;
      return this.Value;
    }
  }
}
