// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.Interfaces.IJamStyle
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Maps.Traffic.DTO.Styles;

namespace Yandex.Maps.Traffic.Interfaces
{
  internal interface IJamStyle
  {
    int Id { get; set; }

    List<JamZoom> Zooms { get; set; }

    int Zorder { get; set; }
  }
}
