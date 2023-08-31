// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ViewModels.MapItemViewModel
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.ComponentModel;
using Yandex.Positioning;

namespace Yandex.Maps.ViewModels
{
  internal class MapItemViewModel : ViewModelBase
  {
    public const string PositionPropertyName = "Position";
    public const string AlignmentPropertyName = "Alignment";
    private GeoCoordinate _position;
    private Alignment _alignment;

    public GeoCoordinate Position
    {
      get => this._position;
      set
      {
        if (object.Equals((object) this._position, (object) value))
          return;
        this._position = value;
        this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (Position)));
      }
    }

    public Alignment Alignment
    {
      get => this._alignment;
      set
      {
        if (this._alignment == value)
          return;
        this._alignment = value;
        this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (Alignment)));
      }
    }
  }
}
