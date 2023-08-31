// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.JamCollectPostDataBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using Yandex.Maps.Traffic.Interfaces;
using Yandex.WebUtils;

namespace Yandex.Maps.Traffic
{
  [UsedImplicitly]
  internal class JamCollectPostDataBuilder : IJamCollectPostDataBuilder
  {
    public PostData GetJamCollectPostData(
      string boundary,
      string uuid,
      long packetid,
      byte[] data,
      string compressedContentType)
    {
      return new PostData()
      {
        Boundary = boundary,
        CompressedContentType = compressedContentType,
        PlainTextParams = {
          [nameof (uuid)] = (object) uuid,
          ["compressed"] = (object) "1",
          [nameof (packetid)] = (object) packetid
        },
        CompressedParams = {
          [nameof (data)] = data
        }
      };
    }
  }
}
