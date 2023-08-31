// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.IQbservableProvider
// Assembly: System.Reactive.Interfaces, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C5CFAE41-B7A6-44A9-B1FA-D115FDEC8DA3
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Interfaces.dll

using System.Linq.Expressions;

namespace System.Reactive.Linq
{
  public interface IQbservableProvider
  {
    IQbservable<TResult> CreateQuery<TResult>(Expression expression);
  }
}
