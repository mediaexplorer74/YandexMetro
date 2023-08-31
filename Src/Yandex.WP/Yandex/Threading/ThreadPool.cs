// Decompiled with JetBrains decompiler
// Type: Yandex.Threading.ThreadPool
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using Yandex.Threading.Interfaces;

namespace Yandex.Threading
{
  public class ThreadPool : IThreadPool
  {
    public bool QueueUserWorkItem(WaitCallback callBack, object state) => System.Threading.ThreadPool.QueueUserWorkItem((System.Threading.WaitCallback) (o => callBack(o)), state);
  }
}
