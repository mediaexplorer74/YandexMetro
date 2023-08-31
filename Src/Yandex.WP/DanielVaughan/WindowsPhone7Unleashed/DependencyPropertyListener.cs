// Decompiled with JetBrains decompiler
// Type: DanielVaughan.WindowsPhone7Unleashed.DependencyPropertyListener
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DanielVaughan.WindowsPhone7Unleashed
{
  public class DependencyPropertyListener
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
  }
}
