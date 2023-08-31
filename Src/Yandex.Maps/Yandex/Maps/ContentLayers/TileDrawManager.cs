// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ContentLayers.TileDrawManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Yandex.Collections;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.ContentLayers.Interfaces;
using Yandex.Media.Imaging;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Maps.ContentLayers
{
  internal class TileDrawManager : ITileDrawManager
  {
    private readonly IDictionary<ITileInfo, Image> _bitmapsStorage;
    private readonly IUiDispatcher _uiDispatcher;
    private readonly IPool<Image> _imagePool;

    public TileDrawManager(IUiDispatcher uiDispatcher, IPool<Image> imagePool)
    {
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      if (imagePool == null)
        throw new ArgumentNullException(nameof (imagePool));
      this._uiDispatcher = uiDispatcher;
      this._imagePool = imagePool;
      this._bitmapsStorage = (IDictionary<ITileInfo, Image>) new ConcurrentDictionary<ITileInfo, Image>();
    }

    public TileLayer TileLayer { get; set; }

    public void DrawTiles(IEnumerable<ITile> tiles, RenderContentMode renderContentMode)
    {
      foreach (ITile tile in tiles)
      {
        BitmapSource bitmapSource = tile.BitmapSource as BitmapSource;
        if (bitmapSource == null)
          break;
        bool isNew = false;
        ITileInfo tileInfo = tile.TileInfo;
        Image image;
        if (!this._bitmapsStorage.TryGetValue(tileInfo, out image))
        {
          image = this._imagePool.Pop();
          isNew = true;
        }
        this._uiDispatcher.BeginInvoke((Action) (() =>
        {
          image.Source = (ImageSource) null;
          image.Source = (ImageSource) bitmapSource;
          if (!isNew)
            return;
          this.AddImage(tileInfo, image);
        }));
      }
    }

    private void AddImage(ITileInfo tileInfo, Image image)
    {
      ((UIElement) image).CacheMode = (CacheMode) new BitmapCache();
      this.TileLayer.AddChild((UIElement) image, tileInfo);
      this._bitmapsStorage[tileInfo] = image;
    }

    public void DisposeTiles()
    {
      List<Image> removeImages = this._bitmapsStorage.Select<KeyValuePair<ITileInfo, Image>, Image>((Func<KeyValuePair<ITileInfo, Image>, Image>) (item => item.Value)).ToList<Image>();
      this._bitmapsStorage.Clear();
      this._uiDispatcher.BeginInvoke((Action) (() =>
      {
        ((PresentationFrameworkCollection<UIElement>) this.TileLayer.Children).Clear();
        foreach (Image image in removeImages)
          this._imagePool.Push(image);
      }));
    }

    public void DisposeTiles(IEnumerable<ITileInfo> tileInfos)
    {
      List<Image> removeImages = new List<Image>();
      foreach (ITileInfo tileInfo in tileInfos)
      {
        Image image;
        if (this._bitmapsStorage.TryGetValue(tileInfo, out image))
        {
          removeImages.Add(image);
          this._bitmapsStorage.Remove(tileInfo);
        }
      }
      this._uiDispatcher.BeginInvoke((Action) (() =>
      {
        foreach (Image element in removeImages)
        {
          this.TileLayer.RemoveChild((UIElement) element);
          this._imagePool.Push(element);
        }
      }));
    }
  }
}
