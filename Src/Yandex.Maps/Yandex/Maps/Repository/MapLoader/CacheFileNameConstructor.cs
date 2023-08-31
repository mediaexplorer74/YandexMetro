// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.CacheFileNameConstructor
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using System.Text;
using Yandex.IO;
using Yandex.Maps.API;
using Yandex.Maps.API.Interfaces;
using Yandex.Maps.Repository.Config;
using Yandex.Maps.Repository.Interfaces;

namespace Yandex.Maps.Repository.MapLoader
{
  [ComVisible(true)]
  public class CacheFileNameConstructor : ITileFileNameConstructor
  {
    private readonly char[] _hexSymbol = new char[16]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'a',
      'b',
      'c',
      'd',
      'e',
      'f'
    };
    private readonly ICacheFolderNameConstructor _cacheFolderNameConstructor;
    private readonly IZoomTreeHeightCalculator _zoomTreeHeightCalculator;
    private readonly ICacheConfigManager _cacheConfigManager;
    private readonly IPath _path;
    private char _directorySeparatorChar;

    public CacheFileNameConstructor(
      [NotNull] ICacheFolderNameConstructor cacheFolderNameConstructor,
      [NotNull] IZoomTreeHeightCalculator zoomTreeHeightCalculator,
      [NotNull] ICacheConfigManager cacheConfigManager,
      [NotNull] IPath path)
    {
      if (cacheFolderNameConstructor == null)
        throw new ArgumentNullException(nameof (cacheFolderNameConstructor));
      if (zoomTreeHeightCalculator == null)
        throw new ArgumentNullException(nameof (zoomTreeHeightCalculator));
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      this._cacheFolderNameConstructor = cacheFolderNameConstructor;
      this._zoomTreeHeightCalculator = zoomTreeHeightCalculator;
      this._cacheConfigManager = cacheConfigManager;
      this._path = path;
      this._directorySeparatorChar = this._path.DirectorySeparatorChar;
    }

    public void ParseFileName(
      string fileName,
      out int i,
      out int j,
      out byte zoom,
      out BaseLayers type)
    {
      string str = this._cacheFolderNameConstructor.GetRootDirectoryWithTileType().ToString();
      if (fileName.StartsWith(str))
      {
        fileName = fileName.Substring(str.Length);
        string[] strArray = fileName.Split(this._directorySeparatorChar);
        int num = 0;
        i = 0;
        j = 0;
        zoom = (byte) 0;
        type = BaseLayers.none;
        int length = strArray.Length;
        bool flag1 = false;
        bool flag2 = true;
        for (int index1 = 0; index1 < length; ++index1)
        {
          string s = strArray[index1];
          if (flag2)
          {
            flag2 = false;
            flag1 = true;
            type = this._cacheConfigManager.GetLayerTypeById(int.Parse(s));
          }
          else if (flag1)
          {
            flag1 = false;
            zoom = byte.Parse(s);
          }
          else
          {
            for (int index2 = 0; index2 < this._hexSymbol.Length; ++index2)
            {
              if ((int) this._hexSymbol[index2] == (int) s[0])
                i += index2 << 4 * (length - index1);
              if ((int) this._hexSymbol[index2] == (int) s[1])
                j += index2 << 4 * (length - index1);
              if (s.Length > 2 && (int) this._hexSymbol[index2] == (int) s[2])
                num = index2;
            }
          }
        }
        i <<= 4;
        j <<= 4;
        if (num >> 1 == 1)
          i += 128;
        if ((num & 1) != 1)
          return;
        j += 128;
      }
      else
      {
        i = 0;
        j = 0;
        zoom = (byte) 0;
        type = BaseLayers.none;
      }
    }

    public string GetTileFileName(ITileInfo tileInfo, out string directoryName)
    {
      StringBuilder directoryWithTileType = this._cacheFolderNameConstructor.GetRootDirectoryWithTileType();
      int layerIdByType = this._cacheConfigManager.GetLayerIdByType(tileInfo.Layer);
      directoryWithTileType.Append(layerIdByType);
      directoryWithTileType.Append(this._directorySeparatorChar);
      directoryWithTileType.Append(tileInfo.Zoom);
      directoryWithTileType.Append(this._directorySeparatorChar);
      int num1 = 0;
      int num2 = 0;
      int heightTreeForZoom = (int) this._zoomTreeHeightCalculator.GetHeightTreeForZooms()[(int) tileInfo.Zoom];
      int num3 = 1 << 4 * (heightTreeForZoom - 1);
      int index1 = 0;
      int index2 = 0;
      for (int index3 = 0; index3 < heightTreeForZoom - 2; ++index3)
      {
        index1 = (tileInfo.X - num1) / num3;
        index2 = (tileInfo.Y - num2) / num3;
        num1 += num3 * index1;
        num2 += num3 * index2;
        num3 >>= 4;
        if (index3 < heightTreeForZoom - 3)
        {
          directoryWithTileType.Append(this._hexSymbol[index1]);
          directoryWithTileType.Append(this._hexSymbol[index2]);
          directoryWithTileType.Append(this._directorySeparatorChar);
        }
      }
      directoryName = directoryWithTileType.ToString();
      directoryWithTileType.Append(this._hexSymbol[index1]);
      directoryWithTileType.Append(this._hexSymbol[index2]);
      int x = tileInfo.X - num1;
      int y = tileInfo.Y - num2;
      int index4 = x >> 7 << 1 | y >> 7;
      directoryWithTileType.Append(this._hexSymbol[index4]);
      if (x >> 7 > 0)
        x -= 128;
      if (y >> 7 > 0)
        y -= 128;
      tileInfo.IdSort = Utils.CalcMortonNumber(x >> 1, y >> 1);
      tileInfo.Index = Utils.CalcIndex(x, y);
      return directoryWithTileType.ToString();
    }
  }
}
