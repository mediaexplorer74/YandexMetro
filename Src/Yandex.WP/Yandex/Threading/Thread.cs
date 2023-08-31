// Decompiled with JetBrains decompiler
// Type: Yandex.Threading.Thread
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using JetBrains.Annotations;
using System;
using System.Threading;
using Yandex.Common;
using Yandex.Threading.Interfaces;

namespace Yandex.Threading
{
  public class Thread : IThread
  {
    public void Start([NotNull] Action threadStart, bool isBackground = true)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Thread.\u003C\u003Ec__DisplayClass2 cDisplayClass2 = new Thread.\u003C\u003Ec__DisplayClass2();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass2.threadStart = threadStart;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass2.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass2.threadStart == null)
        throw new ArgumentNullException(nameof (threadStart));
      // ISSUE: method pointer
      new System.Threading.Thread(new System.Threading.ThreadStart((object) cDisplayClass2, __methodptr(\u003CStart\u003Eb__1)))
      {
        IsBackground = isBackground
      }.Start();
    }

    private void ThreadStart(Action threadStartAction)
    {
      try
      {
        threadStartAction();
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (Exception ex)
      {
        Logger.TrackException(ex);
      }
    }

    public void Sleep(int millisecondsTimeout) => System.Threading.Thread.Sleep(millisecondsTimeout);

    public void Sleep(TimeSpan timeout) => System.Threading.Thread.Sleep(timeout);
  }
}
