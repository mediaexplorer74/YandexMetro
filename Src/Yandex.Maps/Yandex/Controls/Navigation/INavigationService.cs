// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Navigation.INavigationService
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Controls;

namespace Yandex.Controls.Navigation
{
  internal interface INavigationService
  {
    void Navigate(NavigationState state);

    void SaveStates();

    void Init(PhoneApplicationFrame rootFrame, NavigationState initialState);

    void LoadStates();

    bool CanGoBack { get; }

    void GoBack();

    void RemoveBackEntry();
  }
}
