// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.Wrappers.FrameworkElementWrapper
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Windows;
using Yandex.Maps.Interfaces;

namespace Yandex.Maps.Wrappers
{
  internal class FrameworkElementWrapper : IFrameworkElement
  {
    private FrameworkElement _frameworkElement;

    public FrameworkElementWrapper(FrameworkElement frameworkElement) => this._frameworkElement = frameworkElement != null ? frameworkElement : throw new ArgumentNullException(nameof (frameworkElement));

    public double ActualWidth => this._frameworkElement.ActualWidth;

    public double ActualHeight => this._frameworkElement.ActualHeight;
  }
}
