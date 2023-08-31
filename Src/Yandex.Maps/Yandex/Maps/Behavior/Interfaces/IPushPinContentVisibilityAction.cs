// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Behavior.Interfaces.IPushPinContentVisibilityAction
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System.Collections.Generic;

namespace Yandex.Maps.Behavior.Interfaces
{
  internal interface IPushPinContentVisibilityAction
  {
    void InitializeMap([NotNull] MapBase map);

    void OnPushPinContentBecomeVisible([NotNull] PushPin pushPin, [NotNull] IList<PushPin> allPushPins);

    void OnPushPinContentBecomeCollapsed([NotNull] PushPin pushPin, [NotNull] IList<PushPin> allPushPins);

    void OnDetaching();

    void PushPinRemoved([NotNull] PushPin pushpin);
  }
}
