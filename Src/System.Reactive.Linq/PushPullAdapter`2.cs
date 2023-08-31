// Decompiled with JetBrains decompiler
// Type: System.Reactive.PushPullAdapter`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Reactive
{
  internal sealed class PushPullAdapter<T, R> : 
    IObserver<T>,
    IEnumerator<R>,
    IDisposable,
    IEnumerator
  {
    private Action<Notification<T>> yield;
    private Action dispose;
    private Func<Notification<R>> moveNext;
    private Notification<R> current;
    private bool done;
    private bool disposed;

    public PushPullAdapter(
      Action<Notification<T>> yield,
      Func<Notification<R>> moveNext,
      Action dispose)
    {
      this.yield = yield;
      this.moveNext = moveNext;
      this.dispose = dispose;
    }

    public void OnNext(T value) => this.yield(Notification.CreateOnNext<T>(value));

    public void OnError(Exception exception)
    {
      this.yield(Notification.CreateOnError<T>(exception));
      this.dispose();
    }

    public void OnCompleted()
    {
      this.yield(Notification.CreateOnCompleted<T>());
      this.dispose();
    }

    public R Current => this.current.Value;

    public void Dispose()
    {
      this.disposed = true;
      this.dispose();
    }

    object IEnumerator.Current => (object) this.Current;

    public bool MoveNext()
    {
      if (this.disposed)
        throw new ObjectDisposedException("");
      if (!this.done)
      {
        this.current = this.moveNext();
        this.done = this.current.Kind != NotificationKind.OnNext;
      }
      this.current.Exception.ThrowIfNotNull();
      return this.current.HasValue;
    }

    public void Reset() => throw new NotSupportedException();
  }
}
