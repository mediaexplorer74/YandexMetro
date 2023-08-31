// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.EventTriggerBase`1
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

namespace System.Windows.Interactivity
{
  public abstract class EventTriggerBase<T> : EventTriggerBase where T : class
  {
    protected EventTriggerBase()
      : base(typeof (T))
    {
    }

    public T Source => (T) base.Source;

    internal override sealed void OnSourceChangedImpl(object oldSource, object newSource)
    {
      base.OnSourceChangedImpl(oldSource, newSource);
      this.OnSourceChanged(oldSource as T, newSource as T);
    }

    protected virtual void OnSourceChanged(T oldSource, T newSource)
    {
    }
  }
}
