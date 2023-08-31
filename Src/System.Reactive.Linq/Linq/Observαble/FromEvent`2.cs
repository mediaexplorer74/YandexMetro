// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.FromEvent`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;

namespace System.Reactive.Linq.Observαble
{
  internal class FromEvent<TDelegate, TEventArgs> : ClassicEventProducer<TDelegate, TEventArgs>
  {
    private readonly Func<Action<TEventArgs>, TDelegate> _conversion;

    public FromEvent(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      : base(addHandler, removeHandler, scheduler)
    {
    }

    public FromEvent(
      Func<Action<TEventArgs>, TDelegate> conversion,
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler)
      : base(addHandler, removeHandler, scheduler)
    {
      this._conversion = conversion;
    }

    protected override TDelegate GetHandler(Action<TEventArgs> onNext)
    {
      TDelegate @delegate = default (TDelegate);
      return this._conversion != null ? this._conversion(onNext) : ReflectionUtils.CreateDelegate<TDelegate>((object) onNext, typeof (Action<TEventArgs>).GetMethod("Invoke"));
    }
  }
}
