// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SynchronizationContextExtensions
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Threading;

namespace System.Reactive.Concurrency
{
  internal static class SynchronizationContextExtensions
  {
    public static void PostWithStartComplete<T>(
      this SynchronizationContext context,
      Action<T> action,
      T state)
    {
      context.OperationStarted();
      context.Post((SendOrPostCallback) (o =>
      {
        try
        {
          action((T) o);
        }
        finally
        {
          context.OperationCompleted();
        }
      }), (object) state);
    }

    public static void PostWithStartComplete(this SynchronizationContext context, Action action)
    {
      context.OperationStarted();
      context.Post((SendOrPostCallback) (_ =>
      {
        try
        {
          action();
        }
        finally
        {
          context.OperationCompleted();
        }
      }), (object) null);
    }
  }
}
