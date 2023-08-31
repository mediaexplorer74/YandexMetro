// Decompiled with JetBrains decompiler
// Type: System.Windows.HierarchicalDataTemplate
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

using System.Windows.Data;

namespace System.Windows
{
  public class HierarchicalDataTemplate : DataTemplate
  {
    private DataTemplate _itemTemplate;
    private Style _itemContainerStyle;

    public Binding ItemsSource { get; set; }

    internal bool IsItemTemplateSet { get; private set; }

    public DataTemplate ItemTemplate
    {
      get => this._itemTemplate;
      set
      {
        this.IsItemTemplateSet = true;
        this._itemTemplate = value;
      }
    }

    internal bool IsItemContainerStyleSet { get; private set; }

    public Style ItemContainerStyle
    {
      get => this._itemContainerStyle;
      set
      {
        this.IsItemContainerStyleSet = true;
        this._itemContainerStyle = value;
      }
    }
  }
}
