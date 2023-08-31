// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Repository.MapLoader.CacheMetaDataRepository
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

namespace Yandex.Maps.Repository.MapLoader
{
  [ComVisible(true)]
  public class CacheMetaDataRepository : DataRepositoryBase<CacheMetaData>
  {
    private const string CacheVersionFileName = "meta.xml";
    private readonly ICacheFolderNameConstructor _cacheFolderNameConstructor;

    public CacheMetaDataRepository(
      [NotNull] IFileStorage fileStorage,
      [NotNull] IPath path,
      [NotNull] IGenericXmlSerializer<CacheMetaData> cacheMetaDataSerializer,
      [NotNull] ICacheFolderNameConstructor cacheFolderNameConstructor)
      : base(fileStorage, path, cacheMetaDataSerializer)
    {
      this._cacheFolderNameConstructor = cacheFolderNameConstructor != null ? cacheFolderNameConstructor : throw new ArgumentNullException(nameof (cacheFolderNameConstructor));
    }

    protected override string GetDataFileName() => this.Path.Combine(this._cacheFolderNameConstructor.CacheRoot, "meta.xml");
  }
}
