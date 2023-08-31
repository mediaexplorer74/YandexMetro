// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.Config.CacheConfigDataRepository
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using Yandex.IO;
using Yandex.Maps.Repository.Interfaces;
using Yandex.Serialization;
using Yandex.Serialization.Interfaces;

namespace Yandex.Maps.Repository.Config
{
  [ComVisible(true)]
  public class CacheConfigDataRepository : DataRepositoryBase<MapLayers>
  {
    private const string CacheConfigFolder = "config";
    private const string CacheConfigFile = "system.xml";
    private readonly ICacheFolderNameConstructor _cacheFolderNameConstructor;
    private readonly string _cacheConfigPath;

    public CacheConfigDataRepository(
      [NotNull] IFileStorage fileStorage,
      [NotNull] IPath path,
      [NotNull] IGenericXmlSerializer<MapLayers> cacheConfigDataSerializer,
      [NotNull] ICacheFolderNameConstructor cacheFolderNameConstructor)
      : base(fileStorage, path, cacheConfigDataSerializer)
    {
      this._cacheFolderNameConstructor = cacheFolderNameConstructor != null ? cacheFolderNameConstructor : throw new ArgumentNullException(nameof (cacheFolderNameConstructor));
      this._cacheConfigPath = this.Path.Combine("config", "system.xml");
    }

    protected override string GetDataFileName() => this._cacheFolderNameConstructor.GetRootDirectoryWithTileType().Append(this._cacheConfigPath).ToString();
  }
}
