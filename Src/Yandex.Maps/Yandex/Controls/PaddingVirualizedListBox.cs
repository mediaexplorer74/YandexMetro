// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.PaddingVirualizedListBox
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Yandex.Controls
{
  internal class PaddingVirualizedListBox : ListBox
  {
    private const double Epsilon = 0.01;
    public static readonly DependencyProperty ContentBottomPaddingProperty = DependencyProperty.Register(nameof (ContentBottomPadding), typeof (double), typeof (PaddingVirualizedListBox), new PropertyMetadata((object) 0.0));
    private FrameworkElement _lastElement;
    private Thickness _lastElementMargin;

    public double ContentBottomPadding
    {
      get => (double) ((DependencyObject) this).GetValue(PaddingVirualizedListBox.ContentBottomPaddingProperty);
      set => ((DependencyObject) this).SetValue(PaddingVirualizedListBox.ContentBottomPaddingProperty, (object) value);
    }

    protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      ((Selector) this).PrepareContainerForItemOverride(element, item);
      if (Math.Abs(this.ContentBottomPadding) < 0.01 || ((ItemsControl) this).ItemContainerGenerator == null)
        return;
      DependencyObject dependencyObject = ((ItemsControl) this).ItemContainerGenerator.ContainerFromIndex(((PresentationFrameworkCollection<object>) ((ItemsControl) this).Items).Count - 1);
      if (dependencyObject == element)
      {
        if (this._lastElement == dependencyObject)
          return;
        if (this._lastElement != null)
          this._lastElement.Margin = this._lastElementMargin;
        if (!(element is FrameworkElement frameworkElement))
          return;
        this._lastElement = frameworkElement;
        this._lastElementMargin = frameworkElement.Margin;
        this.UpdateLastElementMargin();
      }
      else
      {
        if (dependencyObject == this._lastElement)
          return;
        this._lastElement.Margin = this._lastElementMargin;
        this._lastElement = (FrameworkElement) null;
      }
    }

    private void UpdateLastElementMargin()
    {
      if (this._lastElement == null)
        return;
      this._lastElement.Margin = new Thickness(this._lastElementMargin.Left, this._lastElementMargin.Top, this._lastElementMargin.Right, this._lastElementMargin.Bottom + this.ContentBottomPadding);
    }
  }
}
