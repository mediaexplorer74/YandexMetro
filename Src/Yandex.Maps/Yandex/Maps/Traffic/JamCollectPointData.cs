// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamCollectPointData
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;

namespace Yandex.Maps.Traffic
{
  internal class JamCollectPointData
  {
    public double Lat { get; set; }

    public double Lon { get; set; }

    public double AvgSpeed { get; set; }

    public double Direction { get; set; }

    public DateTimeOffset Time { get; set; }

    public bool IsSending { get; set; }
  }
}
