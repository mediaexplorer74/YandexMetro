// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.Primitives.PanoramaPanel
// Assembly: Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e
// MVID: 3A564E2B-07E7-4B61-AB07-0C8262D2893D
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Phone.Controls.Primitives
{
  public class PanoramaPanel : Panel
  {
    private const int SnapThresholdDivisor = 3;
    private Panorama _owner;
    private readonly List<PanoramaItem> _visibleChildren = new List<PanoramaItem>();
    private readonly List<PanoramaPanel.ItemStop> _itemStops = new List<PanoramaPanel.ItemStop>();
    private PanoramaItem _selectedItem;

    internal IList<PanoramaItem> VisibleChildren => (IList<PanoramaItem>) this._visibleChildren;

    private Panorama Owner
    {
      get => this._owner;
      set
      {
        if (this._owner == value)
          return;
        if (this._owner != null)
          this._owner.Panel = (PanoramaPanel) null;
        this._owner = value;
        if (this._owner == null)
          return;
        this._owner.Panel = this;
      }
    }

    public PanoramaPanel()
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanoramaPanel);
      ((FrameworkElement) this).SizeChanged += new SizeChangedEventHandler(this.PanoramaPanel_SizeChanged);
      ((FrameworkElement) this).Loaded += new RoutedEventHandler(this.PanoramaPanel_Loaded);
      ((FrameworkElement) this).Unloaded += new RoutedEventHandler(this.PanoramaPanel_UnLoaded);
    }

    private void PanoramaPanel_Loaded(object sender, RoutedEventArgs e)
    {
      ((FrameworkElement) this).Loaded -= new RoutedEventHandler(this.PanoramaPanel_Loaded);
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_STARTUP, PerfLog.PanoramaPanel);
    }

    private void PanoramaPanel_UnLoaded(object sender, RoutedEventArgs e) => this._owner = (Panorama) null;

    private void PanoramaPanel_SizeChanged(object sender, SizeChangedEventArgs e) => this.Owner.ItemsWidth = (int) e.NewSize.Width;

    protected virtual Size MeasureOverride(Size availableSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanoramaPanel);
      if (this._owner == null)
        this.FindOwner();
      int defaultItemIndex = this.GetDefaultItemIndex();
      Size size1 = new Size(0.0, availableSize.Height);
      int adjustedViewportWidth = this.Owner.AdjustedViewportWidth;
      int num = (int) Math.Min(availableSize.Height, (double) this.Owner.ViewportHeight);
      Size size2 = new Size((double) adjustedViewportWidth, (double) num);
      Size size3 = new Size(double.PositiveInfinity, (double) num);
      int count = ((PresentationFrameworkCollection<UIElement>) this.Children).Count;
      this._visibleChildren.Clear();
      for (int index = 0; index < count; ++index)
      {
        PanoramaItem child = (PanoramaItem) ((PresentationFrameworkCollection<UIElement>) this.Children)[(index + defaultItemIndex) % count];
        if (((UIElement) child).Visibility == null)
        {
          this._visibleChildren.Add(child);
          ((UIElement) child).Measure(child.Orientation == null ? size2 : size3);
          if (child.Orientation == null)
            size1.Width += (double) adjustedViewportWidth;
          else
            size1.Width += Math.Max((double) adjustedViewportWidth, ((UIElement) child).DesiredSize.Width);
        }
      }
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_MEASURE, PerfLog.PanoramaPanel);
      return size1;
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      PerfLog.BeginLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanoramaPanel);
      this._itemStops.Clear();
      double num = 0.0;
      Rect rect = new Rect(0.0, 0.0, 0.0, finalSize.Height);
      for (int index = 0; index < this._visibleChildren.Count; ++index)
      {
        PanoramaItem visibleChild = this._visibleChildren[index];
        rect.X = (double) (visibleChild.StartPosition = (int) num);
        this._itemStops.Add(new PanoramaPanel.ItemStop(visibleChild, index, visibleChild.StartPosition));
        if (visibleChild.Orientation == null)
        {
          rect.Width = (double) this.Owner.AdjustedViewportWidth;
        }
        else
        {
          rect.Width = Math.Max((double) this.Owner.AdjustedViewportWidth, ((UIElement) visibleChild).DesiredSize.Width);
          if (rect.Width > (double) this.Owner.AdjustedViewportWidth)
            this._itemStops.Add(new PanoramaPanel.ItemStop(visibleChild, index, visibleChild.StartPosition + (int) rect.Width - this.Owner.AdjustedViewportWidth));
        }
        visibleChild.ItemWidth = (int) rect.Width;
        ((UIElement) visibleChild).Arrange(rect);
        num += rect.Width;
      }
      this.Owner.RequestAdjustSelection();
      PerfLog.EndLogMarker(PerfMarkerEvents.MPC_PANO_ARRANGE, PerfLog.PanoramaPanel);
      return finalSize;
    }

    private int GetDefaultItemIndex()
    {
      PanoramaItem defaultItemContainer = this.Owner.GetDefaultItemContainer();
      int defaultItemIndex = defaultItemContainer != null ? ((PresentationFrameworkCollection<UIElement>) this.Children).IndexOf((UIElement) defaultItemContainer) : 0;
      if (defaultItemIndex < 0)
        defaultItemIndex = 0;
      return defaultItemIndex;
    }

    private void GetItemsInView(
      int offset,
      int viewportWidth,
      out int leftIndex,
      out int leftInView,
      out int centerIndex,
      out int rightIndex,
      out int rightInView)
    {
      leftIndex = leftInView = centerIndex = rightIndex = rightInView = -1;
      int count = this.VisibleChildren.Count;
      if (count == 0)
        return;
      for (int index = 0; index < count; ++index)
      {
        PanoramaItem visibleChild = this._visibleChildren[index];
        int num1 = visibleChild.StartPosition + offset;
        int num2 = num1 + visibleChild.ItemWidth - 1;
        if (num1 <= 0 && num2 >= 0)
        {
          leftIndex = index;
          leftInView = Math.Min(viewportWidth, visibleChild.ItemWidth + num1);
        }
        if (num1 < viewportWidth && num2 >= viewportWidth)
        {
          rightIndex = index;
          rightInView = Math.Min(viewportWidth, viewportWidth - num1);
        }
        if (num1 > 0 && num2 < viewportWidth)
          centerIndex = index;
        if (index == 0 && leftInView == -1)
          leftInView = num1;
        if (index == count - 1 && rightInView == -1)
          rightInView = viewportWidth - num2 - 1;
      }
    }

    internal void GetStops(
      int offset,
      int totalWidth,
      out PanoramaPanel.ItemStop previous,
      out PanoramaPanel.ItemStop current,
      out PanoramaPanel.ItemStop next)
    {
      int num1;
      int index1 = num1 = -1;
      int index2 = num1;
      int index3 = num1;
      next = current = previous = (PanoramaPanel.ItemStop) null;
      if (this.VisibleChildren.Count == 0)
        return;
      int num2 = -offset % totalWidth;
      int num3 = 0;
      foreach (PanoramaPanel.ItemStop itemStop in this._itemStops)
      {
        if (itemStop.Position < num2)
        {
          index3 = num3;
        }
        else
        {
          if (itemStop.Position > num2)
          {
            index1 = num3;
            break;
          }
          if (itemStop.Position == num2)
            index2 = num3;
        }
        ++num3;
      }
      if (index3 == -1)
        index3 = this._itemStops.Count - 1;
      if (index1 == -1)
        index1 = 0;
      previous = this._itemStops[index3];
      current = index2 != -1 ? this._itemStops[index2] : (PanoramaPanel.ItemStop) null;
      next = this._itemStops[index1];
    }

    internal void GetSnapOffset(
      int offset,
      int viewportWidth,
      int direction,
      out int snapTo,
      out int newDirection,
      out PanoramaItem newSelection,
      out bool wraparound)
    {
      int num = viewportWidth / 3;
      wraparound = false;
      snapTo = offset;
      newDirection = direction;
      newSelection = this._selectedItem;
      if (this.VisibleChildren.Count == 0)
        return;
      foreach (PanoramaPanel.ItemStop itemStop in this._itemStops)
      {
        if (itemStop.Position == -offset)
        {
          newSelection = itemStop.Item;
          return;
        }
      }
      int leftIndex;
      int leftInView;
      int centerIndex;
      int rightIndex;
      int rightInView;
      this.GetItemsInView(offset, viewportWidth, out leftIndex, out leftInView, out centerIndex, out rightIndex, out rightInView);
      if (leftIndex == rightIndex && leftIndex != -1)
      {
        newSelection = this._selectedItem = this._visibleChildren[leftIndex];
      }
      else
      {
        bool flag1 = false;
        if (leftIndex == -1)
        {
          flag1 = true;
          leftIndex = this._visibleChildren.Count - 1;
        }
        bool flag2 = false;
        if (rightIndex == -1)
        {
          flag2 = true;
          rightIndex = 0;
        }
        int index;
        if (direction < 0)
        {
          if (rightInView > num)
          {
            index = PanoramaPanel.GetBestIndex(centerIndex, rightIndex, leftIndex);
            newDirection = -1;
          }
          else
          {
            index = PanoramaPanel.GetBestIndex(leftIndex, centerIndex, rightIndex);
            newDirection = 1;
          }
        }
        else if (direction > 0)
        {
          if (leftInView > num)
          {
            index = PanoramaPanel.GetBestIndex(leftIndex, centerIndex, rightIndex);
            newDirection = 1;
          }
          else
          {
            index = PanoramaPanel.GetBestIndex(centerIndex, rightIndex, leftIndex);
            newDirection = -1;
          }
        }
        else if (centerIndex != -1)
        {
          index = centerIndex;
          newDirection = -1;
        }
        else if (leftInView > rightInView)
        {
          index = leftIndex;
          newDirection = -1;
        }
        else
        {
          index = rightIndex;
          newDirection = 1;
        }
        this._selectedItem = this._visibleChildren[index];
        snapTo = newDirection >= 0 ? PanoramaPanel.GetRightAlignedOffset(this._selectedItem, viewportWidth) : PanoramaPanel.GetLeftAlignedOffset(this._selectedItem, viewportWidth);
        newSelection = this._selectedItem;
        if ((index != leftIndex || !flag1) && (index != rightIndex || !flag2))
          return;
        wraparound = true;
      }
    }

    private static int GetBestIndex(int n0, int n1, int n2)
    {
      if (n0 >= 0)
        return n0;
      if (n1 >= 0)
        return n1;
      return n2 >= 0 ? n2 : throw new InvalidOperationException("No best index.");
    }

    private static int GetLeftAlignedOffset(PanoramaItem movingTo, int viewportWidth) => -movingTo.StartPosition;

    private static int GetRightAlignedOffset(PanoramaItem movingTo, int viewportWidth) => movingTo.Orientation != null ? -movingTo.ItemWidth + viewportWidth - movingTo.StartPosition - 48 : -movingTo.StartPosition;

    private void FindOwner()
    {
      FrameworkElement frameworkElement = (FrameworkElement) this;
      Panorama panorama;
      do
      {
        frameworkElement = (FrameworkElement) VisualTreeHelper.GetParent((DependencyObject) frameworkElement);
        panorama = frameworkElement as Panorama;
      }
      while (frameworkElement != null && panorama == null);
      this.Owner = panorama;
    }

    internal void NotifyDefaultItemChanged()
    {
      ((UIElement) this).InvalidateMeasure();
      ((UIElement) this).InvalidateArrange();
      ((UIElement) this).UpdateLayout();
    }

    internal void ShowLastItemOnLeft()
    {
      this.ResetItemPositions();
      if (this.VisibleChildren.Count <= 0)
        return;
      ((UIElement) this.VisibleChildren[this.VisibleChildren.Count - 1]).RenderTransform = (Transform) new TranslateTransform()
      {
        X = -((FrameworkElement) this).ActualWidth
      };
    }

    internal void ShowFirstItemOnRight()
    {
      this.ResetItemPositions();
      if (this.VisibleChildren.Count <= 0)
        return;
      ((UIElement) this.VisibleChildren[0]).RenderTransform = (Transform) new TranslateTransform()
      {
        X = ((FrameworkElement) this).ActualWidth
      };
    }

    internal void ResetItemPositions()
    {
      foreach (UIElement visibleChild in (IEnumerable<PanoramaItem>) this.VisibleChildren)
        visibleChild.RenderTransform = (Transform) null;
    }

    internal class ItemStop
    {
      public int Index { get; private set; }

      public int Position { get; private set; }

      public PanoramaItem Item { get; private set; }

      public ItemStop(PanoramaItem item, int index, int position)
      {
        this.Item = item;
        this.Index = index;
        this.Position = position;
      }
    }
  }
}
