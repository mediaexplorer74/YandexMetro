// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.AttachableCollection`1
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace System.Windows.Interactivity
{
  public abstract class AttachableCollection<T> : DependencyObjectCollection<T>, IAttachedObject where T : DependencyObject, IAttachedObject
  {
    private Collection<T> snapshot;
    private DependencyObject associatedObject;

    protected DependencyObject AssociatedObject => this.associatedObject;

    internal AttachableCollection()
    {
      ((INotifyCollectionChanged) this).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
      this.snapshot = new Collection<T>();
    }

    protected abstract void OnAttached();

    protected abstract void OnDetaching();

    internal abstract void ItemAdded(T item);

    internal abstract void ItemRemoved(T item);

    [Conditional("DEBUG")]
    private void VerifySnapshotIntegrity()
    {
      if (this.Count != this.snapshot.Count)
        return;
      for (int index = 0; index < this.Count; ++index)
      {
        if ((object) this[index] != (object) this.snapshot[index])
          break;
      }
    }

    private void VerifyAdd(T item)
    {
      if (this.snapshot.Contains(item))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.DuplicateItemInCollectionExceptionMessage, new object[2]
        {
          (object) typeof (T).Name,
          (object) ((object) this).GetType().Name
        }));
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          IEnumerator enumerator1 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              T current = (T) enumerator1.Current;
              try
              {
                this.VerifyAdd(current);
                this.ItemAdded(current);
              }
              finally
              {
                this.snapshot.Insert(this.IndexOf(current), current);
              }
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case NotifyCollectionChangedAction.Remove:
          IEnumerator enumerator2 = e.OldItems.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              T current = (T) enumerator2.Current;
              this.ItemRemoved(current);
              this.snapshot.Remove(current);
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
        case NotifyCollectionChangedAction.Replace:
          foreach (T oldItem in (IEnumerable) e.OldItems)
          {
            this.ItemRemoved(oldItem);
            this.snapshot.Remove(oldItem);
          }
          IEnumerator enumerator3 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator3.MoveNext())
            {
              T current = (T) enumerator3.Current;
              try
              {
                this.VerifyAdd(current);
                this.ItemAdded(current);
              }
              finally
              {
                this.snapshot.Insert(this.IndexOf(current), current);
              }
            }
            break;
          }
          finally
          {
            if (enumerator3 is IDisposable disposable)
              disposable.Dispose();
          }
        case NotifyCollectionChangedAction.Reset:
          foreach (T obj in this.snapshot)
            this.ItemRemoved(obj);
          this.snapshot = new Collection<T>();
          using (IEnumerator<T> enumerator4 = this.GetEnumerator())
          {
            while (enumerator4.MoveNext())
            {
              T current = enumerator4.Current;
              this.VerifyAdd(current);
              this.ItemAdded(current);
            }
            break;
          }
      }
    }

    DependencyObject IAttachedObject.AssociatedObject => this.AssociatedObject;

    public void Attach(DependencyObject dependencyObject)
    {
      if (dependencyObject == this.AssociatedObject)
        return;
      if (this.AssociatedObject != null)
        throw new InvalidOperationException();
      if (Application.Current == null || Application.Current.RootVisual == null || !(bool) ((DependencyObject) Application.Current.RootVisual).GetValue(DesignerProperties.IsInDesignModeProperty))
        this.associatedObject = dependencyObject;
      this.OnAttached();
    }

    public void Detach()
    {
      this.OnDetaching();
      this.associatedObject = (DependencyObject) null;
    }
  }
}
