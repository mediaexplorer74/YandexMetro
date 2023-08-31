// Decompiled with JetBrains decompiler
// Type: System.Reactive.EventPatternSourceBase`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive
{
  public abstract class EventPatternSourceBase<TSender, TEventArgs> where TEventArgs : EventArgs
  {
    private readonly IObservable<EventPattern<TSender, TEventArgs>> _source;
    private readonly Dictionary<Delegate, Stack<IDisposable>> _subscriptions;
    private readonly Action<Action<TSender, TEventArgs>, EventPattern<TSender, TEventArgs>> _invokeHandler;

    protected EventPatternSourceBase(
      IObservable<EventPattern<TSender, TEventArgs>> source,
      Action<Action<TSender, TEventArgs>, EventPattern<TSender, TEventArgs>> invokeHandler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (invokeHandler == null)
        throw new ArgumentNullException(nameof (invokeHandler));
      this._source = source;
      this._invokeHandler = invokeHandler;
      this._subscriptions = new Dictionary<Delegate, Stack<IDisposable>>();
    }

    protected void Add(Delegate handler, Action<TSender, TEventArgs> invoke)
    {
      if ((object) handler == null)
        throw new ArgumentNullException(nameof (handler));
      if (invoke == null)
        throw new ArgumentNullException(nameof (invoke));
      object gate = new object();
      bool isAdded = false;
      bool isDone = false;
      Action remove = (Action) (() =>
      {
        lock (gate)
        {
          if (isAdded)
            this.Remove(handler);
          else
            isDone = true;
        }
      });
      IDisposable disposable = this._source.Subscribe<EventPattern<TSender, TEventArgs>>((Action<EventPattern<TSender, TEventArgs>>) (x => this._invokeHandler(invoke, x)), (Action<Exception>) (ex =>
      {
        remove();
        ex.Throw();
      }), (Action) (() => remove()));
      lock (gate)
      {
        if (isDone)
          return;
        this.Add(handler, disposable);
        isAdded = true;
      }
    }

    private void Add(Delegate handler, IDisposable disposable)
    {
      lock (this._subscriptions)
      {
        Stack<IDisposable> disposableStack = new Stack<IDisposable>();
        if (!this._subscriptions.TryGetValue(handler, out disposableStack))
          this._subscriptions[handler] = disposableStack = new Stack<IDisposable>();
        disposableStack.Push(disposable);
      }
    }

    protected void Remove(Delegate handler)
    {
      if ((object) handler == null)
        throw new ArgumentNullException(nameof (handler));
      IDisposable disposable = (IDisposable) null;
      lock (this._subscriptions)
      {
        Stack<IDisposable> disposableStack = new Stack<IDisposable>();
        if (this._subscriptions.TryGetValue(handler, out disposableStack))
        {
          disposable = disposableStack.Pop();
          if (disposableStack.Count == 0)
            this._subscriptions.Remove(handler);
        }
      }
      disposable?.Dispose();
    }
  }
}
