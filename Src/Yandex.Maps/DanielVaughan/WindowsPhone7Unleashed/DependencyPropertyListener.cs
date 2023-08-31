// Decompiled with JetBrains decompiler
// Type: DanielVaughan.WindowsPhone7Unleashed.DependencyPropertyListener
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DanielVaughan.WindowsPhone7Unleashed
{
  internal class DependencyPropertyListener
  {
    private static int index;
    private readonly DependencyProperty property;
    private FrameworkElement target;

    public DependencyPropertyListener() => this.property = DependencyProperty.RegisterAttached(nameof (DependencyPropertyListener) + (object) DependencyPropertyListener.index++, typeof (object), typeof (DependencyPropertyListener), new PropertyMetadata((object) null, new PropertyChangedCallback(this.HandleValueChanged)));

    public event EventHandler<BindingChangedEventArgs> Changed;

    private void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => this.OnChanged(new BindingChangedEventArgs(e));

    protected void OnChanged(BindingChangedEventArgs e)
    {
      EventHandler<BindingChangedEventArgs> changed = this.Changed;
      if (changed == null)
        return;
      changed((object) this.target, e);
    }

    public void Attach(FrameworkElement element, Binding binding)
    {
      this.target = this.target == null ? element : throw new Exception("Cannot attach an already attached listener");
      this.target.SetBinding(this.property, binding);
    }

    public void AttachSelf(Binding binding)
    {
      this.target = (FrameworkElement) new Border();
      this.target.SetBinding(this.property, binding);
    }

    public void Detach()
    {
      ((DependencyObject) this.target).ClearValue(this.property);
      this.target = (FrameworkElement) null;
    }

    public static DependencyPropertyListener Attach(
      [NotNull] FrameworkElement element,
      [NotNull] string property,
      [NotNull] EventHandler<BindingChangedEventArgs> callback)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      if (string.IsNullOrEmpty(property))
        throw new ArgumentOutOfRangeException(nameof (property));
      if (callback == null)
        throw new ArgumentNullException(nameof (callback));
      DependencyPropertyListener propertyListener = new DependencyPropertyListener();
      propertyListener.Changed += callback;
      propertyListener.Attach(element, new Binding()
      {
        Path = new PropertyPath(property, new object[0]),
        Source = (object) element
      });
      return propertyListener;
    }
  }
}
