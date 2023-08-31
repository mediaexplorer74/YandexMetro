// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Behaviors.SmartTrimmingTextBlockBehavior
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Y.UI.Common.Behaviors
{
  public class SmartTrimmingTextBlockBehavior : Behavior<TextBlock>
  {
    public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof (string), typeof (SmartTrimmingTextBlockBehavior), new PropertyMetadata(new PropertyChangedCallback(SmartTrimmingTextBlockBehavior.OnTextChanged)));

    public static string GetText(DependencyObject d) => (string) d.GetValue(SmartTrimmingTextBlockBehavior.TextProperty);

    public static void SetText(DependencyObject d, string value) => d.SetValue(SmartTrimmingTextBlockBehavior.TextProperty, (object) value);

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs p) => ((Behavior<TextBlock>) d).AssociatedObject.Text = (string) p.NewValue;

    public double? MaxTextWidth { get; set; }

    public double? TextTrimmingMargin { get; set; }

    public double? EllipcesSize { get; set; }

    protected override void OnAttached()
    {
      this.RedirectAssociatedTextBindingIfExists();
      ((FrameworkElement) this.AssociatedObject).SizeChanged += new SizeChangedEventHandler(this.AssociatedObject_SizeChanged);
      base.OnAttached();
    }

    protected override void OnDetaching()
    {
      ((FrameworkElement) this.AssociatedObject).SizeChanged -= new SizeChangedEventHandler(this.AssociatedObject_SizeChanged);
      base.OnDetaching();
    }

    private void RedirectAssociatedTextBindingIfExists()
    {
      BindingExpression bindingExpression = ((FrameworkElement) this.AssociatedObject).GetBindingExpression(TextBlock.TextProperty);
      if (bindingExpression == null)
        return;
      BindingOperations.SetBinding((DependencyObject) this, SmartTrimmingTextBlockBehavior.TextProperty, (BindingBase) bindingExpression.ParentBinding);
      this.AssociatedObject.Text = SmartTrimmingTextBlockBehavior.GetText((DependencyObject) this);
    }

    private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      double num1 = this.MaxTextWidth ?? ((FrameworkElement) ((FrameworkElement) this.AssociatedObject).Parent).ActualWidth;
      double actualWidth = ((FrameworkElement) this.AssociatedObject).ActualWidth;
      double num2 = this.TextTrimmingMargin ?? 20.0;
      double num3 = this.EllipcesSize ?? 20.0;
      double num4 = num1 - num2;
      if (actualWidth <= num4)
        return;
      string text = this.AssociatedObject.Text;
      if (string.IsNullOrEmpty(text) || text.Length < 5)
        return;
      string str = string.Empty;
      for (int length = 5; length < text.Length; ++length)
      {
        this.AssociatedObject.Text = text.Substring(0, length);
        if (((FrameworkElement) this.AssociatedObject).ActualWidth <= num4 - num3)
          str = this.AssociatedObject.Text;
        else
          break;
      }
      this.AssociatedObject.Text = str + "...";
    }
  }
}
