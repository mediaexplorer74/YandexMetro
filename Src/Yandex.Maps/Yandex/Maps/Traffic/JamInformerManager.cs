// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamInformerManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.Traffic.DTO.Tracks;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.Positioning;

namespace Yandex.Maps.Traffic
{
  internal class JamInformerManager : IJamInformerManager
  {
    private readonly IDictionary<GeoCoordinate, TrafficInformer> _jamInformers;

    public JamInformerManager() => this._jamInformers = (IDictionary<GeoCoordinate, TrafficInformer>) new Dictionary<GeoCoordinate, TrafficInformer>();

    public void UpdateInformers(JamMeta meta)
    {
      if (meta == null)
        throw new ArgumentNullException(nameof (meta));
      lock (this._jamInformers)
      {
        DateTime expiresAt = DateTime.UtcNow.AddSeconds((double) meta.InformerExpiresIn);
        foreach (JamInformer informer1 in meta.Informers)
        {
          TrafficInformer informer2 = new TrafficInformer(informer1, expiresAt);
          if (this._jamInformers.ContainsKey(informer2.Position))
            this._jamInformers[informer2.Position].Update(informer2);
          else
            this._jamInformers.Add(informer2.Position, informer2);
        }
        foreach (GeoCoordinate key in this._jamInformers.Where<KeyValuePair<GeoCoordinate, TrafficInformer>>((Func<KeyValuePair<GeoCoordinate, TrafficInformer>, bool>) (kvp => kvp.Value.ExpiresAt < DateTime.UtcNow)).ToList<KeyValuePair<GeoCoordinate, TrafficInformer>>().Select<KeyValuePair<GeoCoordinate, TrafficInformer>, GeoCoordinate>((Func<KeyValuePair<GeoCoordinate, TrafficInformer>, GeoCoordinate>) (kvp => kvp.Key)))
          this._jamInformers.Remove(key);
      }
    }

    public TrafficInformer GetInformer(GeoCoordinate coordinates)
    {
      lock (this._jamInformers)
        return this._jamInformers[coordinates];
    }
  }
}
