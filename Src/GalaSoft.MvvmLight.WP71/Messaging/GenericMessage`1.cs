// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.GenericMessage`1
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

namespace GalaSoft.MvvmLight.Messaging
{
  public class GenericMessage<T> : MessageBase
  {
    public GenericMessage(T content) => this.Content = content;

    public GenericMessage(object sender, T content)
      : base(sender)
    {
      this.Content = content;
    }

    public GenericMessage(object sender, object target, T content)
      : base(sender, target)
    {
      this.Content = content;
    }

    public T Content { get; protected set; }
  }
}
