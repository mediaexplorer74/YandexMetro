// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.WriteableBitmapPool
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Windows.Media.Imaging;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Media.Imaging
{
  internal class WriteableBitmapPool : 
    DispatcheredPool<WriteableBitmap>,
    IWriteableBitmapPool,
    IPool<WriteableBitmap>
  {
    private readonly IWriteableBitmapBuilder _writeableBitmapFactory;
    private readonly Size _tileSize;

    public WriteableBitmapPool(
      IWriteableBitmapBuilder writeableBitmapFactory,
      Size tileSize,
      IUiDispatcher uiDispatcher)
      : base(uiDispatcher)
    {
      this._writeableBitmapFactory = writeableBitmapFactory != null ? writeableBitmapFactory : throw new ArgumentNullException(nameof (writeableBitmapFactory));
      this._tileSize = tileSize;
    }

    public new void Push(WriteableBitmap item)
    {
      Array.Clear((Array) item.Pixels, 0, item.Pixels.Length);
      base.Push(item);
    }

    protected override WriteableBitmap CreateObject()
    {
      Size imageSize = this.GetImageSize();
      return this._writeableBitmapFactory.CreateWriteableBitmapFromUIThread((int) imageSize.Width, (int) imageSize.Height);
    }

    protected virtual Size GetImageSize() => this._tileSize;
  }
}
