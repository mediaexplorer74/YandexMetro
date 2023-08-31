// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PushPinManagerHelper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Clarity.Phone.Extensions;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Yandex.Maps.Interfaces;

namespace Yandex.Maps
{
  [ComVisible(false)]
  public static class PushPinManagerHelper
  {
    public static readonly DependencyProperty GroupKeysProperty = DependencyProperty.RegisterAttached("GroupKeys", typeof (Collection<string>), typeof (PushPinManager), new PropertyMetadata((object) null, new PropertyChangedCallback(PushPinManagerHelper.GroupKeysPropertyChangedCallback)));
    private static readonly Dictionary<PushPin, IPushPinManager> Cache = new Dictionary<PushPin, IPushPinManager>();

    [TypeConverter(typeof (GroupKeysConverter))]
    [CanBeNull]
    public static Collection<string> GetGroupKeys(PushPin element)
    {
      if (!(((DependencyObject) element).GetValue(PushPinManagerHelper.GroupKeysProperty) is Collection<string> groupKeys))
      {
        groupKeys = new Collection<string>();
        ((DependencyObject) element).SetValue(PushPinManagerHelper.GroupKeysProperty, (object) groupKeys);
      }
      return groupKeys;
    }

    public static void SetGroupKeys(PushPin element, Collection<string> value) => ((DependencyObject) element).SetValue(PushPinManagerHelper.GroupKeysProperty, (object) value);

    private static void GroupKeysPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is PushPin pushPin))
        return;
      if (e.NewValue != null && e.OldValue == null)
      {
        ((FrameworkElement) pushPin).Loaded += new RoutedEventHandler(PushPinManagerHelper.PushpinLoaded);
        ((FrameworkElement) pushPin).Unloaded += new RoutedEventHandler(PushPinManagerHelper.PushpinUnloaded);
        pushPin.VisibilityChanged += new EventHandler(PushPinManagerHelper.PushpinVisibilityChanged);
      }
      else
      {
        ((FrameworkElement) pushPin).Loaded -= new RoutedEventHandler(PushPinManagerHelper.PushpinLoaded);
        ((FrameworkElement) pushPin).Unloaded -= new RoutedEventHandler(PushPinManagerHelper.PushpinUnloaded);
        pushPin.VisibilityChanged -= new EventHandler(PushPinManagerHelper.PushpinVisibilityChanged);
      }
      if (((UIElement) pushPin).Visibility == null && ((FrameworkElement) pushPin).GetVisualParent() != null && e.NewValue is Collection<string> newValue)
        PushPinManagerHelper.FindPushPinManagerAndSetPushPinGroups(pushPin, newValue.ToArray<string>());
      if (e.NewValue != null)
        return;
      lock (PushPinManagerHelper.Cache)
        PushPinManagerHelper.Cache.Remove(pushPin);
    }

    private static void PushpinLoaded(object sender, RoutedEventArgs e)
    {
      PushPin pushPin = (PushPin) sender;
      IPushPinManager pushPinManager = PushPinManagerHelper.GetPushPinManager(pushPin);
      if (pushPinManager != null)
      {
        lock (PushPinManagerHelper.Cache)
          PushPinManagerHelper.Cache[pushPin] = pushPinManager;
      }
      if (((UIElement) pushPin).Visibility != null)
        return;
      Collection<string> groupKeys = PushPinManagerHelper.GetGroupKeys(pushPin);
      if (groupKeys == null)
        return;
      PushPinManagerHelper.FindPushPinManagerAndSetPushPinGroups(pushPin, groupKeys.ToArray<string>());
    }

    private static void FindPushPinManagerAndSetPushPinGroups(PushPin pushpin, string[] groupKeys)
    {
      IPushPinManager pushPinManager;
      lock (PushPinManagerHelper.Cache)
        PushPinManagerHelper.Cache.TryGetValue(pushpin, out pushPinManager);
      pushPinManager?.SetPushPinGroups(pushpin, groupKeys);
    }

    private static void PushpinUnloaded(object sender, RoutedEventArgs e)
    {
      PushPin pushPin = (PushPin) sender;
      PushPinManagerHelper.FindPushPinManagerAndSetPushPinGroups(pushPin, (string[]) null);
      lock (PushPinManagerHelper.Cache)
        PushPinManagerHelper.Cache.Remove(pushPin);
    }

    private static void PushpinVisibilityChanged(object sender, EventArgs e)
    {
      PushPin pushPin = (PushPin) sender;
      if (((UIElement) pushPin).Visibility == null && ((FrameworkElement) pushPin).GetVisualParent() != null)
      {
        Collection<string> groupKeys = PushPinManagerHelper.GetGroupKeys(pushPin);
        if (groupKeys == null)
          return;
        PushPinManagerHelper.FindPushPinManagerAndSetPushPinGroups(pushPin, groupKeys.ToArray<string>());
      }
      else
        PushPinManagerHelper.FindPushPinManagerAndSetPushPinGroups(pushPin, (string[]) null);
    }

    [CanBeNull]
    private static IPushPinManager GetPushPinManager(PushPin pushpin) => ((DependencyObject) pushpin).GetVisualAncestors().OfType<MapBase>().SingleOrDefault<MapBase>()?.PushPinManager;
  }
}
