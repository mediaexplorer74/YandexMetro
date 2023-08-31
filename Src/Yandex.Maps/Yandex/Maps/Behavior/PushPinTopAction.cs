// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Behavior.PushPinTopAction
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Clarity.Phone.Extensions;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Yandex.Maps.Behavior.Interfaces;

namespace Yandex.Maps.Behavior
{
  internal class PushPinTopAction : IPushPinContentVisibilityAction
  {
    private const string MapLayerLocationPropertyName = "MapLayer.Location";
    private const string MapLayerPositionOffsetPropertyName = "MapLayer.PositionOffset";
    private const string MapLayerAlignmentPropertyName = "MapLayer.Alignment";
    private const string BackgroundPropertyName = "Background";
    private const string PaddingPropertyName = "Padding";
    private PushPin _copiedPushPin;
    private PushPin _oldCopiedPushPin;
    private MapBase _map;
    private PushPin _topPushPin;
    private bool _topPushpinVisibilityUpdatePending;

    public void InitializeMap(MapBase map) => this._map = map;

    public void OnPushPinContentBecomeVisible(PushPin pushPin, IList<PushPin> allPushPins)
    {
      if (this._copiedPushPin != null && this._topPushPin != null)
      {
        PushPinTopAction.SwitchContent(this._topPushPin, this._copiedPushPin);
        ((FrameworkElement) this._copiedPushPin).LayoutUpdated -= new EventHandler(this.PushpinLayoutUpdated);
        this._copiedPushPin.IsHidden = false;
        this._copiedPushPin = (PushPin) null;
      }
      this.CopySettingsToTopPushPin(pushPin);
    }

    public void OnPushPinContentBecomeCollapsed(PushPin pushPin, IList<PushPin> allPushPins)
    {
      if (this._copiedPushPin == pushPin)
      {
        PushPinTopAction.SwitchContent(this._topPushPin, this._copiedPushPin);
        ((FrameworkElement) this._copiedPushPin).LayoutUpdated -= new EventHandler(this.PushpinLayoutUpdated);
        this._copiedPushPin.IsHidden = false;
        this._copiedPushPin = (PushPin) null;
      }
      if (this._topPushPin == null || !allPushPins.All<PushPin>((Func<PushPin, bool>) (p => p.ContentVisibility == 1)))
        return;
      ((UIElement) this._topPushPin).Visibility = (Visibility) 1;
    }

    public void OnDetaching() => this.RemoveTopPushPin();

    public void PushPinRemoved(PushPin pushpin)
    {
      if (pushpin != this._copiedPushPin)
        return;
      PushPinTopAction.SwitchContent(this._topPushPin, this._copiedPushPin);
      ((FrameworkElement) this._copiedPushPin).LayoutUpdated -= new EventHandler(this.PushpinLayoutUpdated);
      this._copiedPushPin.IsHidden = false;
      this._copiedPushPin = (PushPin) null;
      ((UIElement) this._topPushPin).Visibility = (Visibility) 1;
    }

    private void RemoveTopPushPin()
    {
      if (this._topPushPin == null || !((PresentationFrameworkCollection<UIElement>) this._map.ContentLayer.Children).Contains((UIElement) this._topPushPin))
        return;
      ((FrameworkElement) this._topPushPin).LayoutUpdated -= new EventHandler(this.TopPushPinLayoutUpdated);
      ((PresentationFrameworkCollection<UIElement>) this._map.ContentLayer.Children).Remove((UIElement) this._topPushPin);
    }

