// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.IMessenger
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

using System;

namespace GalaSoft.MvvmLight.Messaging
{
  public interface IMessenger
  {
    void Register<TMessage>(object recipient, Action<TMessage> action);

    void Register<TMessage>(
      object recipient,
      bool receiveDerivedMessagesToo,
      Action<TMessage> action);

    void Send<TMessage>(TMessage message);

    void Send<TMessage, TTarget>(TMessage message);

    void Unregister(object recipient);

    void Unregister<TMessage>(object recipient);

    void Unregister<TMessage>(object recipient, Action<TMessage> action);
  }
}
