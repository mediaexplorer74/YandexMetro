// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.LayerCommand
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Maps.API;
using Yandex.Maps.Controls;
using Yandex.Maps.Interfaces;

namespace Yandex.Maps
{
  internal class LayerCommand : ILayerCommand
  {
    public LayerCommand(LayerCommandTypes type, ViewportRect viewportRect)
      : this(type)
    {
      this.ViewportRect = viewportRect;
    }

    public LayerCommand(LayerCommandTypes type) => this.Type = type;

    public LayerCommandTypes Type { get; private set; }

    public ViewportRect ViewportRect { get; private set; }
  }
}
