// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Control.EmptyContentControl
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Y.UI.Common.Control
{
  [ContentProperty("InnerControl")]
  public class EmptyContentControl : System.Windows.Controls.Control
  {
    private Grid _layoutRoot;
    private TextBlock _txtMessage;
    private bool _firstVisibilityUpdate = true;
    private bool _loaded;
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (EmptyContentControl), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty InnerControlProperty = DependencyProperty.Register(nameof (InnerControl), typeof (FrameworkElement), typeof (EmptyContentControl), new PropertyMetadata((object) null, new PropertyChangedCallback(EmptyContentControl.OnInnerControlChanged)));
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (ICollection), typeof (EmptyContentControl), new PropertyMetadata((object) null, new PropertyChangedCallback(EmptyContentControl.OnItemsSourceChanged)));

    public EmptyContentControl() => this.DefaultStyleKey = (object) typeof (EmptyContentControl);

    public string Message
    {
      get => (string) ((DependencyObject) this).GetValue(EmptyContentControl.MessageProperty);
      set => ((DependencyObject) this).SetValue(EmptyContentControl.MessageProperty, (object) value);
    }

    public FrameworkElement InnerControl
    {
      get => (FrameworkElement) ((DependencyObject) this).GetValue(EmptyContentControl.InnerControlProperty);
      set => ((DependencyObject) this).SetValue(EmptyContentControl.InnerControlProperty, (object) value);
    }

    private static void OnInnerControlChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((EmptyContentControl) d).OnInnerControlChanged(e.NewValue as FrameworkElement, e.OldValue as FrameworkElement);
    }

    public ICollection ItemsSource
    {
      get => (ICollection) ((DependencyObject) this).GetValue(EmptyContentControl.ItemsSourceProperty);
      set => ((DependencyObject) this).SetValue(EmptyContentControl.ItemsSourceProperty, (object) value);
    }

    private static void OnItemsSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((EmptyContentControl) d).OnItemsSourceChanged(e.NewValue as ICollection, e.OldValue as ICollection);
    }

    public virtual void OnApplyTemplate()
    {
      this._layoutRoot = this.GetTemplateChild("LayoutRoot") as Grid;
      this._txtMessage = this.GetTemplateChild("txtMessage") as TextBlock;
      ((FrameworkElement) this).OnApplyTemplate();
      this._loaded = true;
      this.UpdateUI();
    }

    private void UpdateUI()
    {
      if (!this._loaded || this.InnerControl == null || this._layoutRoot == null)
        return;
      if (!((PresentationFrameworkCollection<UIElement>) ((Panel) this._layoutRoot).Children).Contains((UIElement) this.InnerControl))
        ((PresentationFrameworkCollection<UIElement>) ((Panel) this._layoutRoot).Children).Add((UIElement) this.InnerControl);
      this.ConfigureVisibility(this.ItemsSource);
    }

    private void SetItemsControlVisible()
    {
      if (this.InnerControl == null)
        return;
      ((UIElement) this._txtMessage).Visibility = (Visibility) 1;
      if (this._firstVisibilityUpdate)
      {
        ((UIElement) this.InnerControl).Opacity = 1.0;
      }
      else
      {
        ((UIElement) this.InnerControl).Opacity = 1.0;
        ((UIElement) this.InnerControl).Visibility = (Visibility) 0;
      }
    }

    private void SetItemsControlInvisible()
    {
      if (this.InnerControl == null)
        return;
      if (this._firstVisibilityUpdate)
      {
        ((UIElement) this.InnerControl).Opacity = 0.0;
      }
      else
      {
        ((UIElement) this.InnerControl).Opacity = 1.0;
        ((UIElement) this.InnerControl).Visibility = (Visibility) 1;
      }
      ((UIElement) this._txtMessage).Visibility = (Visibility) 0;
    }

    private void OnInnerControlChanged(FrameworkElement newControl, FrameworkElement old) => this.UpdateUI();

    private void OnItemsSourceChanged(ICollection newItemsSource, ICollection old)
    {
      if (old != null)
        this.Unsubscribe(old);
      if (newItemsSource == null)
        return;
      this.Subscribe(newItemsSource);
      this.UpdateUI();
    }

    private void Unsubscribe(ICollection old)
    {
      INotifyCollectionChanged collectionChanged = (INotifyCollectionChanged) old;
      if (collectionChanged == null)
        return;
      collectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.ICollectionChanged);
    }

    private void Subscribe(ICollection newItemsSource)
    {
      INotifyCollectionChanged collectionChanged = (INotifyCollectionChanged) newItemsSource;
      if (collectionChanged == null)
        return;
      collectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ICollectionChanged);
    }

    private void ICollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => this.ConfigureVisibility(sender as ICollection);

    private void ConfigureVisibility(ICollection col)
    {
      if (col == null)
        return;
      if (col.Count > 0)
        this.SetItemsControlVisible();
      else
        this.SetItemsControlInvisible();
      if (!this._firstVisibilityUpdate)
        return;
      this._firstVisibilityUpdate = false;
    }
  }
}
