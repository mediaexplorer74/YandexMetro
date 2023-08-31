// Decompiled with JetBrains decompiler
// Type: Yandex.Controls.TextBlockEx
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Yandex.Controls
{
  internal class TextBlockEx
  {
    public static readonly DependencyProperty InlinesProperty = DependencyProperty.RegisterAttached("Inlines", typeof (IEnumerable), typeof (TextBlockEx), new PropertyMetadata(new PropertyChangedCallback(TextBlockEx.InlinesPropertyChanged)));

    public static void SetInlines(TextBlock block, IEnumerable value) => ((DependencyObject) block).SetValue(TextBlockEx.InlinesProperty, (object) value);

    public static IEnumerable GetInlines(TextBlock block) => (IEnumerable) ((DependencyObject) block).GetValue(TextBlockEx.InlinesProperty);

    private static void InlinesPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      TextBlock textBlock = (TextBlock) d;
      ((PresentationFrameworkCollection<Inline>) textBlock.Inlines).Clear();
      if (e.NewValue == null)
        return;
      foreach (object obj in (IEnumerable) e.NewValue)
      {
        if (obj is Inline)
          ((PresentationFrameworkCollection<Inline>) textBlock.Inlines).Add((Inline) obj);
        else
          textBlock.Inlines.Add(obj.ToString());
      }
    }
  }
}
