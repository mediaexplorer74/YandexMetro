// Decompiled with JetBrains decompiler
// Type: System.Lazy`1
// Assembly: System.Reactive.Core, Version=2.0.20823.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 4A141561-41F7-4AAC-B01C-666EC3F83778
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Reactive.Core.dll

using System.Diagnostics;
using System.Reactive;
using System.Threading;

namespace System
{
  internal class Lazy<T>
  {
    private static Func<T> ALREADY_INVOKED_SENTINEL = (Func<T>) (() => default (T));
    private object m_boxed;
    private Func<T> m_valueFactory;
    private volatile object m_threadSafeObj;

    public Lazy(Func<T> valueFactory)
    {
      this.m_threadSafeObj = new object();
      this.m_valueFactory = valueFactory;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public T Value
    {
      get
      {
        if (this.m_boxed != null)
        {
          if (this.m_boxed is Lazy<T>.Boxed boxed)
            return boxed.m_value;
          (this.m_boxed as Exception).Throw();
        }
        return this.LazyInitValue();
      }
    }

    private T LazyInitValue()
    {
      boxed = (Lazy<T>.Boxed) null;
      object threadSafeObj = this.m_threadSafeObj;
      bool flag = false;
      try
      {
        if (threadSafeObj != Lazy<T>.ALREADY_INVOKED_SENTINEL)
        {
          Monitor.Enter(threadSafeObj);
          flag = true;
        }
        if (this.m_boxed == null)
        {
          boxed = this.CreateValue();
          this.m_boxed = (object) boxed;
          this.m_threadSafeObj = (object) Lazy<T>.ALREADY_INVOKED_SENTINEL;
        }
        else if (!(this.m_boxed is Lazy<T>.Boxed boxed))
          (this.m_boxed as Exception).Throw();
      }
      finally
      {
        if (flag)
          Monitor.Exit(threadSafeObj);
      }
      return boxed.m_value;
    }

    private Lazy<T>.Boxed CreateValue()
    {
      try
      {
        Func<T> func = !(this.m_valueFactory == Lazy<T>.ALREADY_INVOKED_SENTINEL) ? this.m_valueFactory : throw new InvalidOperationException();
        this.m_valueFactory = Lazy<T>.ALREADY_INVOKED_SENTINEL;
        return new Lazy<T>.Boxed(func());
      }
      catch (Exception ex)
      {
        this.m_boxed = (object) ex;
        throw;
      }
    }

    private class Boxed
    {
      internal T m_value;

      internal Boxed(T value) => this.m_value = value;
    }
  }
}
