// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Config.MapLayer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Maps.API;

namespace Yandex.Maps.Config
{
  internal class MapLayer
  {
    public int Id { get; set; }

    public bool IsService { get; set; }

    public int SizeInPixels { get; set; }

    public ushort MapVersion { get; set; }

    public string Name { get; set; }

    public BaseLayers Layer { get; set; }
  }
}
