// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Imaging.TileImagePool
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.Config.Interfaces;
using Yandex.Media;
using Yandex.Media.Imaging;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.Imaging
{
  internal class TileImagePool : ImagePool
  {
    private const double AppendImageSize = 0.5;
    private readonly IConfigMediator _configMediator;

    private static Size GetTileSize([NotNull] IConfigMediator configMediator)
    {
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      return configMediator.DisplayTileSize.Size;
    }

    public TileImagePool([NotNull] IConfigMediator configMediator, [NotNull] IUiDispatcher uiDispatcher)
      : base(TileImagePool.GetTileSize(configMediator), uiDispatcher)
    {
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      this._configMediator = configMediator;
    }

    protected override Size GetImageSize() => new Size((double) this._configMediator.DisplayTileSize.Width + 0.5, (double) this._configMediator.DisplayTileSize.Height + 0.5);
  }
}
