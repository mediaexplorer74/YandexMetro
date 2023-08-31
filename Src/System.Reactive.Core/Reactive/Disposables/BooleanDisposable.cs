// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.BooleanDisposable
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

namespace System.Reactive.Disposables
{
  public sealed class BooleanDisposable : ICancelable, IDisposable
  {
    internal static readonly BooleanDisposable True = new BooleanDisposable(true);
    private volatile bool _isDisposed;

    public BooleanDisposable()
    {
    }

    private BooleanDisposable(bool isDisposed) => this._isDisposed = isDisposed;

    public bool IsDisposed => this._isDisposed;

    public void Dispose() => this._isDisposed = true;
  }
}
