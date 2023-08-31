// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DTO.Tracks.JamTracks
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Yandex.Maps.Traffic.DTO.Tracks
{
  [XmlRoot("tjam_tracks")]
  [ComVisible(true)]
  public class JamTracks
  {
    private const string error = "error";
    private const string track = "track";
    private const string tracks = "tracks";
    private const string meta = "meta";

    [XmlAttribute("error")]
    public int ErrorCode { get; set; }

    [XmlElement("meta")]
    public JamMeta Meta { get; set; }

    [XmlArrayItem("track")]
    [XmlArray("tracks")]
    public List<JamTrack> Tracks { get; set; }
  }
}
