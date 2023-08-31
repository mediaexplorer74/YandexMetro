// Decompiled with JetBrains decompiler
// Type: System.Reactive.SafeObserver`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive
{
  internal class SafeObserver<TSource> : IObserver<TSource>
  {
    private readonly IObserver<TSource> _observer;
    private readonly IDisposable _disposable;

    public static IObserver<TSource> Create(IObserver<TSource> observer, IDisposable disposable) => observer is AnonymousObserver<TSource> anonymousObserver ? anonymousObserver.MakeSafe(disposable) : (IObserver<TSource>) new SafeObserver<TSource>(observer, disposable);

    private SafeObserver(IObserver<TSource> observer, IDisposable disposable)
    {
      this._observer = observer;
      this._disposable = disposable;
    }

    public void OnNext(TSource value)
    {
      bool flag = false;
      try
      {
        this._observer.OnNext(value);
        flag = true;
      }
      finally
      {
        if (!flag)
          this._disposable.Dispose();
      }
    }

    public void OnError(Exception error)
    {
      try
      {
        this._observer.OnError(error);
      }
      finally
      {
        this._disposable.Dispose();
      }
    }

    public void OnCompleted()
    {
      try
      {
        this._observer.OnCompleted();
      }
      finally
      {
        this._disposable.Dispose();
      }
    }
  }
}
