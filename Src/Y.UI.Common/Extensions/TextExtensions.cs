// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Extensions.TextExtensions
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Y.UI.Common.Extensions
{
  public class TextExtensions : FrameworkElement
  {
    public static readonly DependencyProperty ArticleContentProperty = DependencyProperty.RegisterAttached("InlineList", typeof (List<Inline>), typeof (TextExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(TextExtensions.OnInlineListPropertyChanged)));

    public static string GetInlineList(TextBlock element) => element != null ? ((DependencyObject) element).GetValue(TextExtensions.ArticleContentProperty) as string : string.Empty;

    public static void SetInlineList(TextBlock element, string value) => ((DependencyObject) element)?.SetValue(TextExtensions.ArticleContentProperty, (object) value);

    private static void OnInlineListPropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      TextBlock tb = obj as TextBlock;
      if (tb == null)
        return;
      ((PresentationFrameworkCollection<Inline>) tb.Inlines).Clear();
      if (!(e.NewValue is List<Inline> newValue))
        return;
      Action<Inline> action = (Action<Inline>) (inl => ((PresentationFrameworkCollection<Inline>) tb.Inlines).Add(inl));
      newValue.ForEach(action);
    }
  }
}
