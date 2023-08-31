// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.AnonymousDisposable
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Threading;

namespace System.Reactive.Disposables
{
  internal sealed class AnonymousDisposable : ICancelable, IDisposable
  {
    private volatile Action _dispose;

    public AnonymousDisposable(Action dispose) => this._dispose = dispose;

    public bool IsDisposed => this._dispose == null;

    public void Dispose()
    {
      Action action = Interlocked.Exchange<Action>(ref this._dispose, (Action) null);
      if (action == null)
        return;
      action();
    }
  }
}
