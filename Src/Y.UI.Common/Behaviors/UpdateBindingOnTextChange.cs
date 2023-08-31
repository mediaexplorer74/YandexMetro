// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Behaviors.UpdateBindingOnTextChange
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Y.UI.Common.Behaviors
{
  public class UpdateBindingOnTextChange : Behavior<AutoCompleteBox>
  {
    private BindingExpression _expression;

    protected override void OnAttached()
    {
      base.OnAttached();
      this._expression = ((FrameworkElement) this.AssociatedObject).GetBindingExpression(AutoCompleteBox.TextProperty);
      this.AssociatedObject.TextChanged += (RoutedEventHandler) ((s, e) => this.AssociatedObjectOnTextChanged());
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      this.AssociatedObject.TextChanged -= (RoutedEventHandler) ((s, e) => this.AssociatedObjectOnTextChanged());
      this._expression = (BindingExpression) null;
    }

    private void AssociatedObjectOnTextChanged()
    {
      if (this._expression == null)
        return;
      this._expression.UpdateSource();
    }
  }
}
