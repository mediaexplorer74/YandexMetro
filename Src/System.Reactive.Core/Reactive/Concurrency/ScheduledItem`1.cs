// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ScheduledItem`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
  public abstract class ScheduledItem<TAbsolute> : 
    IScheduledItem<TAbsolute>,
    IComparable<ScheduledItem<TAbsolute>>
    where TAbsolute : IComparable<TAbsolute>
  {
    private readonly SingleAssignmentDisposable _disposable = new SingleAssignmentDisposable();
    private readonly TAbsolute _dueTime;
    private readonly IComparer<TAbsolute> _comparer;

    protected ScheduledItem(TAbsolute dueTime, IComparer<TAbsolute> comparer)
    {
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      this._dueTime = dueTime;
      this._comparer = comparer;
    }

    public TAbsolute DueTime => this._dueTime;

    public void Invoke()
    {
      if (this._disposable.IsDisposed)
        return;
      this._disposable.Disposable = this.InvokeCore();
    }

    protected abstract IDisposable InvokeCore();

    public int CompareTo(ScheduledItem<TAbsolute> other) => object.ReferenceEquals((object) other, (object) null) ? 1 : this._comparer.Compare(this.DueTime, other.DueTime);

    public static bool operator <(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) < 0;

    public static bool operator <=(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) <= 0;

    public static bool operator >(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) > 0;

    public static bool operator >=(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) >= 0;

    public static bool operator ==(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => object.ReferenceEquals((object) left, (object) right);

    public static bool operator !=(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right) => !(left == right);

    public override bool Equals(object obj) => object.ReferenceEquals((object) this, obj);

    public override int GetHashCode() => base.GetHashCode();

    public void Cancel() => this._disposable.Dispose();

    public bool IsCanceled => this._disposable.IsDisposed;
  }
}
