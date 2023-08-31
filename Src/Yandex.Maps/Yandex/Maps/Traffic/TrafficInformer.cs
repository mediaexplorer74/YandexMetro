// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.TrafficInformer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using Yandex.Maps.Traffic.DTO.Tracks;
using Yandex.Positioning;

namespace Yandex.Maps.Traffic
{
  internal class TrafficInformer
  {
    public GeoCoordinate Position { get; private set; }

    public ushort Value { get; set; }

    public bool HasColor { get; set; }

    public DateTime ExpiresAt { get; set; }

    public TrafficInformer(JamInformer informer, DateTime expiresAt)
    {
      this.Position = new GeoCoordinate(informer.Latitude, informer.Longitude);
      this.HasColor = informer.JamColor != JamColors.@false;
      this.Value = informer.Value;
      this.ExpiresAt = expiresAt;
    }

    public void Update(TrafficInformer informer)
    {
      this.Value = informer.Value;
      this.HasColor = informer.HasColor;
      this.ExpiresAt = informer.ExpiresAt;
    }
  }
}
