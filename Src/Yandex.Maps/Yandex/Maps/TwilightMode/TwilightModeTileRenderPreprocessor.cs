// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.TwilightMode.TwilightModeTileRenderPreprocessor
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.Config.Interfaces;
using Yandex.Media.Imaging;

namespace Yandex.Maps.TwilightMode
{
  internal class TwilightModeTileRenderPreprocessor : ITileRenderPreprocessor
  {
    private readonly IConfigMediator _configMediator;

    public TwilightModeTileRenderPreprocessor([NotNull] IConfigMediator configMediator) => this._configMediator = configMediator != null ? configMediator : throw new ArgumentNullException(nameof (configMediator));

    public void PreprocessRenderedData(
      byte[] source,
      int offset,
      int length,
      out byte[] dest,
      out int destOffset,
      out int destLength)
    {
      if (this._configMediator.TwilightModeEnabled)
      {
        dest = new byte[length];
        destOffset = 0;
        destLength = length;
        Buffer.BlockCopy((Array) source, offset, (Array) dest, 0, length);
        TwilightModeTileRenderPreprocessor.ProcessPngPalette(dest);
      }
      else
      {
        dest = source;
        destOffset = offset;
        destLength = length;
      }
    }

    private static void ProcessPngPalette(byte[] data) => PngUtil.InvertPngPalette(data, 0, data.Length);
  }
}
