// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.LockablePivot
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using Microsoft.Phone.Controls.Primitives;
using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Phone.Controls
{
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (PivotItem))]
  [TemplatePart(Name = "HeadersListElement", Type = typeof (PivotHeadersControl))]
  [TemplatePart(Name = "PivotItemPresenter", Type = typeof (ItemsPresenter))]
  public class LockablePivot : Pivot
  {
    private const string HeadersListElement = "HeadersListElement";
    private const string PivotItemPresenterElement = "PivotItemPresenter";
    internal new const string ItemContainerStyleName = "ItemContainerStyle";
    private const double _animOffset = 20.0;
    private bool _isLocked;
    private bool _isUpdating;
    public static readonly DependencyProperty IsLockedProperty = DependencyProperty.Register(nameof (IsLocked), typeof (bool), typeof (LockablePivot), new PropertyMetadata((object) false, new PropertyChangedCallback(LockablePivot.OnIsLockedPropertyChanged)));
    private PivotItem[] _savedItems;
    private LockablePivot.HeaderAnimationInfo[] _animInfo;
    private PivotHeadersControl _header;
    private int _savedIndex;
    private static Duration _animTime = new Duration(TimeSpan.FromMilliseconds(200.0));

    public bool IsLocked
    {
      get => (bool) ((DependencyObject) this).GetValue(LockablePivot.IsLockedProperty);
      set => ((DependencyObject) this).SetValue(LockablePivot.IsLockedProperty, (object) value);
    }

    private static void OnIsLockedPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is LockablePivot lockablePivot))
        return;
      lockablePivot.OnIsLockedChanged((bool) e.NewValue);
    }

    public LockablePivot() => this.SelectionChanged += new SelectionChangedEventHandler(this.OnSelectionChanged);

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      if (this._isUpdating)
        return;
      if (this._isLocked)
        throw new InvalidOperationException("Pivot Items cannot be modified when locked");
      this._animInfo = (LockablePivot.HeaderAnimationInfo[]) null;
      base.OnItemsChanged(e);
    }

    private PivotHeadersControl FindHeader(UIElement start)
    {
      UIElement header = (UIElement) null;
      int childrenCount = VisualTreeHelper.GetChildrenCount((DependencyObject) start);
      for (int index = 0; index < childrenCount; ++index)
      {
        UIElement child = VisualTreeHelper.GetChild((DependencyObject) start, index) as UIElement;
        header = !(child is PivotHeadersControl) ? (UIElement) this.FindHeader(child) : child;
        if (header != null)
          break;
      }
      return header as PivotHeadersControl;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) => this._animInfo = (LockablePivot.HeaderAnimationInfo[]) null;

    private void OnIsLockedChanged(bool newValue)
    {
      this._isLocked = newValue;
      this._isUpdating = true;
      if (this._isLocked)
      {
        this._savedIndex = this.SelectedIndex;
        this.FadeOutHeaders();
        this.SaveAndRemoveItems();
      }
      else
      {
        this.RestoreItems();
        this.FadeInHeaders();
      }
      this._isUpdating = false;
    }

    private void CreateAnimationInformation()
    {
      if (this._animInfo != null)
        return;
      this._animInfo = new LockablePivot.HeaderAnimationInfo[((PresentationFrameworkCollection<object>) this._header.Items).Count - 1];
      int index = 0;
      foreach (PivotHeaderItem pivotHeaderItem in (PresentationFrameworkCollection<object>) this._header.Items)
      {
        if (!pivotHeaderItem.IsSelected)
        {
          this._animInfo[index].opacity = ((UIElement) pivotHeaderItem).Opacity;
          this._animInfo[index].opacityAnimator = new OpacityAnimator((UIElement) pivotHeaderItem);
          this._animInfo[index].transform = TransformAnimator.GetTranslateTransform((UIElement) pivotHeaderItem);
          this._animInfo[index].transformAnimator = new TransformAnimator(this._animInfo[index].transform);
          this._animInfo[index].originalX = this._animInfo[index].transform.X;
          ++index;
        }
      }
    }

    private void SaveAndRemoveItems()
    {
      this._savedItems = new PivotItem[((PresentationFrameworkCollection<object>) this.Items).Count];
      ((PresentationFrameworkCollection<object>) this.Items).CopyTo((object[]) this._savedItems, 0);
      for (int index = ((PresentationFrameworkCollection<object>) this.Items).Count - 1; index > this._savedIndex; --index)
        ((PresentationFrameworkCollection<object>) this.Items).RemoveAt(index);
      for (int index = 0; index < this._savedIndex; ++index)
        ((PresentationFrameworkCollection<object>) this.Items).RemoveAt(0);
    }

    private void RestoreItems()
    {
      for (int index = 0; index < this._savedIndex; ++index)
        ((PresentationFrameworkCollection<object>) this.Items).Insert(index, (object) this._savedItems[index]);
      for (int index = this._savedIndex + 1; index < this._savedItems.Length; ++index)
        ((PresentationFrameworkCollection<object>) this.Items).Add((object) this._savedItems[index]);
      this._savedItems = (PivotItem[]) null;
    }

    private void FadeOutHeaders()
    {
      if (this._header == null)
        this._header = this.FindHeader((UIElement) this);
      this.CreateAnimationInformation();
      foreach (LockablePivot.HeaderAnimationInfo headerAnimationInfo in this._animInfo)
      {
        headerAnimationInfo.opacityAnimator.GoTo(0.0, LockablePivot._animTime);
        headerAnimationInfo.transformAnimator.GoTo(20.0 + headerAnimationInfo.originalX, LockablePivot._animTime);
      }
    }

    private void FadeInHeaders()
    {
      foreach (LockablePivot.HeaderAnimationInfo headerAnimationInfo in this._animInfo)
      {
        headerAnimationInfo.opacityAnimator.GoTo(headerAnimationInfo.opacity, LockablePivot._animTime);
        headerAnimationInfo.transformAnimator.GoTo(headerAnimationInfo.originalX, LockablePivot._animTime);
      }
    }

    private struct HeaderAnimationInfo
    {
      public double opacity;
      public double originalX;
      public OpacityAnimator opacityAnimator;
      public TransformAnimator transformAnimator;
      public TranslateTransform transform;
    }
  }
}
