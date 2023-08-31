// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Behaviors.VisualStateBehavior
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Yandex.Controls.Behaviors
{
  internal class VisualStateBehavior : Behavior<Control>
  {
    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof (State), typeof (object), typeof (VisualStateBehavior), new PropertyMetadata(new PropertyChangedCallback(VisualStateBehavior.StatePropertyChangedCallback)));

    public object State
    {
      get => this.GetValue(VisualStateBehavior.StateProperty);
      set => this.SetValue(VisualStateBehavior.StateProperty, value);
    }

    private static void StatePropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == null)
        return;
      Control associatedObject = ((Behavior<Control>) d).AssociatedObject;
      if (associatedObject == null)
        return;
      VisualStateManager.GoToState(associatedObject, e.NewValue.ToString(), true);
    }
  }
}
