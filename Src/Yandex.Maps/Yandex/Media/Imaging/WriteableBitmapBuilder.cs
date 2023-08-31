// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.WriteableBitmapBuilder
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
  internal class WriteableBitmapBuilder : IWriteableBitmapBuilder
  {
    private readonly IUiDispatcher _uiDispatcher;

    public WriteableBitmapBuilder(IUiDispatcher uiDispatcher) => this._uiDispatcher = uiDispatcher != null ? uiDispatcher : throw new ArgumentNullException(nameof (uiDispatcher));

    public WriteableBitmap CreateWriteableBitmap(int pixelWidth, int pixelHeight)
    {
      WriteableBitmap wb = (WriteableBitmap) null;
      this._uiDispatcher.Invoke((SendOrPostCallback) (state => wb = new WriteableBitmap(pixelWidth, pixelHeight)), (object) null);
      return wb;
    }

    public WriteableBitmap CreateWriteableBitmapFromUIThread(int pixelWidth, int pixelHeight) => new WriteableBitmap(pixelWidth, pixelHeight);
  }
}
