// Decompiled with JetBrains decompiler
// Type: System.Reactive.AnonymousObservable`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Disposables;

namespace System.Reactive
{
  public sealed class AnonymousObservable<T> : ObservableBase<T>
  {
    private readonly Func<IObserver<T>, IDisposable> _subscribe;

    public AnonymousObservable(Func<IObserver<T>, IDisposable> subscribe) => this._subscribe = subscribe != null ? subscribe : throw new ArgumentNullException(nameof (subscribe));

    protected override IDisposable SubscribeCore(IObserver<T> observer) => this._subscribe(observer) ?? Disposable.Empty;
  }
}
