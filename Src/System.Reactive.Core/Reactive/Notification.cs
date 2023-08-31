// Decompiled with JetBrains decompiler
// Type: System.Reactive.Notification
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive
{
  public static class Notification
  {
    public static Notification<T> CreateOnNext<T>(T value) => (Notification<T>) new Notification<T>.OnNextNotification(value);

    public static Notification<T> CreateOnError<T>(Exception error) => error != null ? (Notification<T>) new Notification<T>.OnErrorNotification(error) : throw new ArgumentNullException(nameof (error));

    public static Notification<T> CreateOnCompleted<T>() => (Notification<T>) new Notification<T>.OnCompletedNotification();
  }
}
