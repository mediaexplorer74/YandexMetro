// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.EventObserver
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

using System.Reflection;

namespace System.Windows.Interactivity
{
  public sealed class EventObserver : IDisposable
  {
    private EventInfo eventInfo;
    private object target;
    private Delegate handler;

    public EventObserver(EventInfo eventInfo, object target, Delegate handler)
    {
      if ((object) eventInfo == null)
        throw new ArgumentNullException(nameof (eventInfo));
      if ((object) handler == null)
        throw new ArgumentNullException(nameof (handler));
      this.eventInfo = eventInfo;
      this.target = target;
      this.handler = handler;
      this.eventInfo.AddEventHandler(this.target, handler);
    }

    public void Dispose() => this.eventInfo.RemoveEventHandler(this.target, this.handler);
  }
}
