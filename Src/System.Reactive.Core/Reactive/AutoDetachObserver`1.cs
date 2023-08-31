// Decompiled with JetBrains decompiler
// Type: System.Reactive.AutoDetachObserver`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Disposables;

namespace System.Reactive
{
  internal class AutoDetachObserver<T> : ObserverBase<T>
  {
    private readonly IObserver<T> observer;
    private readonly SingleAssignmentDisposable m = new SingleAssignmentDisposable();

    public AutoDetachObserver(IObserver<T> observer) => this.observer = observer;

    public IDisposable Disposable
    {
      set => this.m.Disposable = value;
    }

    protected override void OnNextCore(T value)
    {
      bool flag = false;
      try
      {
        this.observer.OnNext(value);
        flag = true;
      }
      finally
      {
        if (!flag)
          this.Dispose();
      }
    }

    protected override void OnErrorCore(Exception exception)
    {
      try
      {
        this.observer.OnError(exception);
      }
      finally
      {
        this.Dispose();
      }
    }

    protected override void OnCompletedCore()
    {
      try
      {
        this.observer.OnCompleted();
      }
      finally
      {
        this.Dispose();
      }
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      this.m.Dispose();
    }
  }
}
