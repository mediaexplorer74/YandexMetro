// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.NotificationMessage`1
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

namespace GalaSoft.MvvmLight.Messaging
{
  public class NotificationMessage<T> : GenericMessage<T>
  {
    public NotificationMessage(T content, string notification)
      : base(content)
    {
      this.Notification = notification;
    }

    public NotificationMessage(object sender, T content, string notification)
      : base(sender, content)
    {
      this.Notification = notification;
    }

    public NotificationMessage(object sender, object target, T content, string notification)
      : base(sender, target, content)
    {
      this.Notification = notification;
    }

    public string Notification { get; private set; }
  }
}
