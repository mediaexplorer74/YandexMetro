// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.FromEventPattern
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reflection;

namespace System.Reactive.Linq.Observαble
{
  internal class FromEventPattern
  {
    public class τ<TDelegate, TEventArgs> : ClassicEventProducer<TDelegate, EventPattern<TEventArgs>>
      where TEventArgs : EventArgs
    {
      private readonly Func<EventHandler<TEventArgs>, TDelegate> _conversion;

      public τ(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        : base(addHandler, removeHandler, scheduler)
      {
      }

      public τ(
        Func<EventHandler<TEventArgs>, TDelegate> conversion,
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler,
        IScheduler scheduler)
        : base(addHandler, removeHandler, scheduler)
      {
        this._conversion = conversion;
      }

      protected override TDelegate GetHandler(Action<EventPattern<TEventArgs>> onNext)
      {
        TDelegate @delegate = default (TDelegate);
        return this._conversion != null ? this._conversion((EventHandler<TEventArgs>) ((sender, eventArgs) => onNext(new EventPattern<TEventArgs>(sender, eventArgs)))) : ReflectionUtils.CreateDelegate<TDelegate>((object) (Action<object, TEventArgs>) ((sender, eventArgs) => onNext(new EventPattern<TEventArgs>(sender, eventArgs))), typeof (Action<object, TEventArgs>).GetMethod("Invoke"));
      }
    }

    public class τ<TDelegate, TSender, TEventArgs> : 
      ClassicEventProducer<TDelegate, EventPattern<TSender, TEventArgs>>
      where TEventArgs : EventArgs
    {
      public τ(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        : base(addHandler, removeHandler, scheduler)
      {
      }

      protected override TDelegate GetHandler(Action<EventPattern<TSender, TEventArgs>> onNext) => ReflectionUtils.CreateDelegate<TDelegate>((object) (Action<TSender, TEventArgs>) ((sender, eventArgs) => onNext(new EventPattern<TSender, TEventArgs>(sender, eventArgs))), typeof (Action<TSender, TEventArgs>).GetMethod("Invoke"));
    }

    public class ρ<TSender, TEventArgs, TResult> : EventProducer<Delegate, TResult>
    {
      private readonly object _target;
      private readonly Type _delegateType;
      private readonly MethodInfo _addMethod;
      private readonly MethodInfo _removeMethod;
      private readonly Func<TSender, TEventArgs, TResult> _getResult;

      public ρ(
        object target,
        Type delegateType,
        MethodInfo addMethod,
        MethodInfo removeMethod,
        Func<TSender, TEventArgs, TResult> getResult,
        bool isWinRT,
        IScheduler scheduler)
        : base(scheduler)
      {
        this._target = target;
        this._delegateType = delegateType;
        this._addMethod = addMethod;
        this._removeMethod = removeMethod;
        this._getResult = getResult;
      }

      protected override Delegate GetHandler(Action<TResult> onNext) => ReflectionUtils.CreateDelegate(this._delegateType, (object) (Action<TSender, TEventArgs>) ((sender, eventArgs) => onNext(this._getResult(sender, eventArgs))), typeof (Action<TSender, TEventArgs>).GetMethod("Invoke"));

      protected override IDisposable AddHandler(Delegate handler)
      {
        Action removeHandler = (Action) null;
        try
        {
          this._addMethod.Invoke(this._target, new object[1]
          {
            (object) handler
          });
          removeHandler = (Action) (() => this._removeMethod.Invoke(this._target, new object[1]
          {
            (object) handler
          }));
        }
        catch (TargetInvocationException ex)
        {
          throw ex.InnerException;
        }
        return Disposable.Create((Action) (() =>
        {
          try
          {
            removeHandler();
          }
          catch (TargetInvocationException ex)
          {
            throw ex.InnerException;
          }
        }));
      }
    }
  }
}
