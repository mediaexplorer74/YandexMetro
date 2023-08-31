// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.BulkWriteableBitmapBuilder
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media.Imaging;
using Yandex.Common;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Media.Imaging
{
  internal class BulkWriteableBitmapBuilder : IBulkWriteableBitmapBuilder
  {
    private readonly IWriteableBitmapBuilder _writableBitmapBuilder;
    private readonly IUiDispatcher _uiDispatcher;
    private readonly Size _size;

    public BulkWriteableBitmapBuilder(
      [NotNull] IWriteableBitmapBuilder writableBitmapBuilder,
      [NotNull] IUiDispatcher uiDispatcher,
      Size size)
    {
      if (writableBitmapBuilder == null)
        throw new ArgumentNullException(nameof (writableBitmapBuilder));
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      this._writableBitmapBuilder = writableBitmapBuilder;
      this._uiDispatcher = uiDispatcher;
      this._size = size;
    }

    public IList<WriteableBitmap> CreateWritableBitmaps(int amount)
    {
      IList<WriteableBitmap> writableBitmaps = (IList<WriteableBitmap>) new List<WriteableBitmap>(amount);
      lock (writableBitmaps)
      {
        this._uiDispatcher.BeginInvoke((Action) (() => this.CreateWritableBitmapsOnUiThread(amount, (ICollection<WriteableBitmap>) writableBitmaps)));
        Monitor.Wait((object) writableBitmaps);
      }
      return writableBitmaps;
    }

    private void CreateWritableBitmapsOnUiThread(
      int amount,
      ICollection<WriteableBitmap> writableBitmaps)
    {
      try
      {
        Size bitmapSize = this.GetBitmapSize();
        int width = (int) bitmapSize.Width;
        int height = (int) bitmapSize.Height;
        for (int index = 0; index < amount; ++index)
        {
          WriteableBitmap bitmapFromUiThread = this._writableBitmapBuilder.CreateWriteableBitmapFromUIThread(width, height);
          writableBitmaps.Add(bitmapFromUiThread);
        }
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
      finally
      {
        lock (writableBitmaps)
          Monitor.Pulse((object) writableBitmaps);
      }
    }

    protected virtual Size GetBitmapSize() => this._size;
  }
}
