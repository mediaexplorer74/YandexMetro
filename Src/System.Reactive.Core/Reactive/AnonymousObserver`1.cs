// Decompiled with JetBrains decompiler
// Type: System.Reactive.AnonymousObserver`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive
{
  public sealed class AnonymousObserver<T> : ObserverBase<T>
  {
    private readonly Action<T> _onNext;
    private readonly Action<Exception> _onError;
    private readonly Action _onCompleted;

    public AnonymousObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
    {
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      this._onNext = onNext;
      this._onError = onError;
      this._onCompleted = onCompleted;
    }

    public AnonymousObserver(Action<T> onNext)
      : this(onNext, Stubs.Throw, Stubs.Nop)
    {
    }

    public AnonymousObserver(Action<T> onNext, Action<Exception> onError)
      : this(onNext, onError, Stubs.Nop)
    {
    }

    public AnonymousObserver(Action<T> onNext, Action onCompleted)
      : this(onNext, Stubs.Throw, onCompleted)
    {
    }

    protected override void OnNextCore(T value) => this._onNext(value);

    protected override void OnErrorCore(Exception error) => this._onError(error);

    protected override void OnCompletedCore() => this._onCompleted();

    internal IObserver<T> MakeSafe(IDisposable disposable) => (IObserver<T>) new AnonymousSafeObserver<T>(this._onNext, this._onError, this._onCompleted, disposable);
  }
}
