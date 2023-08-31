// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.IGroupedObservable`2
// Assembly: System.Reactive.Interfaces, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C5CFAE41-B7A6-44A9-B1FA-D115FDEC8DA3
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Interfaces.dll

namespace System.Reactive.Linq
{
  public interface IGroupedObservable<TKey, TElement> : IObservable<TElement>
  {
    TKey Key { get; }
  }
}
