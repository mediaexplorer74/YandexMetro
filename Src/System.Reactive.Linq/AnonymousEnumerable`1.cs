// Decompiled with JetBrains decompiler
// Type: System.Reactive.AnonymousEnumerable`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Reactive
{
  internal sealed class AnonymousEnumerable<T> : IEnumerable<T>, IEnumerable
  {
    private readonly Func<IEnumerator<T>> getEnumerator;

    public AnonymousEnumerable(Func<IEnumerator<T>> getEnumerator) => this.getEnumerator = getEnumerator;

    public IEnumerator<T> GetEnumerator() => this.getEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
