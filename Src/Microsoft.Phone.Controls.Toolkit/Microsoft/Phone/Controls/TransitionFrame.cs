// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.TransitionFrame
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Microsoft.Phone.Controls
{
  [TemplatePart(Name = "SecondContentPresenter", Type = typeof (ContentPresenter))]
  [TemplatePart(Name = "FirstContentPresenter", Type = typeof (ContentPresenter))]
  public class TransitionFrame : PhoneApplicationFrame
  {
    private const string FirstTemplatePartName = "FirstContentPresenter";
    private const string SecondTemplatePartName = "SecondContentPresenter";
    internal static readonly CacheMode BitmapCacheMode = (CacheMode) new BitmapCache();
    private ContentPresenter _firstContentPresenter;
    private ContentPresenter _secondContentPresenter;
    private ContentPresenter _newContentPresenter;
    private ContentPresenter _oldContentPresenter;
    private bool _isForwardNavigation;
    private bool _useFirstAsNew;
    private bool _readyToTransitionToNewContent;
    private bool _contentReady;
    private bool _performingExitTransition;
    private ITransition _storedNewTransition;
    private NavigationInTransition _storedNavigationInTransition;
    private ITransition _storedOldTransition;
    private NavigationOutTransition _storedNavigationOutTransition;

    public TransitionFrame()
    {
      ((Control) this).DefaultStyleKey = (object) typeof (TransitionFrame);
      ((Frame) this).Navigating += new NavigatingCancelEventHandler(this.OnNavigating);
      this.BackKeyPress += new EventHandler<CancelEventArgs>(this.OnBackKeyPress);
    }

    private void FlipPresenters()
    {
      this._newContentPresenter = this._useFirstAsNew ? this._firstContentPresenter : this._secondContentPresenter;
      this._oldContentPresenter = this._useFirstAsNew ? this._secondContentPresenter : this._firstContentPresenter;
      this._useFirstAsNew = !this._useFirstAsNew;
    }

    private void OnNavigating(object sender, NavigatingCancelEventArgs e)
    {
      this._isForwardNavigation = e.NavigationMode != 1;
      if (!(((ContentControl) this).Content is UIElement content))
        return;
      this.FlipPresenters();
      TransitionElement transitionElement = (TransitionElement) null;
      ITransition transition = (ITransition) null;
      NavigationOutTransition navigationOutTransition = TransitionService.GetNavigationOutTransition(content);
      if (navigationOutTransition != null)
        transitionElement = this._isForwardNavigation ? navigationOutTransition.Forward : navigationOutTransition.Backward;
      if (transitionElement != null)
        transition = transitionElement.GetTransition(content);
      if (transition != null)
      {
        TransitionFrame.EnsureStoppedTransition(transition);
        this._storedNavigationOutTransition = navigationOutTransition;
        this._storedOldTransition = transition;
        transition.Completed += new EventHandler(this.OnExitTransitionCompleted);
        this._performingExitTransition = true;
        TransitionFrame.PerformTransition((NavigationTransition) navigationOutTransition, this._oldContentPresenter, transition);
        TransitionFrame.PrepareContentPresenterForCompositor(this._oldContentPresenter);
      }
      else
        this._readyToTransitionToNewContent = true;
    }

    private void OnExitTransitionCompleted(object sender, EventArgs e)
    {
      this._readyToTransitionToNewContent = true;
      this._performingExitTransition = false;
      TransitionFrame.CompleteTransition((NavigationTransition) this._storedNavigationOutTransition, (ContentPresenter) null, this._storedOldTransition);
      this._storedNavigationOutTransition = (NavigationOutTransition) null;
      this._storedOldTransition = (ITransition) null;
      if (!this._contentReady)
        return;
      ITransition storedNewTransition = this._storedNewTransition;
      NavigationInTransition navigationInTransition = this._storedNavigationInTransition;
      this._storedNewTransition = (ITransition) null;
      this._storedNavigationInTransition = (NavigationInTransition) null;
      this.TransitionNewContent(storedNewTransition, navigationInTransition);
    }

    public virtual void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this._firstContentPresenter = ((Control) this).GetTemplateChild("FirstContentPresenter") as ContentPresenter;
      this._secondContentPresenter = ((Control) this).GetTemplateChild("SecondContentPresenter") as ContentPresenter;
      this._newContentPresenter = this._secondContentPresenter;
      this._oldContentPresenter = this._firstContentPresenter;
      this._useFirstAsNew = true;
      this._readyToTransitionToNewContent = true;
      if (((ContentControl) this).Content == null)
        return;
      ((ContentControl) this).OnContentChanged((object) null, ((ContentControl) this).Content);
    }

    protected virtual void OnContentChanged(object oldContent, object newContent)
    {
      ((ContentControl) this).OnContentChanged(oldContent, newContent);
      this._contentReady = true;
      UIElement uiElement = oldContent as UIElement;
      UIElement element = newContent as UIElement;
      if (this._firstContentPresenter == null || this._secondContentPresenter == null || element == null)
        return;
      NavigationInTransition navigationInTransition = (NavigationInTransition) null;
      ITransition newTransition = (ITransition) null;
      if (element != null)
      {
        navigationInTransition = TransitionService.GetNavigationInTransition(element);
        TransitionElement transitionElement = (TransitionElement) null;
        if (navigationInTransition != null)
          transitionElement = this._isForwardNavigation ? navigationInTransition.Forward : navigationInTransition.Backward;
        if (transitionElement != null)
        {
          element.UpdateLayout();
          newTransition = transitionElement.GetTransition(element);
          TransitionFrame.PrepareContentPresenterForCompositor(this._newContentPresenter);
        }
      }
      ((UIElement) this._newContentPresenter).Opacity = 0.0;
      ((UIElement) this._newContentPresenter).Visibility = (Visibility) 0;
      this._newContentPresenter.Content = (object) element;
      ((UIElement) this._oldContentPresenter).Opacity = 1.0;
      ((UIElement) this._oldContentPresenter).Visibility = (Visibility) 0;
      this._oldContentPresenter.Content = (object) uiElement;
      if (this._readyToTransitionToNewContent)
      {
        this.TransitionNewContent(newTransition, navigationInTransition);
      }
      else
      {
        this._storedNewTransition = newTransition;
        this._storedNavigationInTransition = navigationInTransition;
      }
    }

    private void OnBackKeyPress(object sender, CancelEventArgs e)
    {
      if (!this._performingExitTransition || !(((ContentControl) this).Content is UIElement content))
        return;
      TransitionElement transitionElement = (TransitionElement) null;
      ITransition newTransition = (ITransition) null;
      NavigationOutTransition navigationOutTransition = TransitionService.GetNavigationOutTransition(content);
      if (navigationOutTransition != null)
        transitionElement = this._isForwardNavigation ? navigationOutTransition.Forward : navigationOutTransition.Backward;
      if (transitionElement != null)
        newTransition = transitionElement.GetTransition(content);
      if (newTransition == null)
        return;
      TransitionFrame.CompleteTransition((NavigationTransition) this._storedNavigationOutTransition, (ContentPresenter) null, this._storedOldTransition);
      this.TransitionNewContent(newTransition, (NavigationInTransition) null);
    }

    private void TransitionNewContent(
      ITransition newTransition,
      NavigationInTransition navigationInTransition)
    {
      if (this._oldContentPresenter != null)
      {
        ((UIElement) this._oldContentPresenter).Visibility = (Visibility) 1;
        this._oldContentPresenter.Content = (object) null;
      }
      if (newTransition == null)
      {
        TransitionFrame.RestoreContentPresenterInteractivity(this._newContentPresenter);
      }
      else
      {
        TransitionFrame.EnsureStoppedTransition(newTransition);
        newTransition.Completed += (EventHandler) delegate
        {
          TransitionFrame.CompleteTransition((NavigationTransition) navigationInTransition, this._newContentPresenter, newTransition);
        };
        this._readyToTransitionToNewContent = false;
        this._storedNavigationInTransition = (NavigationInTransition) null;
        this._storedNewTransition = (ITransition) null;
        TransitionFrame.PerformTransition((NavigationTransition) navigationInTransition, this._newContentPresenter, newTransition);
      }
    }

    private static void EnsureStoppedTransition(ITransition transition)
    {
      if (transition == null || transition.GetCurrentState() == 2)
        return;
      transition.Stop();
    }

    private static void PerformTransition(
      NavigationTransition navigationTransition,
      ContentPresenter presenter,
      ITransition transition)
    {
      navigationTransition?.OnBeginTransition();
      if (presenter != null && ((UIElement) presenter).Opacity != 1.0)
        ((UIElement) presenter).Opacity = 1.0;
      transition?.Begin();
    }

    private static void CompleteTransition(
      NavigationTransition navigationTransition,
      ContentPresenter presenter,
      ITransition transition)
    {
      transition?.Stop();
      TransitionFrame.RestoreContentPresenterInteractivity(presenter);
      navigationTransition?.OnEndTransition();
    }

    private static void PrepareContentPresenterForCompositor(
      ContentPresenter presenter,
      bool applyBitmapCache = true)
    {
      if (presenter == null)
        return;
      if (applyBitmapCache)
        ((UIElement) presenter).CacheMode = TransitionFrame.BitmapCacheMode;
      ((UIElement) presenter).IsHitTestVisible = false;
    }

    private static void RestoreContentPresenterInteractivity(ContentPresenter presenter)
    {
      if (presenter == null)
        return;
      ((UIElement) presenter).CacheMode = (CacheMode) null;
      if (((UIElement) presenter).Opacity != 1.0)
        ((UIElement) presenter).Opacity = 1.0;
      ((UIElement) presenter).IsHitTestVisible = true;
    }
  }
}
