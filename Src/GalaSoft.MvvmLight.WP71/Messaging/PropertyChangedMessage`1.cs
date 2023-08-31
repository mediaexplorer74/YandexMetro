// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.PropertyChangedMessage`1
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

namespace GalaSoft.MvvmLight.Messaging
{
  public class PropertyChangedMessage<T> : PropertyChangedMessageBase
  {
    public PropertyChangedMessage(object sender, T oldValue, T newValue, string propertyName)
      : base(sender, propertyName)
    {
      this.OldValue = oldValue;
      this.NewValue = newValue;
    }

    public PropertyChangedMessage(T oldValue, T newValue, string propertyName)
      : base(propertyName)
    {
      this.OldValue = oldValue;
      this.NewValue = newValue;
    }

    public PropertyChangedMessage(
      object sender,
      object target,
      T oldValue,
      T newValue,
      string propertyName)
      : base(sender, target, propertyName)
    {
      this.OldValue = oldValue;
      this.NewValue = newValue;
    }

    public T NewValue { get; private set; }

    public T OldValue { get; private set; }
  }
}
