// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.MostRecent`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class MostRecent<TSource> : PushToPullAdapter<TSource, TSource>
  {
    private readonly TSource _initialValue;

    public MostRecent(IObservable<TSource> source, TSource initialValue)
      : base(source)
    {
      this._initialValue = initialValue;
    }

    protected override PushToPullSink<TSource, TSource> Run(IDisposable subscription) => (PushToPullSink<TSource, TSource>) new MostRecent<TSource>._(this._initialValue, subscription);

    private class _ : PushToPullSink<TSource, TSource>
    {
      private volatile NotificationKind _kind;
      private TSource _value;
      private Exception _error;

      public _(TSource initialValue, IDisposable subscription)
        : base(subscription)
      {
        this._kind = NotificationKind.OnNext;
        this._value = initialValue;
      }

      public override void OnNext(TSource value)
      {
        this._value = value;
        this._kind = NotificationKind.OnNext;
      }

      public override void OnError(Exception error)
      {
        this.Dispose();
        this._error = error;
        this._kind = NotificationKind.OnError;
      }

      public override void OnCompleted()
      {
        this.Dispose();
        this._kind = NotificationKind.OnCompleted;
      }

      public override bool TryMoveNext(out TSource current)
      {
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
