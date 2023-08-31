// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Imaging.BulkTileWriteableBitmapBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.Config.Interfaces;
using Yandex.Media;
using Yandex.Media.Imaging;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.Imaging
{
  internal class BulkTileWriteableBitmapBuilder : BulkWriteableBitmapBuilder
  {
    private readonly IConfigMediator _configMediator;

    private static Size GetTileSize([NotNull] IConfigMediator configMediator)
    {
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      return configMediator.DisplayTileSize.Size;
    }

    public BulkTileWriteableBitmapBuilder(
      [NotNull] IWriteableBitmapBuilder writeableBitmapBuilder,
      [NotNull] IUiDispatcher uiDispatcher,
      [NotNull] IConfigMediator configMediator)
      : base(writeableBitmapBuilder, uiDispatcher, BulkTileWriteableBitmapBuilder.GetTileSize(configMediator))
    {
      if (writeableBitmapBuilder == null)
        throw new ArgumentNullException(nameof (writeableBitmapBuilder));
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      this._configMediator = configMediator != null ? configMediator : throw new ArgumentNullException(nameof (configMediator));
    }

    protected override Size GetBitmapSize() => this._configMediator.DisplayTileSize.Size;
  }
}
