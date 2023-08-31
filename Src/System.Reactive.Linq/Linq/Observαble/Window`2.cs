// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.Window`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
  internal class Window<TSource, TWindowClosing> : Producer<IObservable<TSource>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<IObservable<TWindowClosing>> _windowClosingSelector;
    private readonly IObservable<TWindowClosing> _windowBoundaries;

    public Window(
      IObservable<TSource> source,
      Func<IObservable<TWindowClosing>> windowClosingSelector)
    {
      this._source = source;
      this._windowClosingSelector = windowClosingSelector;
    }

    public Window(IObservable<TSource> source, IObservable<TWindowClosing> windowBoundaries)
    {
      this._source = source;
      this._windowBoundaries = windowBoundaries;
    }

    protected override IDisposable Run(
      IObserver<IObservable<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._windowClosingSelector != null)
      {
        Window<TSource, TWindowClosing>._ obj = new Window<TSource, TWindowClosing>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return obj.Run();
      }
      Window<TSource, TWindowClosing>.β β = new Window<TSource, TWindowClosing>.β(this, observer, cancel);
      setSink((IDisposable) β);
      return β.Run();
    }

    private class _ : Sink<IObservable<TSource>>, IObserver<TSource>
    {
      private readonly Window<TSource, TWindowClosing> _parent;
      private ISubject<TSource> _window;
      private object _gate;
      private AsyncLock _windowGate;
      private SerialDisposable _m;
      private RefCountDisposable _refCountDisposable;

      public _(
        Window<TSource, TWindowClosing> parent,
        IObserver<IObservable<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._window = (ISubject<TSource>) new Subject<TSource>();
        this._gate = new object();
        this._windowGate = new AsyncLock();
        this._m = new SerialDisposable();
        CompositeDisposable compositeDisposable = new CompositeDisposable(2)
        {
          (IDisposable) this._m
        };
        this._refCountDisposable = new RefCountDisposable((IDisposable) compositeDisposable);
        this._observer.OnNext((IObservable<TSource>) new WindowObservable<TSource>((IObservable<TSource>) this._window, this._refCountDisposable));
        compositeDisposable.Add(this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
        this._windowGate.Wait(new Action(this.CreateWindowClose));
        return (IDisposable) this._refCountDisposable;
      }

      private void CreateWindowClose()
      {
        IObservable<TWindowClosing> source;
        try
        {
          source = this._parent._windowClosingSelector();
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
        self.Disposable = source.SubscribeSafe<TWindowClosing>((IObserver<TWindowClosing>) new Window<TSource, TWindowClosing>._.ω(this, (IDisposable) self));
      }

      private void CloseWindow(IDisposable closingSubscription)
      {
        closingSubscription.Dispose();
        lock (this._gate)
        {
          ((IObserver<TSource>) this._window).OnCompleted();
          this._window = (ISubject<TSource>) new Subject<TSource>();
          this._observer.OnNext((IObservable<TSource>) new WindowObservable<TSource>((IObservable<TSource>) this._window, this._refCountDisposable));
        }
        this._windowGate.Wait(new Action(this.CreateWindowClose));
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
          ((IObserver<TSource>) this._window).OnNext(value);
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          ((IObserver<TSource>) this._window).OnError(error);
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          ((IObserver<TSource>) this._window).OnCompleted();
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      private class ω : IObserver<TWindowClosing>
      {
        private readonly Window<TSource, TWindowClosing>._ _parent;
        private readonly IDisposable _self;

        public ω(Window<TSource, TWindowClosing>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public void OnNext(TWindowClosing value) => this._parent.CloseWindow(this._self);

        public void OnError(Exception error) => this._parent.OnError(error);

        public void OnCompleted() => this._parent.CloseWindow(this._self);
      }
    }

    private class β : Sink<IObservable<TSource>>, IObserver<TSource>
    {
      private readonly Window<TSource, TWindowClosing> _parent;
      private ISubject<TSource> _window;
      private object _gate;
      private RefCountDisposable _refCountDisposable;

      public β(
        Window<TSource, TWindowClosing> parent,
        IObserver<IObservable<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._window = (ISubject<TSource>) new Subject<TSource>();
        this._gate = new object();
        CompositeDisposable compositeDisposable = new CompositeDisposable(2);
        this._refCountDisposable = new RefCountDisposable((IDisposable) compositeDisposable);
        this._observer.OnNext((IObservable<TSource>) new WindowObservable<TSource>((IObservable<TSource>) this._window, this._refCountDisposable));
        compositeDisposable.Add(this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
        compositeDisposable.Add(this._parent._windowBoundaries.SubscribeSafe<TWindowClosing>((IObserver<TWindowClosing>) new Window<TSource, TWindowClosing>.β.ω(this)));
        return (IDisposable) this._refCountDisposable;
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
          ((IObserver<TSource>) this._window).OnNext(value);
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          ((IObserver<TSource>) this._window).OnError(error);
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          ((IObserver<TSource>) this._window).OnCompleted();
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      private class ω : IObserver<TWindowClosing>
      {
        private readonly Window<TSource, TWindowClosing>.β _parent;

        public ω(Window<TSource, TWindowClosing>.β parent) => this._parent = parent;

        public void OnNext(TWindowClosing value)
        {
          lock (this._parent._gate)
          {
            ((IObserver<TSource>) this._parent._window).OnCompleted();
            this._parent._window = (ISubject<TSource>) new Subject<TSource>();
            this._parent._observer.OnNext((IObservable<TSource>) new WindowObservable<TSource>((IObservable<TSource>) this._parent._window, this._parent._refCountDisposable));
          }
        }

        public void OnError(Exception error) => this._parent.OnError(error);

        public void OnCompleted() => this._parent.OnCompleted();
      }
    }
  }
}
