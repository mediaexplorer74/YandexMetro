// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Behavior.PushPinZIndexBehavior
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Clarity.Phone.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Yandex.Maps.Helpers;
using Yandex.Positioning;

namespace Yandex.Maps.Behavior
{
  [ComVisible(false)]
  public class PushPinZIndexBehavior : PushPinBehaviorBase
  {
    private const int CollapsedPushPinZIndex = 0;
    private const int ExpandedPushPinZIndexOffset = 1;
    private const int ZIndexMaxValue = 32766;

    protected override void OnAttached()
    {
      base.OnAttached();
      foreach (PushPin pushpin in (IEnumerable<PushPin>) this.Group)
      {
        pushpin.StateChanged += new EventHandler(this.PushpinStateChanged);
        PushPinZIndexBehavior.SetZIndex(pushpin);
      }
    }

    protected override void OnDetaching()
    {
      foreach (PushPin pushPin in (IEnumerable<PushPin>) this.Group)
        pushPin.StateChanged -= new EventHandler(this.PushpinStateChanged);
      base.OnDetaching();
    }

    protected override void OnPushPinManagerPushPinAdded(PushPin pushpin)
    {
      pushpin.StateChanged += new EventHandler(this.PushpinStateChanged);
      PushPinZIndexBehavior.SetZIndex(pushpin);
    }

    protected override void OnPushPinManagerPushPinRemoved(PushPin pushpin) => pushpin.StateChanged -= new EventHandler(this.PushpinStateChanged);

    private void PushpinStateChanged(object sender, EventArgs e) => PushPinZIndexBehavior.SetZIndex((PushPin) sender);

    private static void SetZIndex(PushPin pushpin)
    {
      if (!(((FrameworkElement) pushpin).GetVisualParent() is ContentPresenter contentPresenter))
        contentPresenter = (ContentPresenter) pushpin;
      Canvas.SetZIndex((UIElement) contentPresenter, pushpin.State == PushPinState.Collapsed ? 0 : PushPinZIndexBehavior.CalcZIndex((FrameworkElement) pushpin));
    }

    private static int CalcZIndex(FrameworkElement pushpin)
    {
      GeoCoordinate locationRecursively = MapLayerHelper.GetLocationRecursively(pushpin);
      return locationRecursively == null ? 1 : 1 + Convert.ToInt32((-locationRecursively.Latitude % 45.0 + 45.0) * (6553.0 / 18.0));
    }
  }
}
