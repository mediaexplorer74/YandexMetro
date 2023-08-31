// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.Observαble.ZipObserver`1
// Assembly: System.Reactive.Linq, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 1EA51347-D36E-40E3-8643-D4542EFB735C
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Linq.dll

using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
  internal class ZipObserver<T> : IObserver<T>
  {
    private readonly object _gate;
    private readonly IZip _parent;
    private readonly int _index;
    private readonly IDisposable _self;
    private readonly Queue<T> _values;

    public ZipObserver(object gate, IZip parent, int index, IDisposable self)
    {
      this._gate = gate;
      this._parent = parent;
      this._index = index;
      this._self = self;
      this._values = new Queue<T>();
    }

    public Queue<T> Values => this._values;

    public void OnNext(T value)
    {
      lock (this._gate)
      {
        this._values.Enqueue(value);
        this._parent.Next(this._index);
      }
    }

    public void OnError(Exception error)
    {
      this._self.Dispose();
      lock (this._gate)
        this._parent.Fail(error);
    }

    public void OnCompleted()
    {
      this._self.Dispose();
      lock (this._gate)
        this._parent.Done(this._index);
    }
  }
}
