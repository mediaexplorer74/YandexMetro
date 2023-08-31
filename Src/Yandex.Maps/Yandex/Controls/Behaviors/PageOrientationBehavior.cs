// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Behaviors.PageOrientationBehavior
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Yandex.Controls.Behaviors
{
  internal abstract class PageOrientationBehavior : Behavior<PhoneApplicationPage>
  {
    private WeakReference _pageRef;

    protected override void OnAttached()
    {
      base.OnAttached();
      PhoneApplicationPage associatedObject = this.AssociatedObject;
      if (associatedObject == null)
        return;
      this._pageRef = new WeakReference((object) associatedObject);
      ((FrameworkElement) associatedObject).Loaded += new RoutedEventHandler(this.PageLoaded);
      associatedObject.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(this.PageOrientationChanged);
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      PhoneApplicationPage liveItem;
      if (!PageOrientationBehavior.TryGetItem<PhoneApplicationPage>(this._pageRef, out liveItem) || liveItem == null)
        return;
      ((FrameworkElement) liveItem).Loaded -= new RoutedEventHandler(this.PageLoaded);
      liveItem.OrientationChanged -= new EventHandler<OrientationChangedEventArgs>(this.PageOrientationChanged);
    }

    private void PageLoaded(object sender, RoutedEventArgs e) => this.UpdateSystemTrayVisibility();

    private void PageOrientationChanged(object sender, OrientationChangedEventArgs e) => this.UpdateSystemTrayVisibility();

    private void UpdateSystemTrayVisibility()
    {
      PhoneApplicationPage liveItem;
      if (!PageOrientationBehavior.TryGetItem<PhoneApplicationPage>(this._pageRef, out liveItem) || liveItem == null)
        return;
      this.OnUpdatePageOrientation(liveItem.Orientation);
    }

    protected abstract void OnUpdatePageOrientation(PageOrientation currentPageOrientation);

    private static bool TryGetItem<T>([CanBeNull] WeakReference reference, [CanBeNull] out T liveItem) where T : class
    {
      liveItem = default (T);
      if (reference == null || !reference.IsAlive)
        return false;
      liveItem = reference.Target as T;
      return (object) liveItem != null;
    }
  }
}
