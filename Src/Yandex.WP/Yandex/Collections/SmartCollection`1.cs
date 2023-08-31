// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.SmartCollection`1
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Yandex.Collections
{
  public class SmartCollection<T> : ObservableCollection<T>
  {
    private bool _suspendCollectionChangeNotification;

    public SmartCollection() => this._suspendCollectionChangeNotification = false;

    public SmartCollection(IEnumerable<T> collection)
      : base(collection)
    {
      this._suspendCollectionChangeNotification = false;
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (this._suspendCollectionChangeNotification)
        return;
      base.OnCollectionChanged(e);
    }

    public void SuspendCollectionChangeNotification() => this._suspendCollectionChangeNotification = true;

    public void ResumeCollectionChangeNotification() => this._suspendCollectionChangeNotification = false;

    public void AddRange(IEnumerable<T> items)
    {
      this.SuspendCollectionChangeNotification();
      try
      {
        foreach (T obj in items)
        {
          // ISSUE: explicit non-virtual call
          this.InsertItem(__nonvirtual (((Collection<T>) this).Count), obj);
        }
      }
      finally
      {
        this.ResumeCollectionChangeNotification();
        this.NotifyCollectionChanged();
      }
    }

    public void NotifyCollectionChanged() => this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
  }
}
