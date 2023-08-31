// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.ConnectableObservable`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Linq;

namespace System.Reactive.Subjects
{
  internal class ConnectableObservable<TSource, TResult> : 
    IConnectableObservable<TResult>,
    IObservable<TResult>
  {
    private readonly ISubject<TSource, TResult> _subject;
    private readonly IObservable<TSource> _source;
    private readonly object _gate;
    private ConnectableObservable<TSource, TResult>.Connection _connection;

    public ConnectableObservable(IObservable<TSource> source, ISubject<TSource, TResult> subject)
    {
      this._subject = subject;
      this._source = source.AsObservable<TSource>();
      this._gate = new object();
    }

    public IDisposable Connect()
    {
      lock (this._gate)
      {
        if (this._connection == null)
          this._connection = new ConnectableObservable<TSource, TResult>.Connection(this, this._source.SubscribeSafe<TSource>((IObserver<TSource>) this._subject));
        return (IDisposable) this._connection;
      }
    }

    public IDisposable Subscribe(IObserver<TResult> observer) => observer != null ? ((IObservable<TResult>) this._subject).SubscribeSafe<TResult>(observer) : throw new ArgumentNullException(nameof (observer));

    private class Connection : IDisposable
    {
      private readonly ConnectableObservable<TSource, TResult> _parent;
      private IDisposable _subscription;

      public Connection(ConnectableObservable<TSource, TResult> parent, IDisposable subscription)
      {
        this._parent = parent;
        this._subscription = subscription;
      }

      public void Dispose()
      {
        lock (this._parent._gate)
        {
          if (this._subscription == null)
            return;
          this._subscription.Dispose();
          this._subscription = (IDisposable) null;
          this._parent._connection = (ConnectableObservable<TSource, TResult>.Connection) null;
        }
      }
    }
  }
}
