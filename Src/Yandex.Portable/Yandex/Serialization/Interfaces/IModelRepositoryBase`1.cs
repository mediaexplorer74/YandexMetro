// Decompiled with JetBrains decompiler
// Type: Yandex.Serialization.Interfaces.IModelRepositoryBase`1
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

using JetBrains.Annotations;
using System.ComponentModel;

namespace Yandex.Serialization.Interfaces
{
  public interface IModelRepositoryBase<TModel> where TModel : class, INotifyPropertyChanged, new()
  {
    event PropertyChangedEventHandler ModelPropertyChanged;

    [NotNull]
    TModel Model { get; set; }

    bool IsFirstAppRun { get; }

    void SaveModel();
  }
}
