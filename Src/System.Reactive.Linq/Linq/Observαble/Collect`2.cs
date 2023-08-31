// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Collect`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive.Linq.Observαble
{
  internal class Collect<TSource, TResult> : PushToPullAdapter<TSource, TResult>
  {
    private readonly Func<TResult> _getInitialCollector;
    private readonly Func<TResult, TSource, TResult> _merge;
    private readonly Func<TResult, TResult> _getNewCollector;

    public Collect(
      IObservable<TSource> source,
      Func<TResult> getInitialCollector,
      Func<TResult, TSource, TResult> merge,
      Func<TResult, TResult> getNewCollector)
      : base(source)
    {
      this._getInitialCollector = getInitialCollector;
      this._merge = merge;
      this._getNewCollector = getNewCollector;
    }

    protected override PushToPullSink<TSource, TResult> Run(IDisposable subscription)
    {
      Collect<TSource, TResult>._ obj = new Collect<TSource, TResult>._(this, subscription);
      obj.Initialize();
      return (PushToPullSink<TSource, TResult>) obj;
    }

    private class _ : PushToPullSink<TSource, TResult>
    {
      private readonly Collect<TSource, TResult> _parent;
      private object _gate;
      private TResult _collector;
      private bool _hasFailed;
      private Exception _error;
      private bool _hasCompleted;
      private bool _done;

      public _(Collect<TSource, TResult> parent, IDisposable subscription)
        : base(subscription)
      {
        this._parent = parent;
      }

      public void Initialize()
      {
        this._gate = new object();
        this._collector = this._parent._getInitialCollector();
      }

      public override void OnNext(TSource value)
      {
        lock (this._gate)
        {
          try
          {
            this._collector = this._parent._merge(this._collector, value);
          }
          catch (Exception ex)
          {
            this._error = ex;
            this._hasFailed = true;
            this.Dispose();
          }
        }
      }

      public override void OnError(Exception error)
      {
        this.Dispose();
        lock (this._gate)
        {
          this._error = error;
          this._hasFailed = true;
        }
      }

      public override void OnCompleted()
      {
        this.Dispose();
        lock (this._gate)
          this._hasCompleted = true;
      }

      public override bool TryMoveNext(out TResult current)
      {
        lock (this._gate)
        {
          if (this._hasFailed)
          {
            current = default (TResult);
            this._error.Throw();
          }
          else if (this._hasCompleted)
          {
            if (this._done)
            {
              current = default (TResult);
              return false;
            }
            current = this._collector;
            this._done = true;
          }
          else
          {
            current = this._collector;
            try
            {
              this._collector = this._parent._getNewCollector(current);
            }
            catch
            {
              this.Dispose();
              throw;
            }
          }
          return true;
        }
      }
    }
  }
}
