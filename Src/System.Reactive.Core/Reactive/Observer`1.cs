// Decompiled with JetBrains decompiler
// Type: System.Reactive.Observer`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive
{
  internal class Observer<T> : IObserver<T>
  {
    private readonly ImmutableList<IObserver<T>> _observers;

    public Observer(ImmutableList<IObserver<T>> observers) => this._observers = observers;

    public void OnCompleted()
    {
      foreach (IObserver<T> iobserver in this._observers.Data)
        iobserver.OnCompleted();
    }

    public void OnError(Exception error)
    {
      foreach (IObserver<T> iobserver in this._observers.Data)
        iobserver.OnError(error);
    }

    public void OnNext(T value)
    {
      foreach (IObserver<T> iobserver in this._observers.Data)
        iobserver.OnNext(value);
    }

    internal IObserver<T> Add(IObserver<T> observer) => (IObserver<T>) new Observer<T>(this._observers.Add(observer));

    internal IObserver<T> Remove(IObserver<T> observer)
    {
      int num = Array.IndexOf<IObserver<T>>(this._observers.Data, observer);
      if (num < 0)
        return (IObserver<T>) this;
      return this._observers.Data.Length == 2 ? this._observers.Data[1 - num] : (IObserver<T>) new Observer<T>(this._observers.Remove(observer));
    }
  }
}
