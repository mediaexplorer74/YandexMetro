// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ViewModel.PushPinViewModelBase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Yandex.Maps.ViewModels;

namespace Yandex.Maps.ViewModel
{
  internal class PushPinViewModelBase : MapItemViewModel
  {
    public const string BaloonViewModelPropertyName = "BaloonViewModel";
    public const string StatePropertyName = "State";
    public const string ContentVisibilityPropertyName = "ContentVisibility";
    public const string VisibilityPropertyName = "Visibility";
    public const string IsAdvertisementPropertyName = "IsAdvertisement";
    public const string GroupKeysPropertyName = "GroupKeys";
    private Visibility _contentVisibility;
    private PushPinState _state;
    private Visibility _visibility;
    private bool _isAdvertisement;
    private Collection<string> _groupKeys;

    public PushPinState State
    {
      get => this._state;
      set
      {
        if (this._state == value)
          return;
        this._state = value;
        this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (State)));
      }
    }

    public Visibility ContentVisibility
    {
      get => this._contentVisibility;
      set
      {
        if (this._contentVisibility == value)
          return;
        this._contentVisibility = value;
        this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (ContentVisibility)));
      }
    }

    public Visibility Visibility
    {
      get => this._visibility;
      set
      {
        if (this._visibility == value)
          return;
        this._visibility = value;
        this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (Visibility)));
      }
    }

    public bool IsAdvertisement
    {
      get => this._isAdvertisement;
      set
      {
        if (this._isAdvertisement == value)
          return;
        this._isAdvertisement = value;
        this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (IsAdvertisement)));
      }
    }

    [TypeConverter(typeof (GroupKeysConverter))]
    public Collection<string> GroupKeys
    {
      get => this._groupKeys;
      set
      {
        this._groupKeys = value;
        this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (GroupKeys)));
      }
    }
  }
}
