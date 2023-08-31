// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.QuickItemsBehavior`1
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Yandex.Ioc;
using Yandex.Threading.Interfaces;

namespace Yandex.Controls
{
  internal abstract class QuickItemsBehavior<T> : Behavior<T> where T : DependencyObject
  {
    private readonly IUiDispatcher _uiDispatcher;
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (IEnumerable), typeof (QuickItemsBehavior<T>), new PropertyMetadata((object) null, new PropertyChangedCallback(QuickItemsBehavior<T>.ItemsSourceChanged)));
    public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register(nameof (Visibility), typeof (Visibility), typeof (QuickItemsBehavior<T>), new PropertyMetadata((object) (Visibility) 0, new PropertyChangedCallback(QuickItemsBehavior<T>.VisibilityChanged)));
    private Panel _panel;
    private readonly IDictionary<object, UIElement> _children;

    public IEnumerable ItemsSource
    {
      get => (IEnumerable) this.GetValue(QuickItemsBehavior<T>.ItemsSourceProperty);
      set => this.SetValue(QuickItemsBehavior<T>.ItemsSourceProperty, (object) value);
    }

    private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is QuickItemsBehavior<T> quickItemsBehavior))
        return;
      quickItemsBehavior.OnItemsSourceChanged(e);
    }

    private void OnItemsSourceChanged(DependencyPropertyChangedEventArgs e)
    {
      this.Reset();
      this.UnsubscribeFromChanges(e.OldValue as IEnumerable);
      this.SubscribeToChanges(e.NewValue as IEnumerable);
    }

    private static void VisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is QuickItemsBehavior<T> quickItemsBehavior))
        return;
      quickItemsBehavior.OnVisibilityChanged(e);
    }

    private void OnVisibilityChanged(DependencyPropertyChangedEventArgs e)
    {
      Visibility newValue = (Visibility) e.NewValue;
      lock (this._panel)
      {
        foreach (KeyValuePair<object, UIElement> child in (IEnumerable<KeyValuePair<object, UIElement>>) this._children)
          child.Value.Visibility = newValue;
      }
    }

    public Visibility Visibility
    {
      get => (Visibility) this.GetValue(QuickItemsBehavior<T>.VisibilityProperty);
      set => this.SetValue(QuickItemsBehavior<T>.VisibilityProperty, (object) value);
    }

    public Visibility GetVisibility() => this.Visibility;

    public void SetVisibility(Visibility value) => this.Visibility = value;

    protected QuickItemsBehavior()
      : this(IocSingleton<ControlsIocInitializer>.Resolve<IUiDispatcher>())
    {
    }

    protected QuickItemsBehavior([NotNull] IUiDispatcher uiDispatcher)
    {
      this._uiDispatcher = uiDispatcher != null ? uiDispatcher : throw new ArgumentNullException(nameof (uiDispatcher));
      this._children = (IDictionary<object, UIElement>) new Dictionary<object, UIElement>();
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      this._panel = this._panel ?? (this._panel = this.PrepareItemsPanel());
      this.Reset();
      this.SubscribeToChanges(this.ItemsSource);
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      this.UnsubscribeFromChanges(this.ItemsSource);
    }

    private void Reset()
    {
      Panel panel = this._panel;
      if (panel == null)
        return;
      this.Clear();
      if (this.ItemsSource == null)
        return;
      lock (this._panel)
      {
        foreach (object obj in this.ItemsSource)
          this.AddChildToContainter(panel, obj);
      }
    }

    private void Clear()
    {
      Panel panel = this._panel;
      if (panel == null)
        return;
      lock (this._panel)
      {
        foreach (object obj in this._children.Keys.ToArray<object>())
          this.RemoveChildFromContainter(panel, obj);
      }
    }

    private void SubscribeToChanges(IEnumerable value)
    {
      if (!(value is INotifyCollectionChanged collectionChanged))
        return;
      collectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.ItemsSourceCollectionChanged);
      collectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ItemsSourceCollectionChanged);
    }

    private void UnsubscribeFromChanges(IEnumerable value)
    {
      if (!(value is INotifyCollectionChanged collectionChanged))
        return;
      collectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.ItemsSourceCollectionChanged);
    }

    private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => this._uiDispatcher.BeginInvoke((Action) (() =>
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          this.AddItem(e);
          break;
        case NotifyCollectionChangedAction.Remove:
          this.RemoveItem(e);
          break;
        case NotifyCollectionChangedAction.Replace:
          this.Reset();
          break;
        case NotifyCollectionChangedAction.Reset:
          this.Reset();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }));

    private void RemoveItem(NotifyCollectionChangedEventArgs e)
    {
      Panel panel = this._panel;
      if (panel == null)
        return;
      lock (this._panel)
      {
        foreach (object oldItem in (IEnumerable) e.OldItems)
          this.RemoveChildFromContainter(panel, oldItem);
      }
    }

    private void AddItem(NotifyCollectionChangedEventArgs e)
    {
      Panel panel = this._panel;
      if (panel == null)
        return;
      lock (this._panel)
      {
        foreach (object newItem in (IEnumerable) e.NewItems)
        {
          if (!this._children.ContainsKey(newItem))
            this.AddChildToContainter(panel, newItem);
        }
      }
    }

    private void AddChildToContainter(Panel panel, object item)
    {
      try
      {
        UIElement uiElement = this.OnPrepareContainerForItem(item);
        ((PresentationFrameworkCollection<UIElement>) panel.Children).Add(uiElement);
        this._children[item] = uiElement;
      }
      catch
      {
      }
    }

    protected virtual UIElement OnPrepareContainerForItem(object item) => this.PrepareContainerForItem(item);

    private void RemoveChildFromContainter(Panel panel, object item)
    {
      UIElement child;
      if (!this._children.TryGetValue(item, out child))
        return;
      try
      {
        ((PresentationFrameworkCollection<UIElement>) panel.Children).Remove(child);
        this._children.Remove(item);
        this.OnChildRemoved(child);
      }
      catch
      {
      }
    }

    protected virtual void OnChildRemoved(UIElement child)
    {
    }

    [NotNull]
    protected abstract UIElement PrepareContainerForItem(object item);

    protected abstract Panel PrepareItemsPanel();
  }
}
