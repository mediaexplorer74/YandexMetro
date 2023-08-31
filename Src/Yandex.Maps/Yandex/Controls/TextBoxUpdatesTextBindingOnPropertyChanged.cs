// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.TextBoxUpdatesTextBindingOnPropertyChanged
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Yandex.Controls
{
  internal class TextBoxUpdatesTextBindingOnPropertyChanged : Behavior<TextBox>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      this.AssociatedObject.TextChanged += new TextChangedEventHandler(this.TextBoxTextChanged);
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      this.AssociatedObject.TextChanged -= new TextChangedEventHandler(this.TextBoxTextChanged);
    }

    private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
    {
      if (!this.Dispatcher.CheckAccess())
        this.Dispatcher.BeginInvoke((Action) (() => this.TextBoxTextChanged(sender, e)));
      else
        ((FrameworkElement) this.AssociatedObject).GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
    }
  }
}
