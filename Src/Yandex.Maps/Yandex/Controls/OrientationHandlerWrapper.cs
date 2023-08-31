// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.OrientationHandlerWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using Yandex.DevUtils;

namespace Yandex.Controls
{
  internal class OrientationHandlerWrapper : FrameworkElement
  {
    private PhoneApplicationPage _page;
    public static readonly DependencyProperty PageOrientationProperty = DependencyProperty.Register(nameof (PageOrientation), typeof (PageOrientation), typeof (OrientationHandlerWrapper), new PropertyMetadata((object) (PageOrientation) 1));

    public PageOrientation PageOrientation
    {
      get => (PageOrientation) ((DependencyObject) this).GetValue(OrientationHandlerWrapper.PageOrientationProperty);
      set => ((DependencyObject) this).SetValue(OrientationHandlerWrapper.PageOrientationProperty, (object) value);
    }

    public OrientationHandlerWrapper()
    {
      this.Loaded += new RoutedEventHandler(this.RotationHandlerWrapperLoaded);
      this.Unloaded += new RoutedEventHandler(this.OrientationHandlerWrapperUnloaded);
    }

    private event EventHandler _portraitOrientationSet;

    private event EventHandler _landscapeLeftOrientationSet;

    private event EventHandler _landscapeRightOrientationSet;

    private void OnPortraitOrientationSet(EventArgs e)
    {
      EventHandler portraitOrientationSet = this._portraitOrientationSet;
      if (portraitOrientationSet == null)
        return;
      portraitOrientationSet((object) this, e);
    }

    private void OnLandscapeLeftOrientationSet(EventArgs e)
    {
      EventHandler leftOrientationSet = this._landscapeLeftOrientationSet;
      if (leftOrientationSet == null)
        return;
      leftOrientationSet((object) this, e);
    }

    private void OnLandscapeRightOrientationSet(EventArgs e)
    {
      EventHandler rightOrientationSet = this._landscapeRightOrientationSet;
      if (rightOrientationSet == null)
        return;
      rightOrientationSet((object) this, e);
    }

    public event EventHandler PortraitOrientationSet
    {
      add
      {
        this._portraitOrientationSet += value;
        this.FireOrientationChangedIfAllSubscribed();
      }
      remove => this._portraitOrientationSet -= value;
    }

    public event EventHandler LandscapeLeftOrientationSet
    {
      add
      {
        this._landscapeLeftOrientationSet += value;
        this.FireOrientationChangedIfAllSubscribed();
      }
      remove => this._landscapeLeftOrientationSet -= value;
    }

    public event EventHandler LandscapeRightOrientationSet
    {
      add
      {
        this._landscapeRightOrientationSet += value;
        this.FireOrientationChangedIfAllSubscribed();
      }
      remove => this._landscapeRightOrientationSet -= value;
    }

    private void FireOrientationChangedIfAllSubscribed()
    {
      if (this._landscapeLeftOrientationSet == null || this._landscapeRightOrientationSet == null || this._portraitOrientationSet == null)
        return;
      this.OnOrientationSet(this._page.Orientation);
    }

    private void RotationHandlerWrapperLoaded(object sender, RoutedEventArgs e)
    {
      if (DesignerProperties.IsInDesignMode)
        return;
      this._page = ((ContentControl) Application.Current.RootVisual).Content as PhoneApplicationPage;
      if (this._page == null)
        return;
      this.OnOrientationSet(this._page.Orientation);
      this._page.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(this.PageOrientationChanged);
    }

    private void OrientationHandlerWrapperUnloaded(object sender, RoutedEventArgs e)
    {
      if (this._page == null)
        return;
      this._page.OrientationChanged -= new EventHandler<OrientationChangedEventArgs>(this.PageOrientationChanged);
    }

    private void PageOrientationChanged(object sender, OrientationChangedEventArgs e) => this.OnOrientationSet(e.Orientation);

    private void OnOrientationSet(PageOrientation pageOrientation)
    {
      this.PageOrientation = pageOrientation;
      PageOrientation pageOrientation1 = pageOrientation;
      if (pageOrientation1 != 5)
      {
        if (pageOrientation1 != 18)
        {
          if (pageOrientation1 != 34)
            return;
          this.OnLandscapeRightOrientationSet(EventArgs.Empty);
        }
        else
          this.OnLandscapeLeftOrientationSet(EventArgs.Empty);
      }
      else
        this.OnPortraitOrientationSet(EventArgs.Empty);
    }
  }
}
