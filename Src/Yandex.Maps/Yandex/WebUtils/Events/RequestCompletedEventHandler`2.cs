// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Events.RequestCompletedEventHandler`2
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Runtime.InteropServices;

namespace Yandex.WebUtils.Events
{
  [ComVisible(true)]
  public delegate void RequestCompletedEventHandler<TParameters, TResult>(
    object sender,
    RequestCompletedEventArgs<TParameters, TResult> e)
    where TResult : class;
}
