// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.NotificationMessage
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

namespace GalaSoft.MvvmLight.Messaging
{
  public class NotificationMessage : MessageBase
  {
    public NotificationMessage(string notification) => this.Notification = notification;

    public NotificationMessage(object sender, string notification)
      : base(sender)
    {
      this.Notification = notification;
    }

    public NotificationMessage(object sender, object target, string notification)
      : base(sender, target)
    {
      this.Notification = notification;
    }

    public string Notification { get; private set; }
  }
}
