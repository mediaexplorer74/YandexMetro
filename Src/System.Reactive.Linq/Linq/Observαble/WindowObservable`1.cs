// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.WindowObservable`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
  internal class WindowObservable<TSource> : AddRef<TSource>
  {
    public WindowObservable(IObservable<TSource> source, RefCountDisposable refCount)
      : base(source, refCount)
    {
    }
  }
}
