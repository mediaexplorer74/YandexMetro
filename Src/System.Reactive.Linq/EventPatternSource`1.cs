// Decompiled with JetBrains decompiler
// Type: System.Reactive.EventPatternSource`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

namespace System.Reactive
{
  internal class EventPatternSource<TEventArgs> : 
    EventPatternSourceBase<object, TEventArgs>,
    IEventPatternSource<TEventArgs>
    where TEventArgs : EventArgs
  {
    public EventPatternSource(
      IObservable<EventPattern<object, TEventArgs>> source,
      Action<Action<object, TEventArgs>, EventPattern<object, TEventArgs>> invokeHandler)
      : base(source, invokeHandler)
    {
    }

    event EventHandler<TEventArgs> IEventPatternSource<TEventArgs>.OnNext
    {
      add => this.Add((Delegate) value, (Action<object, TEventArgs>) ((o, e) => value(o, e)));
      remove => this.Remove((Delegate) value);
    }
  }
}
