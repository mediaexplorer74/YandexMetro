// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.TransformAnimator
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Microsoft.Phone.Controls
{
  internal sealed class TransformAnimator
  {
    private static readonly PropertyPath TranslateXPropertyPath = new PropertyPath("X", new object[0]);
    private readonly Storyboard _sbRunning = new Storyboard();
    private readonly DoubleAnimation _daRunning = new DoubleAnimation();
    private TranslateTransform _transform;
    private Action _oneTimeAction;

    public TransformAnimator(TranslateTransform translateTransform)
    {
      this._transform = translateTransform;
      ((Timeline) this._sbRunning).Completed += new EventHandler(this.OnCompleted);
      ((PresentationFrameworkCollection<Timeline>) this._sbRunning.Children).Add((Timeline) this._daRunning);
      Storyboard.SetTarget((Timeline) this._daRunning, (DependencyObject) this._transform);
      Storyboard.SetTargetProperty((Timeline) this._daRunning, TransformAnimator.TranslateXPropertyPath);
    }

    public double CurrentOffset => this._transform.X;

    public void GoTo(double targetOffset, Duration duration) => this.GoTo(targetOffset, duration, (IEasingFunction) null, (Action) null);

    public void GoTo(double targetOffset, Duration duration, Action completionAction) => this.GoTo(targetOffset, duration, (IEasingFunction) null, completionAction);

    public void GoTo(double targetOffset, Duration duration, IEasingFunction easingFunction) => this.GoTo(targetOffset, duration, easingFunction, (Action) null);

    public void GoTo(
      double targetOffset,
      Duration duration,
      IEasingFunction easingFunction,
      Action completionAction)
    {
      this._daRunning.To = new double?(targetOffset);
      ((Timeline) this._daRunning).Duration = duration;
      this._daRunning.EasingFunction = easingFunction;
      this._sbRunning.Begin();
      this._sbRunning.SeekAlignedToLastTick(TimeSpan.Zero);
      this._oneTimeAction = completionAction;
    }

    public void UpdateEasingFunction(IEasingFunction ease)
    {
      if (this._daRunning == null || this._daRunning.EasingFunction == ease)
        return;
      this._daRunning.EasingFunction = ease;
    }

    public void UpdateDuration(Duration duration)
    {
      if (this._daRunning == null)
        return;
      ((Timeline) this._daRunning).Duration = duration;
    }

    private void OnCompleted(object sender, EventArgs e)
    {
      Action oneTimeAction = this._oneTimeAction;
      if (oneTimeAction == null || this._sbRunning.GetCurrentState() == null)
        return;
      this._oneTimeAction = (Action) null;
      oneTimeAction();
    }

    public static void EnsureAnimator(
      FrameworkElement targetElement,
      ref TransformAnimator animator)
    {
      if (animator == null)
      {
        TranslateTransform translateTransform = TransformAnimator.GetTranslateTransform((UIElement) targetElement);
        if (translateTransform != null)
          animator = new TransformAnimator(translateTransform);
      }
      if (animator == null)
        throw new InvalidOperationException("The animation system could not be prepared for the target element.");
    }

    public static TranslateTransform GetTranslateTransform(UIElement container)
    {
      if (container == null)
        throw new ArgumentNullException(nameof (container));
      if (!(container.RenderTransform is TranslateTransform translateTransform))
      {
        if (container.RenderTransform == null)
        {
          translateTransform = new TranslateTransform();
          container.RenderTransform = (Transform) translateTransform;
        }
        else if (container.RenderTransform is TransformGroup)
        {
          TransformGroup renderTransform = container.RenderTransform as TransformGroup;
          translateTransform = ((IEnumerable<Transform>) renderTransform.Children).Where<Transform>((Func<Transform, bool>) (t => t is TranslateTransform)).Select<Transform, TranslateTransform>((Func<Transform, TranslateTransform>) (t => (TranslateTransform) t)).FirstOrDefault<TranslateTransform>();
          if (translateTransform == null)
          {
            translateTransform = new TranslateTransform();
            ((PresentationFrameworkCollection<Transform>) renderTransform.Children).Add((Transform) translateTransform);
          }
        }
        else
        {
          TransformGroup transformGroup = new TransformGroup();
          Transform renderTransform = container.RenderTransform;
          container.RenderTransform = (Transform) null;
          ((PresentationFrameworkCollection<Transform>) transformGroup.Children).Add(renderTransform);
          translateTransform = new TranslateTransform();
          ((PresentationFrameworkCollection<Transform>) transformGroup.Children).Add((Transform) translateTransform);
          container.RenderTransform = (Transform) transformGroup;
        }
      }
      return translateTransform;
    }
  }
}
