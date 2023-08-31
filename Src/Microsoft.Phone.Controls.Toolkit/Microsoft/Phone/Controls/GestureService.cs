// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.GestureService
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System;
using System.Windows;

namespace Microsoft.Phone.Controls
{
  public static class GestureService
  {
    public static readonly DependencyProperty GestureListenerProperty = DependencyProperty.RegisterAttached("GestureListener", typeof (GestureListener), typeof (GestureService), new PropertyMetadata((PropertyChangedCallback) null));

    public static GestureListener GetGestureListener(DependencyObject obj) => obj != null ? GestureService.GetGestureListenerInternal(obj, true) : throw new ArgumentNullException(nameof (obj));

    internal static GestureListener GetGestureListenerInternal(
      DependencyObject obj,
      bool createIfMissing)
    {
      GestureListener listenerInternal = (GestureListener) obj.GetValue(GestureService.GestureListenerProperty);
      if (listenerInternal == null && createIfMissing)
      {
        listenerInternal = new GestureListener();
        GestureService.SetGestureListenerInternal(obj, listenerInternal);
      }
      return listenerInternal;
    }

    [Obsolete("Do not add handlers using this method. Instead, use GetGestureListener, which will create a new instance if one is not already set, to add your handlers to an element.", true)]
    public static void SetGestureListener(DependencyObject obj, GestureListener value)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      GestureService.SetGestureListenerInternal(obj, value);
    }

    private static void SetGestureListenerInternal(DependencyObject obj, GestureListener value) => obj.SetValue(GestureService.GestureListenerProperty, (object) value);
  }
}
