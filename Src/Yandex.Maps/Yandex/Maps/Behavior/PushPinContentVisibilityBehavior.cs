// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Behavior.PushPinContentVisibilityBehavior
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Yandex.Maps.Behavior.Interfaces;

namespace Yandex.Maps.Behavior
{
  [ComVisible(false)]
  public class PushPinContentVisibilityBehavior : PushPinBehaviorBase
  {
    private readonly List<IPushPinContentVisibilityAction> _actions = new List<IPushPinContentVisibilityAction>()
    {
      (IPushPinContentVisibilityAction) new PushPinCollapseAction(),
      (IPushPinContentVisibilityAction) new PushPinTopAction()
    };

    protected override void OnAttached()
    {
      base.OnAttached();
      foreach (IPushPinContentVisibilityAction action in this._actions)
        action.InitializeMap(this.AssociatedObject);
      foreach (PushPin pushPin in (IEnumerable<PushPin>) this.Group)
        pushPin.ContentVisibilityChanged += new EventHandler(this.PushpinContentVisibilityChanged);
    }

    protected override void OnPushPinManagerPushPinAdded(PushPin pushpin)
    {
      pushpin.ContentVisibilityChanged += new EventHandler(this.PushpinContentVisibilityChanged);
      this.ProcessActions(pushpin);
    }

    protected override void OnPushPinManagerPushPinRemoved(PushPin pushpin)
    {
      foreach (IPushPinContentVisibilityAction action in this._actions)
        action.PushPinRemoved(pushpin);
      pushpin.ContentVisibilityChanged -= new EventHandler(this.PushpinContentVisibilityChanged);
    }

    protected override void OnDetaching()
    {
      foreach (IPushPinContentVisibilityAction action in this._actions)
        action.OnDetaching();
      foreach (PushPin pushPin in (IEnumerable<PushPin>) this.Group)
        pushPin.ContentVisibilityChanged -= new EventHandler(this.PushpinContentVisibilityChanged);
      base.OnDetaching();
    }

    private void PushpinContentVisibilityChanged(object sender, EventArgs e) => this.ProcessActions((PushPin) sender);

    private void ProcessActions(PushPin pushPin)
    {
      foreach (IPushPinContentVisibilityAction action in this._actions)
      {
        if (pushPin.ContentVisibility == null)
          action.OnPushPinContentBecomeVisible(pushPin, this.Group);
        else
          action.OnPushPinContentBecomeCollapsed(pushPin, this.Group);
      }
    }
  }
}
