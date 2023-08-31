// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.DTO.Tracks.JamMeta
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Yandex.Serialization;

namespace Yandex.Maps.Traffic.DTO.Tracks
{
  [ComVisible(true)]
  public class JamMeta
  {
    private const string time = "time";
    private const string next_update = "next_update";
    private const string informer_expire_in = "informer_expire_in";
    private const string jams_expire_in = "jams_expire_in";
    private const string retry_timeout = "retry_timeout";
    private const string jam_informers = "jam_informers";
    private const string informer = "informer";

    [XmlIgnore]
    public DateTime Time { get; set; }

    [XmlElement("time")]
    public string TimeString
    {
      get => this.Time.ToString();
      set
      {
        uint result;
        this.Time = uint.TryParse(value, out result) ? UnixTimestampConverter.GetDateTime(result) : new DateTime();
      }
    }

    public uint TimeUint
    {
      get => 0;
      set => this.Time = UnixTimestampConverter.GetDateTime(value);
    }

    [XmlElement("next_update")]
    public uint NextUpdateIn { get; set; }

    [XmlElement("informer_expire_in")]
    public uint InformerExpiresIn { get; set; }

    [XmlElement("jams_expire_in")]
    public uint JamsExpireIn { get; set; }

    [XmlElement("retry_timeout")]
    public uint RetryTimeout { get; set; }

    [XmlArray("jam_informers")]
    [XmlArrayItem("informer")]
    public List<JamInformer> Informers { get; set; }
  }
}
