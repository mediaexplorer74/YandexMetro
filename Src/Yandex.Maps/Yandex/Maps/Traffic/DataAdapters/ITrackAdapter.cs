// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DataAdapters.ITrackAdapter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using Yandex.Maps.Traffic.DTO.Tracks;

namespace Yandex.Maps.Traffic.DataAdapters
{
  internal interface ITrackAdapter
  {
    IList<Track> ReadTracks(IList<JamTrack> tracks, byte zoom);
  }
}
