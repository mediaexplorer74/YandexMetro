// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.BehaviorCollection
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

namespace System.Windows.Interactivity
{
  public sealed class BehaviorCollection : AttachableCollection<Behavior>
  {
    internal BehaviorCollection()
    {
    }

    protected override void OnAttached()
    {
      foreach (Behavior behavior in (DependencyObjectCollection<Behavior>) this)
        behavior.Attach(this.AssociatedObject);
    }

    protected override void OnDetaching()
    {
      foreach (Behavior behavior in (DependencyObjectCollection<Behavior>) this)
        behavior.Detach();
    }

    internal override void ItemAdded(Behavior item)
    {
      if (this.AssociatedObject == null)
        return;
      item.Attach(this.AssociatedObject);
    }

    internal override void ItemRemoved(Behavior item)
    {
      if (((IAttachedObject) item).AssociatedObject == null)
        return;
      item.Detach();
    }
  }
}
