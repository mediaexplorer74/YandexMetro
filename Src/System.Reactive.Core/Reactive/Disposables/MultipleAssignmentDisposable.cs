// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.MultipleAssignmentDisposable
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive.Disposables
{
  public sealed class MultipleAssignmentDisposable : ICancelable, IDisposable
  {
    private readonly object _gate = new object();
    private IDisposable _current;

    public bool IsDisposed
    {
      get
      {
        lock (this._gate)
          return this._current == BooleanDisposable.True;
      }
    }

    public IDisposable Disposable
    {
      get
      {
        lock (this._gate)
          return this._current == BooleanDisposable.True ? (IDisposable) DefaultDisposable.Instance : this._current;
      }
      set
      {
        bool flag = false;
        lock (this._gate)
        {
          flag = this._current == BooleanDisposable.True;
          if (!flag)
            this._current = value;
        }
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
        if (this._current != BooleanDisposable.True)
        {
          disposable = this._current;
          this._current = (IDisposable) BooleanDisposable.True;
        }
      }
      disposable?.Dispose();
    }
  }
}
