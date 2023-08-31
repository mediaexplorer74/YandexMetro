// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.TurnstileTransition
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Windows;

namespace Microsoft.Phone.Controls
{
  public class TurnstileTransition : TransitionElement
  {
    public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof (Mode), typeof (TurnstileTransitionMode), typeof (TurnstileTransition), (PropertyMetadata) null);

    public TurnstileTransitionMode Mode
    {
      get => (TurnstileTransitionMode) this.GetValue(TurnstileTransition.ModeProperty);
      set => this.SetValue(TurnstileTransition.ModeProperty, (object) value);
    }

    public override ITransition GetTransition(UIElement element) => Transitions.Turnstile(element, this.Mode);
  }
}
