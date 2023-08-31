// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.SingleAssignmentDisposable
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Threading;

namespace System.Reactive.Disposables
{
  public sealed class SingleAssignmentDisposable : ICancelable, IDisposable
  {
    private volatile IDisposable _current;

    public bool IsDisposed => this._current == BooleanDisposable.True;

    public IDisposable Disposable
    {
      get
      {
        IDisposable current = this._current;
        return current == BooleanDisposable.True ? (IDisposable) DefaultDisposable.Instance : current;
      }
      set
      {
        IDisposable disposable = Interlocked.CompareExchange<IDisposable>(ref this._current, value, (IDisposable) null);
        if (disposable == null)
          return;
        if (disposable != BooleanDisposable.True)
          throw new InvalidOperationException(Strings_Core.DISPOSABLE_ALREADY_ASSIGNED);
        value?.Dispose();
      }
    }

    public void Dispose() => Interlocked.Exchange<IDisposable>(ref this._current, (IDisposable) BooleanDisposable.True)?.Dispose();
  }
}
