// Decompiled with JetBrains decompiler
// Type: Yandex.Threading.ThreadPool
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Threading.Interfaces;

namespace Yandex.Threading
{
  internal class ThreadPool : IThreadPool
  {
    public bool QueueUserWorkItem(WaitCallback callBack, object state) => System.Threading.ThreadPool.QueueUserWorkItem((System.Threading.WaitCallback) (o => callBack(o)), state);
  }
}
