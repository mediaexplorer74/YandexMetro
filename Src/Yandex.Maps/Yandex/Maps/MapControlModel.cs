// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.MapControlModel
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;
using Yandex.Maps.API;
using Yandex.Maps.Controls;
using Yandex.Maps.Interfaces;
using Yandex.Media;

namespace Yandex.Maps
{
  [ComVisible(true)]
  public class MapControlModel : IMapState
  {
    public BaseLayers DisplayLayers { get; set; }

    public Point InstantCenter { get; set; }

    public double InstantZoomLevel { get; set; }

    public AnimationLevel AnimationLevel { get; set; }

    public OperationMode OperationMode { get; set; }

    public Thickness ContentPadding { get; set; }

    public bool TwilightModeEnabled { get; set; }
  }
}
