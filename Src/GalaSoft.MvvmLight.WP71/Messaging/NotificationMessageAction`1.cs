// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.NotificationMessageAction`1
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

using System;

namespace GalaSoft.MvvmLight.Messaging
{
  public class NotificationMessageAction<TCallbackParameter> : NotificationMessageWithCallback
  {
    public NotificationMessageAction(string notification, Action<TCallbackParameter> callback)
      : base(notification, (Delegate) callback)
    {
    }

    public NotificationMessageAction(
      object sender,
      string notification,
      Action<TCallbackParameter> callback)
      : base(sender, notification, (Delegate) callback)
    {
    }

    public NotificationMessageAction(
      object sender,
      object target,
      string notification,
      Action<TCallbackParameter> callback)
      : base(sender, target, notification, (Delegate) callback)
    {
    }

    public void Execute(TCallbackParameter parameter) => this.Execute((object) parameter);
  }
}
