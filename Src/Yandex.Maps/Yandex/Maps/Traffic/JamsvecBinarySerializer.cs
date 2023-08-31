// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamsvecBinarySerializer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Yandex.Maps.Traffic.DTO.Tracks;
using Yandex.Positioning;
using Yandex.Serialization.Interfaces;

namespace Yandex.Maps.Traffic
{
  internal class JamsvecBinarySerializer : IGenericXmlSerializer<JamTracks>
  {
    private const uint Magic = 1397574230;
    private const uint versionLL = 256;
    private const uint versionXY = 512;
    private const int JamTrackHeaderLength = 16;
    private const int JamMetaHeaderLength = 36;
    private static readonly Encoding _utf8 = (Encoding) new UTF8Encoding();

    public JamTracks Deserialize(Stream stream, bool leaveOpen = false)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      if (!stream.CanRead)
        throw new ArgumentException("inputStream should support reading.");
      using (GZipStream input = new GZipStream(stream, CompressionMode.Decompress, leaveOpen))
      {
        JamTracks jamTracks = new JamTracks();
        using (BinaryReader binaryReader = new BinaryReader((Stream) input))
        {
          uint num1 = binaryReader.ReadUInt32();
          int num2 = (int) binaryReader.ReadUInt32();
          if (1397574230U == num1)
          {
            jamTracks.Meta = JamsvecBinarySerializer.ReadMeta(binaryReader);
            jamTracks.Tracks = JamsvecBinarySerializer.ReadTracks(binaryReader);
          }
          byte[] buffer = new byte[4096];
          do
            ;
          while (binaryReader.Read(buffer, 0, 256) > 0);
        }
        return jamTracks;
      }
    }

    private static JamMeta ReadMeta(BinaryReader binaryReader)
    {
      uint num1 = binaryReader.ReadUInt32();
      JamMeta jamMeta = new JamMeta()
      {
        TimeUint = binaryReader.ReadUInt32(),
        NextUpdateIn = binaryReader.ReadUInt32(),
        InformerExpiresIn = binaryReader.ReadUInt32(),
        JamsExpireIn = binaryReader.ReadUInt32(),
        RetryTimeout = binaryReader.ReadUInt32()
      };
      int capacity = (int) binaryReader.ReadUInt32();
      if (num1 > 36U)
        binaryReader.ReadBytes((int) num1 - 36);
      List<JamInformer> jamInformerList = new List<JamInformer>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        JamInformer jamInformer = new JamInformer()
        {
          Latitude = (double) binaryReader.ReadSingle(),
          Longitude = (double) binaryReader.ReadSingle(),
          Value = binaryReader.ReadUInt16(),
          JamColor = (JamColors) binaryReader.ReadInt16()
        };
        uint count = binaryReader.ReadUInt32();
        byte[] bytes = binaryReader.ReadBytes((int) count);
        uint num2 = count & 3U;
        if (num2 != 0U)
          binaryReader.ReadBytes(4 - (int) num2);
        jamInformer.Title = JamsvecBinarySerializer._utf8.GetString(bytes, 0, bytes.Length);
        jamInformerList.Add(jamInformer);
      }
      jamMeta.Informers = jamInformerList;
      return jamMeta;
    }

    private static List<JamTrack> ReadTracks(BinaryReader binaryReader)
    {
      uint capacity1 = binaryReader.ReadUInt32();
      List<JamTrack> jamTrackList = new List<JamTrack>((int) capacity1);
      for (int index1 = 0; (long) index1 < (long) capacity1; ++index1)
      {
        uint num = binaryReader.ReadUInt32();
        JamTrack jamTrack = new JamTrack()
        {
          Severity = binaryReader.ReadUInt16(),
          StyleId = (int) binaryReader.ReadUInt16(),
          AvgSpeed = (double) binaryReader.ReadSingle(),
          StreetCategory = (int) binaryReader.ReadUInt16()
        };
        ushort capacity2 = binaryReader.ReadUInt16();
        if (num > 16U)
          binaryReader.ReadBytes((int) num - 16);
        List<GeoCoordinate> geoCoordinateList = jamTrack.Coordinates = new List<GeoCoordinate>((int) capacity2);
        for (int index2 = 0; index2 < (int) capacity2; ++index2)
          geoCoordinateList.Add(new GeoCoordinate((double) binaryReader.ReadSingle(), (double) binaryReader.ReadSingle()));
        jamTrackList.Add(jamTrack);
      }
      return jamTrackList;
    }

    public JamTracks Deserialize(string value) => throw new NotImplementedException();

    public void Serialize(JamTracks value, Stream outStream) => throw new NotImplementedException();

    public string Serialize(JamTracks value) => throw new NotImplementedException();
  }
}
