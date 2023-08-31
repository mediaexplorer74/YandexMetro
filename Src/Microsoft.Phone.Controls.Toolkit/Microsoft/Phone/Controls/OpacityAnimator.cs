// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.OpacityAnimator
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
  internal sealed class OpacityAnimator
  {
    private static readonly PropertyPath OpacityPropertyPath = new PropertyPath("Opacity", new object[0]);
    private readonly Storyboard _sbRunning = new Storyboard();
    private readonly DoubleAnimation _daRunning = new DoubleAnimation();
    private bool _suppressChangeNotification;
    private Action _oneTimeAction;

    public OpacityAnimator(UIElement target)
    {
      ((Timeline) this._sbRunning).Completed += new EventHandler(this.OnCompleted);
      ((PresentationFrameworkCollection<Timeline>) this._sbRunning.Children).Add((Timeline) this._daRunning);
      Storyboard.SetTarget((Timeline) this._daRunning, (DependencyObject) target);
      Storyboard.SetTargetProperty((Timeline) this._daRunning, OpacityAnimator.OpacityPropertyPath);
    }

    public void GoTo(double targetOpacity, Duration duration) => this.GoTo(targetOpacity, duration, (IEasingFunction) null, (Action) null);

    public void GoTo(double targetOpacity, Duration duration, Action completionAction) => this.GoTo(targetOpacity, duration, (IEasingFunction) null, completionAction);

    public void GoTo(
      double targetOpacity,
      Duration duration,
      IEasingFunction easingFunction,
      Action completionAction)
    {
      this._daRunning.To = new double?(targetOpacity);
      ((Timeline) this._daRunning).Duration = duration;
      this._daRunning.EasingFunction = easingFunction;
      this._sbRunning.Begin();
      this._suppressChangeNotification = true;
      this._sbRunning.SeekAlignedToLastTick(TimeSpan.Zero);
      this._oneTimeAction = completionAction;
    }

    private void OnCompleted(object sender, EventArgs e)
    {
      Action oneTimeAction = this._oneTimeAction;
      if (oneTimeAction != null)
      {
        this._oneTimeAction = (Action) null;
        oneTimeAction();
      }
      if (this._suppressChangeNotification)
        return;
      this._suppressChangeNotification = false;
    }

    public static void EnsureAnimator(UIElement targetElement, ref OpacityAnimator animator)
    {
      if (animator == null)
        animator = new OpacityAnimator(targetElement);
      if (animator == null)
        throw new InvalidOperationException("The animation system could not be prepared for the target element.");
    }
  }
}
