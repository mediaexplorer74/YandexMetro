// Decompiled with JetBrains decompiler
// Type: Yandex.Media.Imaging.DispatcheredPool`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Threading;
using Yandex.Media.Imaging.Interfaces;
using Yandex.Threading.Interfaces;

namespace Yandex.Media.Imaging
{
  internal abstract class DispatcheredPool<T> : IPool<T> where T : class
  {
    private const int PoolSize = 4;
    private readonly object _storageLock = new object();
    private readonly Stack<T> _storage;
    private readonly IUiDispatcher _uiDispatcher;
    private bool _cleared;

    protected DispatcheredPool(IUiDispatcher uiDispatcher)
    {
      this._uiDispatcher = uiDispatcher != null ? uiDispatcher : throw new ArgumentNullException(nameof (uiDispatcher));
      this._storage = new Stack<T>();
    }

    public T Pop()
    {
      lock (this._storageLock)
      {
        if (this._storage.Count > 0 && !this._cleared)
          return this._storage.Pop();
        if (this._uiDispatcher.CheckAccess())
        {
          this.CreateObjects();
          return this._storage.Pop();
        }
        while (this._storage.Count == 0 || this._cleared)
        {
          this._uiDispatcher.BeginInvoke(new Action(this.CreateObjects));
          Monitor.Wait(this._storageLock);
        }
        return this._storage.Pop();
      }
    }

    private void CreateObjects()
    {
      for (int index = 0; index < 4; ++index)
      {
        lock (this._storageLock)
        {
          this._storage.Push(this.CreateObject());
          this._cleared = false;
          Monitor.Pulse(this._storageLock);
        }
      }
    }

    public void Push(T item)
    {
      if (this._storage.Count > 4)
        return;
      lock (this._storageLock)
      {
        this._storage.Push(item);
        Monitor.Pulse(this._storageLock);
      }
    }

    protected void Clear()
    {
      lock (this._storageLock)
      {
        this._cleared = true;
        this._storage.Clear();
      }
    }

    protected abstract T CreateObject();
  }
}
