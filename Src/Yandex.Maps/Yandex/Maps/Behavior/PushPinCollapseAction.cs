// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Behavior.PushPinCollapseAction
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Yandex.Maps.Behavior.Interfaces;

namespace Yandex.Maps.Behavior
{
  internal class PushPinCollapseAction : IPushPinContentVisibilityAction
  {
    public void InitializeMap(MapBase map)
    {
    }

    public void OnPushPinContentBecomeVisible(PushPin pushPin, IList<PushPin> allPushPins)
    {
      foreach (PushPin pushPin1 in allPushPins.Where<PushPin>((Func<PushPin, bool>) (pp => pp != pushPin)))
        pushPin1.ContentVisibility = (Visibility) 1;
    }

    public void OnPushPinContentBecomeCollapsed(PushPin pushPin, IList<PushPin> allPushPins)
    {
    }

    public void OnDetaching()
    {
    }

    public void PushPinRemoved(PushPin pushpin)
    {
    }
  }
}
