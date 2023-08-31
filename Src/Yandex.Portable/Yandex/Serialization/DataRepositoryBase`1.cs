// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.DataRepositoryBase`1
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using JetBrains.Annotations;
using System;
using System.IO;
using Yandex.Common;
using Yandex.IO;
using Yandex.PAL.IO;
using Yandex.Serialization.Interfaces;

namespace Yandex.Serialization
{
  public abstract class DataRepositoryBase<TData> : IDataRepository<TData> where TData : class, new()
  {
    private readonly IFileStorage _fileStorage;
    private readonly IGenericXmlSerializer<TData> _dataSerializer;
    private readonly IPath _path;
    private TData _data;
    private readonly object _dataSync = new object();

    protected DataRepositoryBase(
      [NotNull] IFileStorage fileStorage,
      [NotNull] IPath path,
      [NotNull] IGenericXmlSerializer<TData> dataSerializer)
    {
      if (fileStorage == null)
        throw new ArgumentNullException(nameof (fileStorage));
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (dataSerializer == null)
        throw new ArgumentNullException(nameof (dataSerializer));
      this._fileStorage = fileStorage;
      this._path = path;
      this._dataSerializer = dataSerializer;
    }

    public TData Data
    {
      get
      {
        lock (this._dataSync)
          return this._data ?? (this._data = this.ReadData());
      }
    }

    private TData ReadData()
    {
      TData data = default (TData);
      string dataFileName = this.GetDataFileName();
      try
      {
        if (this._fileStorage.FileExists(dataFileName))
        {
          using (Stream stream = this._fileStorage.OpenFile(dataFileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
            data = this._dataSerializer.Deserialize(stream);
        }
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
      return data ?? new TData();
    }

    protected abstract string GetDataFileName();

    public void Save()
    {
      string dataFileName = this.GetDataFileName();
      try
      {
        string directoryName = this._path.GetDirectoryName(dataFileName);
        if (!this._fileStorage.DirectoryExistsCached(directoryName))
          this._fileStorage.CreateDirectory(directoryName);
        using (Stream outStream = this._fileStorage.OpenFile(dataFileName, FileMode.Create, FileAccess.Write, FileShare.None))
          this._dataSerializer.Serialize(this.Data, outStream);
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
    }
  }
}
