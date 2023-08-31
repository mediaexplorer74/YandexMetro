// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.ExtendedPhoneApplicationPage
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Controls;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Yandex.Controls
{
  internal class ExtendedPhoneApplicationPage : PhoneApplicationPage
  {
    public event EventHandler<NavigationEventArgs> NavigatedTo;

    public void FireNavigatedTo(NavigationEventArgs e)
    {
      EventHandler<NavigationEventArgs> navigatedTo = this.NavigatedTo;
      if (navigatedTo == null)
        return;
      navigatedTo((object) this, e);
    }

    public event EventHandler<NavigationEventArgs> NavigatedFrom;

    public void FireNavigatedFrom(NavigationEventArgs e)
    {
      EventHandler<NavigationEventArgs> navigatedFrom = this.NavigatedFrom;
      if (navigatedFrom == null)
        return;
      navigatedFrom((object) this, e);
    }

    public event EventHandler<NavigatingCancelEventArgs> NavigatingFrom;

    public void FireNavigatingFrom(NavigatingCancelEventArgs e)
    {
      EventHandler<NavigatingCancelEventArgs> navigatingFrom = this.NavigatingFrom;
      if (navigatingFrom == null)
        return;
      navigatingFrom((object) this, e);
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      this.FireNavigatedTo(e);
      ((Page) this).OnNavigatedTo(e);
    }

    protected virtual void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
      this.FireNavigatingFrom(e);
      ((Page) this).OnNavigatingFrom(e);
    }

    protected virtual void OnNavigatedFrom(NavigationEventArgs e)
    {
      this.FireNavigatedFrom(e);
      ((Page) this).OnNavigatedFrom(e);
    }
  }
}
