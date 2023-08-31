// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.TriggerActionCollection
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

namespace System.Windows.Interactivity
{
  public class TriggerActionCollection : AttachableCollection<TriggerAction>
  {
    internal TriggerActionCollection()
    {
    }

    protected override void OnAttached()
    {
      foreach (TriggerAction triggerAction in (DependencyObjectCollection<TriggerAction>) this)
        triggerAction.Attach(this.AssociatedObject);
    }

    protected override void OnDetaching()
    {
      foreach (TriggerAction triggerAction in (DependencyObjectCollection<TriggerAction>) this)
        triggerAction.Detach();
    }

    internal override void ItemAdded(TriggerAction item)
    {
      if (item.IsHosted)
        throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
      if (this.AssociatedObject != null)
        item.Attach(this.AssociatedObject);
      item.IsHosted = true;
    }

    internal override void ItemRemoved(TriggerAction item)
    {
      if (((IAttachedObject) item).AssociatedObject != null)
        item.Detach();
      item.IsHosted = false;
    }
  }
}
