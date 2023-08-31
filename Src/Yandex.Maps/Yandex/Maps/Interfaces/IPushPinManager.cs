// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Interfaces.IPushPinManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Yandex.Maps.Events;

namespace Yandex.Maps.Interfaces
{
  [ComVisible(false)]
  public interface IPushPinManager
  {
    event EventHandler<GroupKeyArgs> PushPinAddedToGroup;

    event EventHandler<GroupKeyArgs> PushPinRemovedFromGroup;

    IDictionary<string, List<PushPin>> Groups { get; }

    void SetPushPinGroups(PushPin pushpin, string[] groupKeys);

    void CollapseAllPushpins();
  }
}
