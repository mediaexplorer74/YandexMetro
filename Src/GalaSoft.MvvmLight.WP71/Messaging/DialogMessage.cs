// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.DialogMessage
// Assembly: GalaSoft.MvvmLight.WP71, Version=3.0.0.19988, Culture=neutral, PublicKeyToken=null
// MVID: FEAEB788-B688-4545-AAB4-A8BE1A48D352
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\GalaSoft.MvvmLight.WP71.dll

using System;
using System.Windows;

namespace GalaSoft.MvvmLight.Messaging
{
  public class DialogMessage : GenericMessage<string>
  {
    public DialogMessage(string content, Action<MessageBoxResult> callback)
      : base(content)
    {
      this.Callback = callback;
    }

    public DialogMessage(object sender, string content, Action<MessageBoxResult> callback)
      : base(sender, content)
    {
      this.Callback = callback;
    }

    public DialogMessage(
      object sender,
      object target,
      string content,
      Action<MessageBoxResult> callback)
      : base(sender, target, content)
    {
      this.Callback = callback;
    }

    public MessageBoxButton Button { get; set; }

    public Action<MessageBoxResult> Callback { get; private set; }

    public string Caption { get; set; }

    public MessageBoxResult DefaultResult { get; set; }

    public void ProcessCallback(MessageBoxResult result)
    {
      if (this.Callback == null)
        return;
      this.Callback(result);
    }
  }
}
