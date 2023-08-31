// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Behaviors.FlyInOutImageBehavior
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Y.UI.Common.Behaviors
{
  public class FlyInOutImageBehavior : Behavior<Image>
  {
    private Storyboard _flyinAnimation;

    protected override void OnAttached()
    {
      ((UIElement) this.AssociatedObject).RenderTransform = (Transform) new TranslateTransform()
      {
        X = -480.0
      };
      this.SetInitialState();
      ((FrameworkElement) this.AssociatedObject).Loaded += new RoutedEventHandler(this.AssociatedObjectLoaded);
      ((FrameworkElement) this.AssociatedObject).Unloaded += new RoutedEventHandler(this.AssociatedObjectUnloaded);
      base.OnAttached();
    }

    protected override void OnDetaching()
    {
      if (this._flyinAnimation != null)
        this._flyinAnimation.Stop();
      this.SetInitialState();
      ((FrameworkElement) this.AssociatedObject).Loaded -= new RoutedEventHandler(this.AssociatedObjectLoaded);
      ((FrameworkElement) this.AssociatedObject).Loaded -= new RoutedEventHandler(this.AssociatedObjectUnloaded);
      base.OnDetaching();
    }

    private void AssociatedObjectUnloaded(object sender, RoutedEventArgs e) => this.SetInitialState();

    private void AssociatedObjectLoaded(object sender, RoutedEventArgs e) => this.Activate();

    private void Activate()
    {
      ((UIElement) this.AssociatedObject).Visibility = (Visibility) 0;
      if (this._flyinAnimation == null)
      {
        Storyboard storyboard = new Storyboard();
        ((Timeline) storyboard).Duration = Duration.Automatic;
        this._flyinAnimation = storyboard;
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
        DoubleKeyFrameCollection keyFrames1 = animationUsingKeyFrames.KeyFrames;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame1 = new DiscreteDoubleKeyFrame();
        ((DoubleKeyFrame) discreteDoubleKeyFrame1).KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1000.0));
        ((DoubleKeyFrame) discreteDoubleKeyFrame1).Value = -480.0;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame2 = discreteDoubleKeyFrame1;
        ((PresentationFrameworkCollection<DoubleKeyFrame>) keyFrames1).Add((DoubleKeyFrame) discreteDoubleKeyFrame2);
        DoubleKeyFrameCollection keyFrames2 = animationUsingKeyFrames.KeyFrames;
        EasingDoubleKeyFrame easingDoubleKeyFrame1 = new EasingDoubleKeyFrame();
        ((DoubleKeyFrame) easingDoubleKeyFrame1).KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1700.0));
        EasingDoubleKeyFrame easingDoubleKeyFrame2 = easingDoubleKeyFrame1;
        CubicEase cubicEase1 = new CubicEase();
        ((EasingFunctionBase) cubicEase1).EasingMode = (EasingMode) 0;
        CubicEase cubicEase2 = cubicEase1;
        easingDoubleKeyFrame2.EasingFunction = (IEasingFunction) cubicEase2;
        ((DoubleKeyFrame) easingDoubleKeyFrame1).Value = 0.0;
        EasingDoubleKeyFrame easingDoubleKeyFrame3 = easingDoubleKeyFrame1;
        ((PresentationFrameworkCollection<DoubleKeyFrame>) keyFrames2).Add((DoubleKeyFrame) easingDoubleKeyFrame3);
        DoubleAnimation doubleAnimation1 = new DoubleAnimation();
        doubleAnimation1.From = new double?(0.0);
        doubleAnimation1.To = new double?(1.0);
        ((Timeline) doubleAnimation1).Duration = new Duration(TimeSpan.FromMilliseconds(1700.0));
        DoubleAnimation doubleAnimation2 = doubleAnimation1;
        ((PresentationFrameworkCollection<Timeline>) this._flyinAnimation.Children).Add((Timeline) animationUsingKeyFrames);
        ((PresentationFrameworkCollection<Timeline>) this._flyinAnimation.Children).Add((Timeline) doubleAnimation2);
        Storyboard.SetTarget((Timeline) animationUsingKeyFrames, (DependencyObject) ((UIElement) this.AssociatedObject).RenderTransform);
        Storyboard.SetTargetProperty((Timeline) animationUsingKeyFrames, new PropertyPath("X", new object[0]));
        Storyboard.SetTarget((Timeline) doubleAnimation2, (DependencyObject) this.AssociatedObject);
        Storyboard.SetTargetProperty((Timeline) doubleAnimation2, new PropertyPath("Opacity", new object[0]));
      }
      this._flyinAnimation.Stop();
      this._flyinAnimation.Begin();
    }

    private void SetInitialState()
    {
      ((UIElement) this.AssociatedObject).Visibility = (Visibility) 1;
      ((TranslateTransform) ((UIElement) this.AssociatedObject).RenderTransform).X = -480.0;
      ((UIElement) this.AssociatedObject).Opacity = 0.0;
    }
  }
}
