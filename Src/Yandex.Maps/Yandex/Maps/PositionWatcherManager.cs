// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PositionWatcherManager
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.ComponentModel;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.Interfaces;
using Yandex.Positioning.Interfaces;

namespace Yandex.Maps
{
  [UsedImplicitly]
  internal class PositionWatcherManager : IPositionWatcherManager
  {
    private readonly IConfigMediator _configMediator;
    private readonly IPositionWatcher _positionWatcher;

    public PositionWatcherManager([NotNull] IConfigMediator configMediator, [NotNull] IPositionWatcher positionWatcher)
    {
      if (configMediator == null)
        throw new ArgumentNullException(nameof (configMediator));
      if (positionWatcher == null)
        throw new ArgumentNullException(nameof (positionWatcher));
      this._configMediator = configMediator;
      this._positionWatcher = positionWatcher;
      this._configMediator.PropertyChanged += new PropertyChangedEventHandler(this.ConfigMediatorPropertyChanged);
      this.UpdatePositionWatcher();
    }

    private void ConfigMediatorPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "UseLocation"))
        return;
      this.UpdatePositionWatcher();
    }

    private void UpdatePositionWatcher() => this._positionWatcher.Enabled = this._configMediator.EnableLocationService;
  }
}
