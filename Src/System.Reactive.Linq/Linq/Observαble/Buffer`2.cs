// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Buffer`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class Buffer<TSource, TBufferClosing> : Producer<IList<TSource>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<IObservable<TBufferClosing>> _bufferClosingSelector;
    private readonly IObservable<TBufferClosing> _bufferBoundaries;

    public Buffer(
      IObservable<TSource> source,
      Func<IObservable<TBufferClosing>> bufferClosingSelector)
    {
      this._source = source;
      this._bufferClosingSelector = bufferClosingSelector;
    }

    public Buffer(IObservable<TSource> source, IObservable<TBufferClosing> bufferBoundaries)
    {
      this._source = source;
      this._bufferBoundaries = bufferBoundaries;
    }

    protected override IDisposable Run(
      IObserver<IList<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._bufferClosingSelector != null)
      {
        Buffer<TSource, TBufferClosing>._ obj = new Buffer<TSource, TBufferClosing>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return obj.Run();
      }
      Buffer<TSource, TBufferClosing>.β β = new Buffer<TSource, TBufferClosing>.β(this, observer, cancel);
      setSink((IDisposable) β);
      return β.Run();
    }

    private class _ : Sink<IList<TSource>>, IObserver<TSource>
    {
      private readonly Buffer<TSource, TBufferClosing> _parent;
      private IList<TSource> _buffer;
      private object _gate;
      private AsyncLock _bufferGate;
      private SerialDisposable _m;

      public _(
        Buffer<TSource, TBufferClosing> parent,
        IObserver<IList<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._buffer = (IList<TSource>) new List<TSource>();
        this._gate = new object();
        this._bufferGate = new AsyncLock();
        this._m = new SerialDisposable();
        CompositeDisposable compositeDisposable = new CompositeDisposable(2)
        {
          (IDisposable) this._m
        };
        compositeDisposable.Add(this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
        this._bufferGate.Wait(new Action(this.CreateBufferClose));
        return (IDisposable) compositeDisposable;
      }

      private void CreateBufferClose()
      {
        IObservable<TBufferClosing> source;
        try
        {
          source = this._parent._bufferClosingSelector();
        }
        catch (Exception ex)
        {
          lock (this._gate)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
        }
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._m.Disposable = (IDisposable) self;
        self.Disposable = source.SubscribeSafe<TBufferClosing>((IObserver<TBufferClosing>) new Buffer<TSource, TBufferClosing>._.ω(this, (IDisposable) self));
      }

      private void CloseBuffer(IDisposable closingSubscription)
      {
        closingSubscription.Dispose();
        lock (this._gate)
        {
          IList<TSource> buffer = this._buffer;
          this._buffer = (IList<TSource>) new List<TSource>();
          this._observer.OnNext(buffer);
        }
        this._bufferGate.Wait(new Action(this.CreateBufferClose));
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
          this._buffer.Add(value);
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._buffer.Clear();
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._observer.OnNext(this._buffer);
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      private class ω : IObserver<TBufferClosing>
      {
        private readonly Buffer<TSource, TBufferClosing>._ _parent;
        private readonly IDisposable _self;

        public ω(Buffer<TSource, TBufferClosing>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public void OnNext(TBufferClosing value) => this._parent.CloseBuffer(this._self);

        public void OnError(Exception error) => this._parent.OnError(error);

        public void OnCompleted() => this._parent.CloseBuffer(this._self);
      }
    }

    private class β : Sink<IList<TSource>>, IObserver<TSource>
    {
      private readonly Buffer<TSource, TBufferClosing> _parent;
      private IList<TSource> _buffer;
      private object _gate;
      private RefCountDisposable _refCountDisposable;

      public β(
        Buffer<TSource, TBufferClosing> parent,
        IObserver<IList<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._buffer = (IList<TSource>) new List<TSource>();
        this._gate = new object();
        CompositeDisposable compositeDisposable = new CompositeDisposable(2);
        this._refCountDisposable = new RefCountDisposable((IDisposable) compositeDisposable);
        compositeDisposable.Add(this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
        compositeDisposable.Add(this._parent._bufferBoundaries.SubscribeSafe<TBufferClosing>((IObserver<TBufferClosing>) new Buffer<TSource, TBufferClosing>.β.ω(this)));
        return (IDisposable) this._refCountDisposable;
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
          this._buffer.Add(value);
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._buffer.Clear();
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._observer.OnNext(this._buffer);
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      private class ω : IObserver<TBufferClosing>
      {
        private readonly Buffer<TSource, TBufferClosing>.β _parent;

        public ω(Buffer<TSource, TBufferClosing>.β parent) => this._parent = parent;

        public void OnNext(TBufferClosing value)
        {
          lock (this._parent._gate)
          {
            IList<TSource> buffer = this._parent._buffer;
            this._parent._buffer = (IList<TSource>) new List<TSource>();
            this._parent._observer.OnNext(buffer);
          }
        }

        public void OnError(Exception error) => this._parent.OnError(error);

        public void OnCompleted() => this._parent.OnCompleted();
      }
    }
  }
}
