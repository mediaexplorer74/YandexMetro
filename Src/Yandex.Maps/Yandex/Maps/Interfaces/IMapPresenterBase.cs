// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Interfaces.IMapPresenterBase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Yandex.Maps.Events;
using Yandex.Media;
using Yandex.Positioning;

namespace Yandex.Maps.Interfaces
{
  internal interface IMapPresenterBase : IMapState, INotifyPropertyChanged, IDisposable
  {
    event EventHandler<OperationStatusChangedEventArgs> OperationStatusChanged;

    Thickness ContentPadding { get; set; }

    void Connect();

    void EnsureControlIsVisible(object mapChild);

    void EnsureRectangleIsVisible(GeoCoordinatesRect rect);

    void EnsureRectangleIsVisible(RelativeRect rect);

    void EnsureVisibility(IEnumerable<object> mapChildren);

    OperationMode OperationMode { get; }

    void DisableManipulations();

    void EnableManipulations();

    void DisableMapReload();

    void EnableMapReload();

    void CenterOnControl(object mapChild);
  }
}
