// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.Navigation.LoggedNavigationService
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Controls;
using Yandex.Common;
using Yandex.Serialization.Interfaces;

namespace Yandex.Controls.Navigation
{
  internal class LoggedNavigationService : NavigationService
  {
    public LoggedNavigationService(
      IGenericXmlSerializer<NavigationState[]> modelSerializer)
      : base(modelSerializer)
    {
    }

    public override void Navigate(NavigationState state)
    {
      Logger.SendActivity(state.Uri.ToString());
      base.Navigate(state);
    }

    public override void Init(PhoneApplicationFrame rootFrame, NavigationState initialState)
    {
      base.Init(rootFrame, initialState);
      Logger.SendActivity(initialState.Uri.ToString());
    }
  }
}
