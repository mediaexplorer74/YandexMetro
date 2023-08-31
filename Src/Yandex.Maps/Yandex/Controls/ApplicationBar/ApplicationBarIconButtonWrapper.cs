// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.ApplicationBar.ApplicationBarIconButtonWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Microsoft.Phone.Shell;
using System;
using System.Windows;

namespace Yandex.Controls.ApplicationBar
{
  internal class ApplicationBarIconButtonWrapper : 
    ApplicationBarMenuItemGenericWrapper<ApplicationBarIconButton>
  {
    public static readonly DependencyProperty IconUriProperty = DependencyProperty.Register(nameof (IconUri), typeof (Uri), typeof (ApplicationBarIconButtonWrapper), new PropertyMetadata(new PropertyChangedCallback(ApplicationBarIconButtonWrapper.IconUriPropertyChangedCallback)));

    public Uri IconUri
    {
      get => (Uri) ((DependencyObject) this).GetValue(ApplicationBarIconButtonWrapper.IconUriProperty);
      set => ((DependencyObject) this).SetValue(ApplicationBarIconButtonWrapper.IconUriProperty, (object) value);
    }

    public ApplicationBarIconButtonWrapper()
      : base(new ApplicationBarIconButton())
    {
    }

    public ApplicationBarIconButtonWrapper(ApplicationBarIconButton applicationBarIconButton)
      : base(applicationBarIconButton)
    {
    }

    private static void IconUriPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is ApplicationBarIconButtonWrapper iconButtonWrapper))
        return;
      iconButtonWrapper.ApplicationBarMenuItem.IconUri = (Uri) e.NewValue;
    }
  }
}
