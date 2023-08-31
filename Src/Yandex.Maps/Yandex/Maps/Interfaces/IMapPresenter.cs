// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Interfaces.IMapPresenter
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.ComponentModel;
using Yandex.Maps.Controls.Events;

namespace Yandex.Maps.Interfaces
{
  internal interface IMapPresenter : 
    IMapPresenterBase,
    INotifyPropertyChanged,
    IDisposable,
    IMap,
    IMapState
  {
    event EventHandler<TrafficAvailableChangedArgs> TrafficAvailbleChanged;

    event EventHandler<TrafficValueChangedArgs> TrafficValueChanged;

    bool UseLocation { get; set; }
  }
}
