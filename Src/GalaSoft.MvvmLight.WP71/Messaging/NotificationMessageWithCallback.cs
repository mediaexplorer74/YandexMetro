// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.NotificationMessageWithCallback
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

using System;

namespace GalaSoft.MvvmLight.Messaging
{
  public class NotificationMessageWithCallback : NotificationMessage
  {
    private readonly Delegate _callback;

    public NotificationMessageWithCallback(string notification, Delegate callback)
      : base(notification)
    {
      NotificationMessageWithCallback.CheckCallback(callback);
      this._callback = callback;
    }

    public NotificationMessageWithCallback(object sender, string notification, Delegate callback)
      : base(sender, notification)
    {
      NotificationMessageWithCallback.CheckCallback(callback);
      this._callback = callback;
    }

    public NotificationMessageWithCallback(
      object sender,
      object target,
      string notification,
      Delegate callback)
      : base(sender, target, notification)
    {
      NotificationMessageWithCallback.CheckCallback(callback);
      this._callback = callback;
    }

    public virtual object Execute(params object[] arguments) => this._callback.DynamicInvoke(arguments);

    private static void CheckCallback(Delegate callback)
    {
      if ((object) callback == null)
        throw new ArgumentNullException(nameof (callback), "Callback may not be null");
    }
  }
}
