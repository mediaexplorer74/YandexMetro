// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.GroupedObservable`2
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
  internal class GroupedObservable<TKey, TElement> : 
    ObservableBase<TElement>,
    IGroupedObservable<TKey, TElement>,
    IObservable<TElement>
  {
    private readonly TKey _key;
    private readonly IObservable<TElement> _subject;
    private readonly RefCountDisposable _refCount;

    public GroupedObservable(TKey key, ISubject<TElement> subject, RefCountDisposable refCount)
    {
      this._key = key;
      this._subject = (IObservable<TElement>) subject;
      this._refCount = refCount;
    }

    public GroupedObservable(TKey key, ISubject<TElement> subject)
    {
      this._key = key;
      this._subject = (IObservable<TElement>) subject;
    }

    public TKey Key => this._key;

    protected override IDisposable SubscribeCore(IObserver<TElement> observer)
    {
      if (this._refCount == null)
        return this._subject.Subscribe(observer);
      return (IDisposable) new CompositeDisposable(new IDisposable[2]
      {
        this._refCount.GetDisposable(),
        this._subject.Subscribe(observer)
      });
    }
  }
}
