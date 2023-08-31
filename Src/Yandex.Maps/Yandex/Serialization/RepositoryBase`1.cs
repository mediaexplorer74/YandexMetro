// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.RepositoryBase`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.IO;
using System.Threading;
using Yandex.Common;
using Yandex.IO;
using Yandex.Serialization.Interfaces;

namespace Yandex.Serialization
{
  internal class RepositoryBase<TModel> : IRepositoryBase<TModel>, IReadOnlyRepositoryBase<TModel> where TModel : class, new()
  {
    internal static readonly string ModelFileName = typeof (TModel).Name;
    internal static readonly string ModelTempFileName = typeof (TModel).Name + "Temp";
    private bool _isFirstAppRun;
    private readonly IFileStorage _fileStorage;
    private readonly object _modelLock = new object();
    private readonly object _modelFileLock = new object();
    private readonly IGenericXmlSerializer<TModel> _modelSerializer;
    protected volatile TModel _model;

    public RepositoryBase([NotNull] IGenericXmlSerializer<TModel> modelSerializer, [NotNull] IFileStorage fileStorage)
    {
      if (modelSerializer == null)
        throw new ArgumentNullException(nameof (modelSerializer));
      if (fileStorage == null)
        throw new ArgumentNullException(nameof (fileStorage));
      this._modelSerializer = modelSerializer;
      this._fileStorage = fileStorage;
      this.IsFirstAppRun = true;
    }

    public virtual TModel Model
    {
      get
      {
        this.InitializeModelSafe();
        return this._model;
      }
      set => this._model = value;
    }

    public void SaveModel()
    {
      if ((object) this._model == null)
        return;
      try
      {
        Monitor.Enter(this._modelFileLock);
        using (Stream outStream = this._fileStorage.OpenFile(RepositoryBase<TModel>.ModelTempFileName, Yandex.IO.FileMode.Create, Yandex.PAL.IO.FileAccess.Write, Yandex.PAL.IO.FileShare.None))
          this.SerializeModel(outStream);
        if (this._fileStorage.FileExists(RepositoryBase<TModel>.ModelFileName))
          this._fileStorage.DeleteFile(RepositoryBase<TModel>.ModelFileName);
        this._fileStorage.MoveFile(RepositoryBase<TModel>.ModelTempFileName, RepositoryBase<TModel>.ModelFileName);
        Monitor.Exit(this._modelFileLock);
      }
      catch (Exception ex)
      {
        if (this._fileStorage.FileExists(RepositoryBase<TModel>.ModelTempFileName))
          this._fileStorage.DeleteFile(RepositoryBase<TModel>.ModelTempFileName);
        Monitor.Exit(this._modelFileLock);
        Logger.SendError(ex);
      }
    }

    public bool IsFirstAppRun
    {
      get
      {
        this.InitializeModelSafe();
        return this._isFirstAppRun;
      }
      private set => this._isFirstAppRun = value;
    }

    protected virtual void SerializeModel(Stream outStream) => this._modelSerializer.Serialize(this.Model, outStream);

    protected virtual TModel DeserializeModel(Stream inStream)
    {
      try
      {
        return this._modelSerializer.Deserialize(inStream);
      }
      catch (Exception ex)
      {
        Logger.SendError(ex);
        return default (TModel);
      }
    }

    private void InitializeModelSafe()
    {
      if ((object) this._model != null)
        return;
      lock (this._modelLock)
      {
        if ((object) this._model != null)
          return;
        this.Model = this.InitializeModel();
      }
    }

    protected virtual TModel InitializeModel()
    {
      TModel model = this.TryLoadModel(RepositoryBase<TModel>.ModelTempFileName) ?? this.TryLoadModel(RepositoryBase<TModel>.ModelFileName);
      if ((object) model != null)
        this.IsFirstAppRun = false;
      else
        model = new TModel();
      return model;
    }

    [CanBeNull]
    private TModel TryLoadModel(string modelFileName)
    {
      try
      {
        lock (this._modelFileLock)
        {
          using (Stream inStream = this._fileStorage.OpenFile(modelFileName, Yandex.IO.FileMode.Open, Yandex.PAL.IO.FileAccess.Read, Yandex.PAL.IO.FileShare.None))
            return this.DeserializeModel(inStream);
        }
      }
      catch (FileNotFoundException ex)
      {
        return default (TModel);
      }
      catch (Exception ex)
      {
        if (ex.InnerException is FileNotFoundException)
          return default (TModel);
        throw;
      }
    }
  }
}
