// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Interfaces.ITileFileNameConstructor
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;

namespace Yandex.Maps.Repository.Interfaces
{
  internal interface ITileFileNameConstructor
  {
    string GetTileFileName(ITileInfo tileInfo, out string directoryName);

    void ParseFileName(string fileName, out int i, out int j, out byte zoom, out BaseLayers type);
  }
}
