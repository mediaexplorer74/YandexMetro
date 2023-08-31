// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.ModelRepositoryBase`1
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.IO;
using Yandex.Common;
using Yandex.IO;
using Yandex.PAL.IO;
using Yandex.Serialization.Interfaces;

namespace Yandex.Serialization
{
  public class ModelRepositoryBase<TModel> : IModelRepositoryBase<TModel> where TModel : class, INotifyPropertyChanged, new()
  {
    internal static readonly string ModelFileName = typeof (TModel).Name;
    internal static readonly string ModelTempFileName = typeof (TModel).Name + "Temp";
    private readonly IFileStorage _fileStorage;
    private readonly object _modelLock = new object();
    private readonly IGenericXmlSerializer<TModel> _modelSerializer;
    private TModel _model;
    private bool _isFirstAppRun;

    public ModelRepositoryBase(
      [NotNull] IGenericXmlSerializer<TModel> modelSerializer,
      [NotNull] IFileStorage fileStorage)
    {
      if (modelSerializer == null)
        throw new ArgumentNullException(nameof (modelSerializer));
      if (fileStorage == null)
        throw new ArgumentNullException(nameof (fileStorage));
      this._modelSerializer = modelSerializer;
      this._fileStorage = fileStorage;
      this.IsFirstAppRun = true;
    }

    public event PropertyChangedEventHandler ModelPropertyChanged;

    public TModel Model
    {
      get
      {
        this.InitializeModelSafe();
        return this._model;
      }
      set
      {
        if ((object) this._model != null)
          this._model.PropertyChanged -= new PropertyChangedEventHandler(this.MainModelPropertyChanged);
        this._model = value;
        this._model.PropertyChanged += new PropertyChangedEventHandler(this.MainModelPropertyChanged);
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
        this.InitializeModel();
      }
    }

    public void SaveModel()
    {
      if ((object) this._model == null)
        return;
      try
      {
        using (Stream outStream = this._fileStorage.OpenFile(ModelRepositoryBase<TModel>.ModelTempFileName, FileMode.Create, FileAccess.Write, FileShare.None))
          this.SerializeModel(outStream);
        if (this._fileStorage.FileExists(ModelRepositoryBase<TModel>.ModelFileName))
          this._fileStorage.DeleteFile(ModelRepositoryBase<TModel>.ModelFileName);
        this._fileStorage.MoveFile(ModelRepositoryBase<TModel>.ModelTempFileName, ModelRepositoryBase<TModel>.ModelFileName);
      }
      catch (Exception ex)
      {
        Logger.SendError(ex.InnerException ?? ex);
        if (!this._fileStorage.FileExists(ModelRepositoryBase<TModel>.ModelTempFileName))
          return;
        this._fileStorage.DeleteFile(ModelRepositoryBase<TModel>.ModelTempFileName);
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

    public virtual void InitializeModel()
    {
      TModel model = this.TryLoadModel(ModelRepositoryBase<TModel>.ModelTempFileName) ?? this.TryLoadModel(ModelRepositoryBase<TModel>.ModelFileName);
      if ((object) model != null)
        this.IsFirstAppRun = false;
      else
        model = new TModel();
      this.Model = model;
    }

    [CanBeNull]
    private TModel TryLoadModel(string modelFileName)
    {
      if (!this._fileStorage.FileExists(modelFileName))
        return default (TModel);
      using (Stream inStream = this._fileStorage.OpenFile(modelFileName, FileMode.Open, FileAccess.Read, FileShare.None))
        return this.DeserializeModel(inStream);
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

    private void MainModelPropertyChanged(object sender, PropertyChangedEventArgs e) => this.OnModelPropertyChanged(e);

    protected virtual void OnModelPropertyChanged(PropertyChangedEventArgs e)
    {
      if (this.ModelPropertyChanged == null)
        return;
      this.ModelPropertyChanged((object) this, e);
    }
  }
}
