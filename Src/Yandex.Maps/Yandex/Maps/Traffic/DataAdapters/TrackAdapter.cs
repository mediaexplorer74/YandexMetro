// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DataAdapters.TrackAdapter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Traffic.DTO.Tracks;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps.Traffic.DataAdapters
{
  internal class TrackAdapter : ITrackAdapter
  {
    private readonly IGeoPixelConverter _geoPixelConverter;

    public TrackAdapter([NotNull] IGeoPixelConverter geoPixelConverter) => this._geoPixelConverter = geoPixelConverter != null ? geoPixelConverter : throw new ArgumentNullException(nameof (geoPixelConverter));

    public IList<Track> ReadTracks(IList<JamTrack> tracks, byte zoom) => (IList<Track>) tracks.Select<JamTrack, Track>((Func<JamTrack, Track>) (dtoTrack => new Track()
    {
      StyleId = dtoTrack.StyleId,
      PixelPoints = (IList<Point>) dtoTrack.Coordinates.Select<GeoCoordinate, Point>((Func<GeoCoordinate, Point>) (coord => this._geoPixelConverter.CoordinatesToZoomPoint(coord, zoom))).ToList<Point>()
    })).ToList<Track>();
  }
}
