﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Traffic.Interfaces.IJamCollectPostDataBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.WebUtils;

namespace Yandex.Maps.Traffic.Interfaces
{
  internal interface IJamCollectPostDataBuilder
  {
    PostData GetJamCollectPostData(
      string boundary,
      string uuid,
      long packetid,
      byte[] xmlData,
      string compressedContentType);
  }
}
