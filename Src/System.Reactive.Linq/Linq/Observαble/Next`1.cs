// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Next`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Threading;

namespace System.Reactive.Linq.Observαble
{
  internal class Next<TSource> : PushToPullAdapter<TSource, TSource>
  {
    public Next(IObservable<TSource> source)
      : base(source)
    {
    }

    protected override PushToPullSink<TSource, TSource> Run(IDisposable subscription) => (PushToPullSink<TSource, TSource>) new Next<TSource>._(subscription);

    private class _ : PushToPullSink<TSource, TSource>
    {
      private readonly object _gate;
      private readonly Semaphore _semaphore;
      private bool _waiting;
      private NotificationKind _kind;
      private TSource _value;
      private Exception _error;

      public _(IDisposable subscription)
        : base(subscription)
      {
        this._gate = new object();
        this._semaphore = new Semaphore(0, 1);
      }

      public override void OnNext(TSource value)
      {
        lock (this._gate)
        {
          if (this._waiting)
          {
            this._value = value;
            this._kind = NotificationKind.OnNext;
            this._semaphore.Release();
          }
          this._waiting = false;
        }
      }

      public override void OnError(Exception error)
      {
        this.Dispose();
        lock (this._gate)
        {
          this._error = error;
          this._kind = NotificationKind.OnError;
          if (this._waiting)
            this._semaphore.Release();
          this._waiting = false;
        }
      }

      public override void OnCompleted()
      {
        this.Dispose();
        lock (this._gate)
        {
          this._kind = NotificationKind.OnCompleted;
          if (this._waiting)
            this._semaphore.Release();
          this._waiting = false;
        }
      }

      public override bool TryMoveNext(out TSource current)
      {
        bool flag = false;
        lock (this._gate)
        {
          this._waiting = true;
          flag = this._kind != NotificationKind.OnNext;
        }
        if (!flag)
          this._semaphore.WaitOne();
        switch (this._kind)
        {
          case NotificationKind.OnNext:
            current = this._value;
            return true;
          case NotificationKind.OnError:
            this._error.Throw();
            break;
        }
        current = default (TSource);
        return false;
      }
    }
  }
}
