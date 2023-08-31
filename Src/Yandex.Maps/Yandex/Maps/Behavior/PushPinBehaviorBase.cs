// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Behavior.PushPinBehaviorBase
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using Yandex.Maps.Events;

namespace Yandex.Maps.Behavior
{
  [ComVisible(false)]
  public abstract class PushPinBehaviorBase : System.Windows.Interactivity.Behavior<MapBase>
  {
    private static readonly DependencyProperty _GroupKeyProperty = DependencyProperty.Register(nameof (GroupKey), typeof (string), typeof (PushPinBehaviorBase), (PropertyMetadata) null);
    private string _groupKey;

    public string GroupKey
    {
      get => (string) this.GetValue(PushPinBehaviorBase._GroupKeyProperty);
      set
      {
        if (this.GroupKey != null)
          throw new InvalidOperationException();
        this.SetValue(PushPinBehaviorBase._GroupKeyProperty, (object) value);
      }
    }

    protected override void OnAttached()
    {
      if ((this._groupKey = this.GroupKey) == null)
        throw new Exception("Undefined GroupKey");
      base.OnAttached();
      this.AssociatedObject.PushPinManager.PushPinAddedToGroup += new EventHandler<GroupKeyArgs>(this.PushPinManagerPushPinAddedToGroup);
      this.AssociatedObject.PushPinManager.PushPinRemovedFromGroup += new EventHandler<GroupKeyArgs>(this.PushPinManagerPushPinRemovedFromGroup);
    }

    protected override void OnDetaching()
    {
      this.AssociatedObject.PushPinManager.PushPinAddedToGroup -= new EventHandler<GroupKeyArgs>(this.PushPinManagerPushPinAddedToGroup);
      this.AssociatedObject.PushPinManager.PushPinRemovedFromGroup -= new EventHandler<GroupKeyArgs>(this.PushPinManagerPushPinRemovedFromGroup);
      base.OnDetaching();
    }

    private void PushPinManagerPushPinAddedToGroup(object sender, GroupKeyArgs e)
    {
      if (!(e.GroupKey == this._groupKey))
        return;
      this.OnPushPinManagerPushPinAdded((PushPin) sender);
    }

    private void PushPinManagerPushPinRemovedFromGroup(object sender, GroupKeyArgs e)
    {
      if (!(e.GroupKey == this._groupKey))
        return;
      this.OnPushPinManagerPushPinRemoved((PushPin) sender);
    }

    protected virtual void OnPushPinManagerPushPinAdded(PushPin pushpin)
    {
    }

    protected virtual void OnPushPinManagerPushPinRemoved(PushPin pushpin)
    {
    }

    protected IList<PushPin> Group
    {
      get
      {
        List<PushPin> group;
        if (!this.AssociatedObject.PushPinManager.Groups.TryGetValue(this._groupKey, out group))
          group = new List<PushPin>();
        return (IList<PushPin>) group;
      }
    }
  }
}
