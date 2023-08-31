// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.API.PublicTransportStop
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Yandex.Positioning;

namespace Yandex.Maps.API
{
  [ComVisible(true)]
  public class PublicTransportStop : IEquatable<PublicTransportStop>
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public IList<PublicTransport> Transports { get; set; }

    public GeoCoordinate Position { get; set; }

    public string Style { get; set; }

    public bool Equals(PublicTransportStop other) => other != null && this.Id == other.Id;

    public override bool Equals(object obj) => this.Equals(obj as PublicTransportStop);

    public override int GetHashCode() => this.Id.GetHashCode();
  }
}
