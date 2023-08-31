// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.SerialDisposable
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive.Disposables
{
  public sealed class SerialDisposable : ICancelable, IDisposable
  {
    private readonly object _gate = new object();
    private IDisposable _current;
    private bool _disposed;

    public bool IsDisposed
    {
      get
      {
        lock (this._gate)
          return this._disposed;
      }
    }

    public IDisposable Disposable
    {
      get => this._current;
      set
      {
        bool flag = false;
        IDisposable disposable = (IDisposable) null;
        lock (this._gate)
        {
          flag = this._disposed;
          if (!flag)
          {
            disposable = this._current;
            this._current = value;
          }
        }
        disposable?.Dispose();
        if (!flag || value == null)
          return;
        value.Dispose();
      }
    }

    public void Dispose()
    {
      IDisposable disposable = (IDisposable) null;
      lock (this._gate)
      {
        if (!this._disposed)
        {
          this._disposed = true;
          disposable = this._current;
          this._current = (IDisposable) null;
        }
      }
      disposable?.Dispose();
    }
  }
}
