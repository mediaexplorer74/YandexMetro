// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.ForEach`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Threading;

namespace System.Reactive.Linq.Observαble
{
  internal class ForEach<TSource>
  {
    public class _ : IObserver<TSource>
    {
      private readonly Action<TSource> _onNext;
      private readonly Action _done;
      private Exception _exception;
      private int _stopped;

      public _(Action<TSource> onNext, Action done)
      {
        this._onNext = onNext;
        this._done = done;
        this._stopped = 0;
      }

      public Exception Error => this._exception;

      public void OnNext(TSource value)
      {
        if (this._stopped != 0)
          return;
        try
        {
          this._onNext(value);
        }
        catch (Exception ex)
        {
          this.OnError(ex);
        }
      }

      public void OnError(Exception error)
      {
        if (Interlocked.Exchange(ref this._stopped, 1) != 0)
          return;
        this._exception = error;
        this._done();
      }

      public void OnCompleted()
      {
        if (Interlocked.Exchange(ref this._stopped, 1) != 0)
          return;
        this._done();
      }
    }

    public class τ : IObserver<TSource>
    {
      private readonly Action<TSource, int> _onNext;
      private readonly Action _done;
      private int _index;
      private Exception _exception;
      private int _stopped;

      public τ(Action<TSource, int> onNext, Action done)
      {
        this._onNext = onNext;
        this._done = done;
        this._index = 0;
        this._stopped = 0;
      }

      public Exception Error => this._exception;

      public void OnNext(TSource value)
      {
        if (this._stopped != 0)
          return;
        try
        {
          this._onNext(value, checked (this._index++));
        }
        catch (Exception ex)
        {
          this.OnError(ex);
        }
      }

      public void OnError(Exception error)
      {
        if (Interlocked.Exchange(ref this._stopped, 1) != 0)
          return;
        this._exception = error;
        this._done();
      }

      public void OnCompleted()
      {
        if (Interlocked.Exchange(ref this._stopped, 1) != 0)
          return;
        this._done();
      }
    }
  }
}
