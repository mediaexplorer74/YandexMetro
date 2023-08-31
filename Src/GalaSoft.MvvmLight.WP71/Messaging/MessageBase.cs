// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.MessageBase
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

namespace GalaSoft.MvvmLight.Messaging
{
  public class MessageBase
  {
    public MessageBase()
    {
    }

    public MessageBase(object sender) => this.Sender = sender;

    public MessageBase(object sender, object target)
      : this(sender)
    {
      this.Target = target;
    }

    public object Sender { get; protected set; }

    public object Target { get; protected set; }
  }
}
