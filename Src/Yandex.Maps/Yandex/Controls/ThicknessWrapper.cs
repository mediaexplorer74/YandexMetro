// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.ThicknessWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Reflection;
using System.Windows;

namespace Yandex.Controls
{
  internal class ThicknessWrapper : FrameworkElement
  {
    public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(nameof (Target), typeof (FrameworkElement), typeof (ThicknessWrapper), new PropertyMetadata((object) null, new PropertyChangedCallback(ThicknessWrapper.OnTargetChanged)));
    public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(nameof (PropertyName), typeof (string), typeof (ThicknessWrapper), new PropertyMetadata((object) "Margin"));
    public static readonly DependencyProperty SideProperty = DependencyProperty.Register(nameof (Side), typeof (ThicknessAnimationSide), typeof (ThicknessWrapper), new PropertyMetadata((object) ThicknessAnimationSide.Left, new PropertyChangedCallback(ThicknessWrapper.OnSideChanged)));
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (ThicknessWrapper), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ThicknessWrapper.OnValueChanged)));

    public FrameworkElement Target
    {
      get => (FrameworkElement) ((DependencyObject) this).GetValue(ThicknessWrapper.TargetProperty);
      set => ((DependencyObject) this).SetValue(ThicknessWrapper.TargetProperty, (object) value);
    }

    private static void OnTargetChanged(
      DependencyObject source,
      DependencyPropertyChangedEventArgs args)
    {
      ((ThicknessWrapper) source).UpdateMargin();
    }

    public string PropertyName
    {
      get => (string) ((DependencyObject) this).GetValue(ThicknessWrapper.PropertyNameProperty);
      set => ((DependencyObject) this).SetValue(ThicknessWrapper.PropertyNameProperty, (object) value);
    }

    public ThicknessAnimationSide Side
    {
      get => (ThicknessAnimationSide) ((DependencyObject) this).GetValue(ThicknessWrapper.SideProperty);
      set => ((DependencyObject) this).SetValue(ThicknessWrapper.SideProperty, (object) value);
    }

    private static void OnSideChanged(
      DependencyObject source,
      DependencyPropertyChangedEventArgs args)
    {
      ((ThicknessWrapper) source).UpdateMargin();
    }

    public double Value
    {
      get => (double) ((DependencyObject) this).GetValue(ThicknessWrapper.ValueProperty);
      set => ((DependencyObject) this).SetValue(ThicknessWrapper.ValueProperty, (object) value);
    }

    private static void OnValueChanged(
      DependencyObject source,
      DependencyPropertyChangedEventArgs args)
    {
      ((ThicknessWrapper) source).UpdateMargin();
    }

    private void UpdateMargin()
    {
      if (this.Target == null)
        return;
      PropertyInfo property = this.Target.GetType().GetProperty(this.PropertyName);
      Thickness thickness1 = (Thickness) property.GetValue((object) this.Target, (object[]) null);
      Thickness thickness2 = new Thickness(this.CalculateThickness(ThicknessAnimationSide.Left, thickness1.Left), this.CalculateThickness(ThicknessAnimationSide.Top, thickness1.Top), this.CalculateThickness(ThicknessAnimationSide.Right, thickness1.Right), this.CalculateThickness(ThicknessAnimationSide.Bottom, thickness1.Bottom));
      property.SetValue((object) this.Target, (object) thickness2, (object[]) null);
    }

    private double CalculateThickness(ThicknessAnimationSide sideToCalculate, double currentValue) => (this.Side & sideToCalculate) != sideToCalculate ? currentValue : this.Value;
  }
}
