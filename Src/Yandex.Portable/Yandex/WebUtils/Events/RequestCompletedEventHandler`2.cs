// Decompiled with JetBrains decompiler
// Type: Yandex.WebUtils.Events.RequestCompletedEventHandler`2
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

namespace Yandex.WebUtils.Events
{
  public delegate void RequestCompletedEventHandler<TParameters, TResult>(
    object sender,
    RequestCompletedEventArgs<TParameters, TResult> e)
    where TResult : class;
}
