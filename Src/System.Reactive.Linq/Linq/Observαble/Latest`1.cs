// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Latest`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Threading;

namespace System.Reactive.Linq.Observαble
{
  internal class Latest<TSource> : PushToPullAdapter<TSource, TSource>
  {
    public Latest(IObservable<TSource> source)
      : base(source)
    {
    }

    protected override PushToPullSink<TSource, TSource> Run(IDisposable subscription) => (PushToPullSink<TSource, TSource>) new Latest<TSource>._(subscription);

    private class _ : PushToPullSink<TSource, TSource>
    {
      private readonly object _gate;
      private readonly Semaphore _semaphore;
      private bool _notificationAvailable;
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
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._notificationAvailable;
          this._notificationAvailable = true;
          this._kind = NotificationKind.OnNext;
          this._value = value;
        }
        if (!flag)
          return;
        this._semaphore.Release();
      }

      public override void OnError(Exception error)
      {
        this.Dispose();
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._notificationAvailable;
          this._notificationAvailable = true;
          this._kind = NotificationKind.OnError;
          this._error = error;
        }
        if (!flag)
          return;
        this._semaphore.Release();
      }

      public override void OnCompleted()
      {
        this.Dispose();
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._notificationAvailable;
          this._notificationAvailable = true;
          this._kind = NotificationKind.OnCompleted;
        }
        if (!flag)
          return;
        this._semaphore.Release();
      }

      public override bool TryMoveNext(out TSource current)
      {
        NotificationKind notificationKind = NotificationKind.OnNext;
        Exception exception = (Exception) null;
        this._semaphore.WaitOne();
        lock (this._gate)
        {
          notificationKind = this._kind;
          switch (notificationKind)
          {
            case NotificationKind.OnError:
              exception = this._error;
              break;
          }
          this._notificationAvailable = false;
        }
        switch (notificationKind)
        {
          case NotificationKind.OnNext:
            current = this._value;
            return true;
          case NotificationKind.OnError:
            exception.Throw();
            break;
        }
        current = default (TSource);
        return false;
      }
    }
  }
}
