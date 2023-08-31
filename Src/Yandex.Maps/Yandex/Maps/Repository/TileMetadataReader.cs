// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.TileMetadataReader
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Ionic.Zlib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Yandex.Maps.API;
using Yandex.Maps.Repository.Dto;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Positioning;
using Yandex.Serialization.Interfaces;

namespace Yandex.Maps.Repository
{
  internal class TileMetadataReader : ITileMetadataReader
  {
    private const int ReadBufferLength = 4096;
    private readonly IGenericXmlSerializer<TileMetadataDto> _metadataSerializer;

    public TileMetadataReader(
      IGenericXmlSerializer<TileMetadataDto> metadataSerializer)
    {
      this._metadataSerializer = metadataSerializer != null ? metadataSerializer : throw new ArgumentNullException(nameof (metadataSerializer));
    }

    [CanBeNull]
    public TileMetadata ReadTile(byte[] data, int dataOffset, int dataLength) => this.ConvertFromDto(this._metadataSerializer.Deserialize(TileMetadataReader.DecodeTileData(data, dataOffset, dataLength)));

    [CanBeNull]
    private TileMetadata ConvertFromDto(TileMetadataDto dto)
    {
      MetadataPropertyDto metadataPropertyDto = Enumerable.OfType<MetadataPropertyDto>(dto.GeoObjects).FirstOrDefault<MetadataPropertyDto>();
      if (metadataPropertyDto == null || metadataPropertyDto.AnyMetaData == null || metadataPropertyDto.AnyMetaData.Features == null)
        return (TileMetadata) null;
      AnyMetdataFeaturesDto features = metadataPropertyDto.AnyMetaData.Features;
      return new TileMetadata()
      {
        Features = new TileMetaFeatures()
        {
          GpsBuses = features.GpsBuses,
          Routeguidance = features.Routeguidance,
          Routing = features.Routing,
          RoutingPublicTransport = features.RoutingPublicTransport,
          Semaphore = features.Semaphore,
          Streetview = features.Streetview,
          Vmap = features.Vmap,
          Voice = features.Voice,
          Widgets = features.Widgets
        },
        TransportStops = TileMetadataReader.ParsePublicTransportStops(dto)
      };
    }

    [CanBeNull]
    private static IList<PublicTransportStop> ParsePublicTransportStops(TileMetadataDto dto)
    {
      FeatureMembersDto featureMembersDto = Enumerable.OfType<FeatureMembersDto>(dto.GeoObjects).FirstOrDefault<FeatureMembersDto>();
      return featureMembersDto == null || featureMembersDto.GeoObjects == null ? (IList<PublicTransportStop>) null : (IList<PublicTransportStop>) ((IEnumerable<GeoObject>) featureMembersDto.GeoObjects).Where<GeoObject>((Func<GeoObject, bool>) (obj => obj != null && obj.MetaDataProperty != null && obj.MetaDataProperty.AnyMetaData != null && obj.MetaDataProperty.AnyMetaData.Type == "station")).Select(station => new
      {
        station = station,
        stopMetaData = station.MetaDataProperty.StopMetaData
      }).Where(_param0 => _param0.stopMetaData != null && _param0.station.Point != null).Select(_param0 => new PublicTransportStop()
      {
        Id = _param0.stopMetaData.Id,
        Name = _param0.stopMetaData.Name,
        Type = _param0.stopMetaData.Type,
        Position = new GeoCoordinate(_param0.station.Point.Pos, true),
        Style = _param0.station.Style.TrimStart('#'),
        Transports = TileMetadataReader.ParsePublicTransports((IEnumerable<TransportDto>) _param0.stopMetaData.Transports)
      }).ToList<PublicTransportStop>();
    }

    [CanBeNull]
    private static IList<PublicTransport> ParsePublicTransports(
      IEnumerable<TransportDto> transportDto)
    {
      return transportDto != null ? (IList<PublicTransport>) transportDto.Where<TransportDto>((Func<TransportDto, bool>) (dto => dto != null)).Select<TransportDto, PublicTransport>((Func<TransportDto, PublicTransport>) (dto => new PublicTransport()
      {
        Id = dto.Id,
        Name = dto.Name,
        Type = dto.Type
      })).ToList<PublicTransport>() : (IList<PublicTransport>) null;
    }

    private static string DecodeTileData(byte[] data, int dataOffset, int dataLength)
    {
      using (MemoryStream memoryStream1 = new MemoryStream())
      {
        using (MemoryStream memoryStream2 = new MemoryStream(data, dataOffset, dataLength, false))
        {
          using (GZipStream gzipStream = new GZipStream((Stream) memoryStream2, CompressionMode.Decompress))
          {
            byte[] buffer = new byte[4096];
            int count;
            while ((count = gzipStream.Read(buffer, 0, 4096)) > 0)
              memoryStream1.Write(buffer, 0, count);
          }
        }
        return Encoding.UTF8.GetString(memoryStream1.ToArray(), 0, (int) memoryStream1.Length);
      }
    }
  }
}
