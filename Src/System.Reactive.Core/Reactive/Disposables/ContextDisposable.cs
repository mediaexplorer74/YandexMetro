// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.ContextDisposable
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive.Disposables
{
  public sealed class ContextDisposable : ICancelable, IDisposable
  {
    private readonly SynchronizationContext _context;
    private volatile IDisposable _disposable;

    public ContextDisposable(SynchronizationContext context, IDisposable disposable)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      if (disposable == null)
        throw new ArgumentNullException(nameof (disposable));
      this._context = context;
      this._disposable = disposable;
    }

    public SynchronizationContext Context => this._context;

    public bool IsDisposed => this._disposable == BooleanDisposable.True;

    public void Dispose()
    {
      IDisposable state = Interlocked.Exchange<IDisposable>(ref this._disposable, (IDisposable) BooleanDisposable.True);
      if (state == BooleanDisposable.True)
        return;
      this._context.PostWithStartComplete<IDisposable>((Action<IDisposable>) (d => d.Dispose()), state);
    }
  }
}
