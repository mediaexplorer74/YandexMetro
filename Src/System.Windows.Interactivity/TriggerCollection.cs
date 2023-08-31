// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.TriggerCollection
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

namespace System.Windows.Interactivity
{
  public sealed class TriggerCollection : AttachableCollection<TriggerBase>
  {
    internal TriggerCollection()
    {
    }

    protected override void OnAttached()
    {
      foreach (TriggerBase triggerBase in (DependencyObjectCollection<TriggerBase>) this)
        triggerBase.Attach(this.AssociatedObject);
    }

    protected override void OnDetaching()
    {
      foreach (TriggerBase triggerBase in (DependencyObjectCollection<TriggerBase>) this)
        triggerBase.Detach();
    }

    internal override void ItemAdded(TriggerBase item)
    {
      if (this.AssociatedObject == null)
        return;
      item.Attach(this.AssociatedObject);
    }

    internal override void ItemRemoved(TriggerBase item)
    {
      if (((IAttachedObject) item).AssociatedObject == null)
        return;
      item.Detach();
    }
  }
}
