// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Behaviors.TriggerAnimationBehavior
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Y.UI.Common.Behaviors
{
  public class TriggerAnimationBehavior : Behavior<FrameworkElement>
  {
    public static readonly DependencyProperty PropertyToMonitorProperty = DependencyProperty.Register(nameof (PropertyToMonitor), typeof (object), typeof (TriggerAnimationBehavior), new PropertyMetadata(new PropertyChangedCallback(TriggerAnimationBehavior.OnPropertyToMonitorChanged)));
    public static readonly DependencyProperty AnimationToBeginProperty = DependencyProperty.Register(nameof (AnimationToBegin), typeof (Storyboard), typeof (TriggerAnimationBehavior), (PropertyMetadata) null);
    public static readonly DependencyProperty AnimationBackToBeginProperty = DependencyProperty.Register(nameof (AnimationBackToBegin), typeof (Storyboard), typeof (TriggerAnimationBehavior), (PropertyMetadata) null);
    public static readonly DependencyProperty ValueToMonitorProperty = DependencyProperty.Register(nameof (ValueToMonitor), typeof (object), typeof (TriggerAnimationBehavior), (PropertyMetadata) null);

    private static void OnPropertyToMonitorChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      TriggerAnimationBehavior animationBehavior = (TriggerAnimationBehavior) o;
      object valueToMonitor = animationBehavior.ValueToMonitor;
      (valueToMonitor == null || valueToMonitor.Equals(e.NewValue) ? animationBehavior.AnimationToBegin : animationBehavior.AnimationBackToBegin)?.Begin();
    }

    public object PropertyToMonitor
    {
      get => this.GetValue(TriggerAnimationBehavior.PropertyToMonitorProperty);
      set => this.SetValue(TriggerAnimationBehavior.PropertyToMonitorProperty, value);
    }

    public Storyboard AnimationToBegin
    {
      get => (Storyboard) this.GetValue(TriggerAnimationBehavior.AnimationToBeginProperty);
      set => this.SetValue(TriggerAnimationBehavior.AnimationToBeginProperty, (object) value);
    }

    public Storyboard AnimationBackToBegin
    {
      get => (Storyboard) this.GetValue(TriggerAnimationBehavior.AnimationBackToBeginProperty);
      set => this.SetValue(TriggerAnimationBehavior.AnimationBackToBeginProperty, (object) value);
    }

    public object ValueToMonitor
    {
      get => this.GetValue(TriggerAnimationBehavior.ValueToMonitorProperty);
      set => this.SetValue(TriggerAnimationBehavior.ValueToMonitorProperty, value);
    }
  }
}
