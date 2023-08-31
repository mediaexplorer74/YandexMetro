// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Geocoding.GeocodeUrlProvider
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using Yandex.Maps.Config.Interfaces;
using Yandex.Maps.Geocoding.Interfaces;
using Yandex.Patterns;

namespace Yandex.Maps.Geocoding
{
  internal class GeocodeUrlProvider : IGeocodeUrlProvider, IStateService
  {
    private readonly IPrinterUrlProvider _printerUrlProvider;

    public GeocodeUrlProvider([NotNull] IPrinterUrlProvider printerUrlProvider) => this._printerUrlProvider = printerUrlProvider != null ? printerUrlProvider : throw new ArgumentNullException(nameof (printerUrlProvider));

    public string GetBaseUrl() => this._printerUrlProvider.GetGeocodeUrl();

    public event EventHandler<StateChangedEventArgs> StateChanged
    {
      add => this._printerUrlProvider.StateChanged += value;
      remove => this._printerUrlProvider.StateChanged -= value;
    }

    public ServiceState State => this._printerUrlProvider.State;
  }
}
