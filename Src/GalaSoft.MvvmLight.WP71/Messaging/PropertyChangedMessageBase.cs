// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.PropertyChangedMessageBase
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

namespace GalaSoft.MvvmLight.Messaging
{
  public abstract class PropertyChangedMessageBase : MessageBase
  {
    protected PropertyChangedMessageBase(object sender, string propertyName)
      : base(sender)
    {
      this.PropertyName = propertyName;
    }

    protected PropertyChangedMessageBase(object sender, object target, string propertyName)
      : base(sender, target)
    {
      this.PropertyName = propertyName;
    }

    protected PropertyChangedMessageBase(string propertyName) => this.PropertyName = propertyName;

    public string PropertyName { get; protected set; }
  }
}
