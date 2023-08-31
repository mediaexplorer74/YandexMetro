// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.ViewModel.PushPinViewModel`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.ComponentModel;

namespace Yandex.Maps.ViewModel
{
  internal class PushPinViewModel<TBalloonViewModel> : PushPinViewModelBase where TBalloonViewModel : new()
  {
    private TBalloonViewModel _balloonViewModel;

    public PushPinViewModel() => this._balloonViewModel = new TBalloonViewModel();

    public TBalloonViewModel BaloonViewModel
    {
      get => this._balloonViewModel;
      set
      {
        if ((object) this._balloonViewModel != null && this._balloonViewModel.Equals((object) value))
          return;
        this._balloonViewModel = value;
        this.OnPropertyChanged(new PropertyChangedEventArgs(nameof (BaloonViewModel)));
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (object.ReferenceEquals(obj, (object) this))
        return true;
      if (!(obj is PushPinViewModel<TBalloonViewModel> pushPinViewModel))
        return false;
      TBalloonViewModel baloonViewModel = this.BaloonViewModel;
      return (object) baloonViewModel == null ? null == (object) pushPinViewModel.BaloonViewModel : baloonViewModel.Equals((object) pushPinViewModel.BaloonViewModel);
    }

    public override int GetHashCode() => this.BaloonViewModel.GetHashCode();
  }
}
