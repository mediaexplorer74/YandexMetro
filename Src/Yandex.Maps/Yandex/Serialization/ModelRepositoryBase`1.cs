// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.ModelRepositoryBase`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System.ComponentModel;
using Yandex.IO;
using Yandex.Serialization.Interfaces;

namespace Yandex.Serialization
{
  internal class ModelRepositoryBase<TModel> : 
    RepositoryBase<TModel>,
    IModelRepositoryBase<TModel>,
    IRepositoryBase<TModel>,
    IReadOnlyRepositoryBase<TModel>
    where TModel : class, INotifyPropertyChanged, new()
  {
    public ModelRepositoryBase(
      [NotNull] IGenericXmlSerializer<TModel> modelSerializer,
      [NotNull] IFileStorage fileStorage)
      : base(modelSerializer, fileStorage)
    {
    }

    public event PropertyChangedEventHandler ModelPropertyChanged;

    public override TModel Model
    {
      get => base.Model;
      set
      {
        if ((object) this._model == (object) value)
          return;
        if ((object) this._model != null)
          this._model.PropertyChanged -= new PropertyChangedEventHandler(this.MainModelPropertyChanged);
        this._model = value;
        this._model.PropertyChanged += new PropertyChangedEventHandler(this.MainModelPropertyChanged);
      }
    }

    protected virtual void OnModelPropertyChanged(PropertyChangedEventArgs args)
    {
      PropertyChangedEventHandler modelPropertyChanged = this.ModelPropertyChanged;
      if (modelPropertyChanged == null)
        return;
      modelPropertyChanged((object) this, args);
    }

    private void MainModelPropertyChanged(object sender, PropertyChangedEventArgs args) => this.OnModelPropertyChanged(args);
  }
}