    private void CopySettingsToTopPushPin([NotNull] PushPin pushpin)
    {
      if (!this.TryAddTopPushPin() || this._copiedPushPin == pushpin || pushpin.Content == null && pushpin.ContentTemplate == null)
        return;
      if (this._copiedPushPin != null)
        ((FrameworkElement) this._copiedPushPin).LayoutUpdated -= new EventHandler(this.PushpinLayoutUpdated);
      this._copiedPushPin = pushpin;
      pushpin.IsHidden = true;
      ((FrameworkElement) pushpin).LayoutUpdated += new EventHandler(this.PushpinLayoutUpdated);
      ((UIElement) this._topPushPin).Visibility = (Visibility) 0;
      this._topPushPin.ContentVisibility = (Visibility) 0;
      PushPinTopAction.SwitchContent(pushpin, this._topPushPin);
      ((Control) this._topPushPin).Template = ((Control) pushpin).Template;
      ((FrameworkElement) this._topPushPin).Style = ((FrameworkElement) pushpin).Style;
      BitmapSource expandedImageSource = pushpin.ExpandedImageSource;
      this._topPushPin.ExpandedImageSource = expandedImageSource == null || expandedImageSource.PixelHeight <= 0 || expandedImageSource.PixelWidth <= 0 ? expandedImageSource : (BitmapSource) new WriteableBitmap(expandedImageSource);
      ((FrameworkElement) this._topPushPin).DataContext = ((FrameworkElement) pushpin).DataContext;
      ((FrameworkElement) this._topPushPin).SetBinding(Control.BackgroundProperty, new Binding()
      {
        Path = new PropertyPath("Background", new object[0]),
        Source = (object) pushpin,
        Mode = (BindingMode) 1
      });
      ((FrameworkElement) this._topPushPin).SetBinding(Control.PaddingProperty, new Binding()
      {
        Path = new PropertyPath("Padding", new object[0]),
        Source = (object) pushpin,
        Mode = (BindingMode) 1
      });
      ((FrameworkElement) this._topPushPin).SetBinding(PushPin.StateProperty, new Binding()
      {
        Path = new PropertyPath("State", new object[0]),
        Source = (object) pushpin,
        Mode = (BindingMode) 1
      });
      FrameworkElement locationSource = this.GetLocationSource(pushpin);
      Binding binding1 = AttachedPropertyBindingProvider.GetBinding("MapLayer.Location");
      binding1.Mode = (BindingMode) 1;
      binding1.Source = (object) locationSource;
      ((FrameworkElement) this._topPushPin).SetBinding(MapLayer.LocationProperty, binding1);
      Binding binding2 = AttachedPropertyBindingProvider.GetBinding("MapLayer.PositionOffset");
      binding2.Mode = (BindingMode) 1;
      binding2.Source = (object) locationSource;
      ((FrameworkElement) this._topPushPin).SetBinding(MapLayer.PositionOffsetProperty, binding2);
      Binding binding3 = AttachedPropertyBindingProvider.GetBinding("MapLayer.Alignment");
      binding3.Mode = (BindingMode) 1;
      binding3.Source = (object) locationSource;
      ((FrameworkElement) this._topPushPin).SetBinding(MapLayer.AlignmentProperty, binding3);
      this._topPushpinVisibilityUpdatePending = true;
    }

    private void PushpinLayoutUpdated(object sender, EventArgs e)
    {
      if (this._copiedPushPin == null)
        return;
      ((UIElement) this._topPushPin).Visibility = ((DependencyObject) this._copiedPushPin).GetVisualAncestorsAndSelf().OfType<UIElement>().Any<UIElement>((Func<UIElement, bool>) (ancestor => ancestor.Visibility == 1)) ? (Visibility) 1 : (Visibility) 0;
    }

    private FrameworkElement GetLocationSource(PushPin pushpin) => ((DependencyObject) pushpin).GetValue(MapLayer.LocationProperty) == null ? ((FrameworkElement) pushpin).GetVisualParent() : (FrameworkElement) pushpin;

    private static void SwitchContent(PushPin pushpin1, PushPin pushPin2)
    {
      object content = pushpin1.Content;
      DataTemplate contentTemplate = pushpin1.ContentTemplate;
      pushpin1.Content = (object) null;
      pushpin1.ContentTemplate = (DataTemplate) null;
      pushPin2.Content = content;
      pushPin2.ContentTemplate = contentTemplate;
    }

    private bool TryAddTopPushPin()
    {
      if (this._map.ContentLayer == null)
        return false;
      if (this._topPushPin != null)
        return true;
      PushPin pushPin = new PushPin();
      ((UIElement) pushPin).CacheMode = (CacheMode) new BitmapCache();
      this._topPushPin = pushPin;
      ((DependencyObject) this._topPushPin).SetValue(Canvas.ZIndexProperty, (object) 100700);
      ((FrameworkElement) this._topPushPin).LayoutUpdated += new EventHandler(this.TopPushPinLayoutUpdated);
      this._topPushPin.BalloonTapped += new EventHandler<GestureEventArgs>(this.TopPushPinOnBalloonTapped);
      this._topPushPin.ContentVisibilityChanged += new EventHandler(this.TopPushPinContentVisibilityChanged);
      this._map.ContentLayer.AddChild((UIElement) this._topPushPin);
      return true;
    }

    private void TopPushPinOnBalloonTapped(object sender, GestureEventArgs gestureEventArgs)
    {
      if (this._copiedPushPin == null)
        return;
      this._copiedPushPin.OnBalloonTapped(gestureEventArgs);
    }

    private void TopPushPinContentVisibilityChanged(object sender, EventArgs e)
    {
      if (((PushPin) sender).ContentVisibility != 1 || this._copiedPushPin == null)
        return;
      this._copiedPushPin.ContentVisibility = (Visibility) 1;
    }

    private void TopPushPinLayoutUpdated(object sender, EventArgs e)
    {
      if (!this._topPushpinVisibilityUpdatePending)
        return;
      this._topPushpinVisibilityUpdatePending = false;
      if (this._oldCopiedPushPin != this._copiedPushPin)
        this.EnsureControlVisibility((UIElement) this._topPushPin);
      this._oldCopiedPushPin = this._copiedPushPin;
    }

    private void EnsureControlVisibility(UIElement control)
    {
      if (this._map == null || control == null)
        return;
      this._map.EnsureControlIsVisible(control);
    }
  }
}
