// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.PushToPullSink`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal abstract class PushToPullSink<TSource, TResult> : 
    IObserver<TSource>,
    IEnumerator<TResult>,
    IEnumerator,
    IDisposable
  {
    private readonly IDisposable _subscription;
    private bool _done;

    public PushToPullSink(IDisposable subscription) => this._subscription = subscription;

    public abstract void OnNext(TSource value);

    public abstract void OnError(Exception error);

    public abstract void OnCompleted();

    public abstract bool TryMoveNext(out TResult current);

    public bool MoveNext()
    {
      if (!this._done)
      {
        TResult current = default (TResult);
        if (this.TryMoveNext(out current))
        {
          this.Current = current;
          return true;
        }
        this._done = true;
        this._subscription.Dispose();
      }
      return false;
    }

    public TResult Current { get; private set; }

    object IEnumerator.Current => (object) this.Current;

    public void Reset() => throw new NotSupportedException();

    public void Dispose() => this._subscription.Dispose();
  }
}
