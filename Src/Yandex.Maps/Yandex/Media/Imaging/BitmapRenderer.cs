// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.BitmapRenderer
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Threading;
using System.Windows.Media.Imaging;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Media.Imaging
{
  internal class BitmapRenderer : IBitmapRenderer
  {
    private readonly IUiDispatcher _uiDispatcher;
    private readonly IBitmapFactory _bitmapFactory;

    public BitmapRenderer(IUiDispatcher uiDispatcher, IBitmapFactory bitmapFactory)
    {
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      if (bitmapFactory == null)
        throw new ArgumentNullException(nameof (bitmapFactory));
      this._uiDispatcher = uiDispatcher;
      this._bitmapFactory = bitmapFactory;
    }

    public void Render(
      byte[] source,
      int offset,
      int length,
      int[] destination,
      int width,
      int height)
    {
      WriteableBitmap writeableBitmap = (WriteableBitmap) null;
      BitmapSource bitmapImage = this._bitmapFactory.GetBitmap(source, offset, length);
      this._uiDispatcher.Invoke((SendOrPostCallback) (argument => writeableBitmap = new WriteableBitmap(bitmapImage)), (object) null);
      writeableBitmap.Pixels.CopyTo((Array) destination, 0);
    }

    public void Render(byte[] source, int offset, int length, out int[] destination)
    {
      WriteableBitmap writeableBitmap = new WriteableBitmap(this._bitmapFactory.GetBitmap(source, offset, length));
      destination = writeableBitmap.Pixels;
    }
  }
}
