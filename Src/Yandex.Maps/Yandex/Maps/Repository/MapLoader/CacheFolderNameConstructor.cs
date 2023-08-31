// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.CacheFolderNameConstructor
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Text;
using Yandex.Globalization;
using Yandex.IO;
using Yandex.Maps.API;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.PrinterClient.Interfaces;
using Yandex.Maps.Repository.Interfaces;

namespace Yandex.Maps.Repository.MapLoader
{
  internal class CacheFolderNameConstructor : ICacheFolderNameConstructor, ICacheRootDirectoryBuilder
  {
    private static char _separatorChar;
    private readonly IConfigMediator _configMediator;
    private readonly ICacheTileScaleAdapter _cacheTileScaleAdapter;
    private readonly string _cultureInfoName;

    public CacheFolderNameConstructor(
      string rootPath,
      uint cacheFormatVersion,
      [NotNull] IConfigMediator configMediator,
      [NotNull] ICacheTileScaleAdapter cacheTileScaleAdapter,
      IPath path)
    {
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      if (cacheTileScaleAdapter == null)
        throw new ArgumentNullException(nameof (cacheTileScaleAdapter));
      this.CacheRoot = rootPath;
      this.CacheFormatVersion = cacheFormatVersion;
      this._configMediator = configMediator;
      this._cacheTileScaleAdapter = cacheTileScaleAdapter;
      CacheFolderNameConstructor._separatorChar = path.DirectorySeparatorChar;
      if (CultureInfo.InvariantCulture.Equals((object) CultureInfo.CurrentCulture))
        return;
      this._cultureInfoName = CultureInfo.CurrentUICulture.GetSpecificName();
    }

    public uint CacheFormatVersion { get; private set; }

    public StringBuilder GetRootDirectoryWithTileType(
      short cacheFileScale,
      TileType tileType,
      uint cacheFormatVersion)
    {
      StringBuilder directoryWithTileType = new StringBuilder(this.CacheRoot);
      directoryWithTileType.Append(CacheFolderNameConstructor._separatorChar);
      if (!string.IsNullOrEmpty(this._cultureInfoName))
      {
        directoryWithTileType.Append(this._cultureInfoName);
        directoryWithTileType.Append(CacheFolderNameConstructor._separatorChar);
      }
      switch (cacheFormatVersion)
      {
        case 1:
          directoryWithTileType.Append(tileType.ToString());
          break;
        case 2:
          directoryWithTileType.Append(cacheFileScale.ToString());
          switch (tileType)
          {
            case TileType.@default:
              break;
            case TileType.default2x:
              directoryWithTileType.Append(".2x");
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      directoryWithTileType.Append(CacheFolderNameConstructor._separatorChar);
      return directoryWithTileType;
    }

    public string CacheRoot { get; private set; }

    public StringBuilder GetRootDirectoryWithTileType() => this.GetRootDirectoryWithTileType(this.CacheFormatVersion);

    public StringBuilder GetRootDirectoryWithTileType(uint cacheFormatVersion) => this.GetRootDirectoryWithTileType(this._cacheTileScaleAdapter.GetCacheFileScale(this._configMediator.TilesStretchFactor), this._configMediator.TileType, cacheFormatVersion);

    public CultureInfo CultureInfo { get; set; }
  }
}
