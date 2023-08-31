// Decompiled with JetBrains decompiler
// Type: Y.UI.Common.Control.ImplicitDataTemplateControl
// Assembly: Y.UI.Common, Version=1.0.6124.20830, Culture=neutral, PublicKeyToken=null
// MVID: 5D744A46-B2F9-409E-8109-6E29AB154B4E
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.UI.Common.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Y.UI.Common.Control
{
  public class ImplicitDataTemplateControl : ContentControl
  {
    public static DependencyProperty TemplatePrefixProperty = DependencyProperty.Register(nameof (TemplatePrefix), typeof (string), typeof (ImplicitDataTemplateControl), (PropertyMetadata) null);

    public string TemplatePrefix
    {
      get => (string) ((DependencyObject) this).GetValue(ImplicitDataTemplateControl.TemplatePrefixProperty);
      set => ((DependencyObject) this).SetValue(ImplicitDataTemplateControl.TemplatePrefixProperty, (object) value);
    }

    protected virtual void OnContentChanged(object oldContent, object newContent)
    {
      base.OnContentChanged(oldContent, newContent);
      DataTemplate dataTemplate = (DataTemplate) null;
      if (newContent != null)
        dataTemplate = ImplicitDataTemplateControl.InternalResolve((FrameworkElement) this, this.GetContentTypeName(this.TemplatePrefix, newContent));
      this.ContentTemplate = dataTemplate;
    }

    protected virtual string GetContentTypeName(string templatePrefix, object newContent) => templatePrefix + newContent.GetType().Name;

    protected static DataTemplate InternalResolve(FrameworkElement element, string contentTypeName) => element != null ? ImplicitDataTemplateControl.TryFindDataTemplate(element.Resources, contentTypeName) ?? ImplicitDataTemplateControl.InternalResolve(VisualTreeHelper.GetParent((DependencyObject) element) as FrameworkElement, contentTypeName) : ImplicitDataTemplateControl.TryFindDataTemplate(Application.Current.Resources, contentTypeName);

    private static DataTemplate TryFindDataTemplate(
      ResourceDictionary resourceDictionary,
      string contentTypeName)
    {
      DataTemplate dataTemplate = (DataTemplate) null;
      if (resourceDictionary.Contains((object) contentTypeName))
        dataTemplate = resourceDictionary[(object) contentTypeName] as DataTemplate;
      return dataTemplate;
    }
  }
}
