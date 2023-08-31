// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.DataRepositoryBase`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.IO;
using Yandex.Common;
using Yandex.IO;
using Yandex.Serialization.Interfaces;

namespace Yandex.Serialization
{
  internal abstract class DataRepositoryBase<TData> : IDataRepository<TData> where TData : class, new()
  {
    private readonly IFileStorage _fileStorage;
    private readonly IGenericXmlSerializer<TData> _dataSerializer;
    protected readonly IPath Path;
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
      this.Path = path;
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
        using (Stream stream = this._fileStorage.OpenFile(dataFileName, Yandex.IO.FileMode.Open, Yandex.PAL.IO.FileAccess.Read, Yandex.PAL.IO.FileShare.Read))
          data = this._dataSerializer.Deserialize(stream);
      }
      catch (FileNotFoundException ex)
      {
        return new TData();
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
      return data ?? new TData();
    }

    [NotNull]
    protected abstract string GetDataFileName();

    public void Save()
    {
      string dataFileName = this.GetDataFileName();
      try
      {
        string directoryName = this.Path.GetDirectoryName(dataFileName);
        if (!this._fileStorage.DirectoryExists(directoryName))
          this._fileStorage.CreateDirectory(directoryName);
        using (Stream outStream = this._fileStorage.OpenFile(dataFileName, Yandex.IO.FileMode.Create, Yandex.PAL.IO.FileAccess.Write, Yandex.PAL.IO.FileShare.None))
          this._dataSerializer.Serialize(this.Data, outStream);
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
    }
  }
}
