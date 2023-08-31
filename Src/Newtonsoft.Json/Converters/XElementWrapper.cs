// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XElementWrapper
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
  {
    private XElement Element => (XElement) this.WrappedNode;

    public XElementWrapper(XElement element)
      : base((XContainer) element)
    {
    }

    public void SetAttributeNode(IXmlNode attribute) => this.Element.Add(((XObjectWrapper) attribute).WrappedNode);

    public override IList<IXmlNode> Attributes => (IList<IXmlNode>) this.Element.Attributes().Select<XAttribute, XAttributeWrapper>((Func<XAttribute, XAttributeWrapper>) (a => new XAttributeWrapper(a))).Cast<IXmlNode>().ToList<IXmlNode>();

    public override string Value
    {
      get => this.Element.Value;
      set => this.Element.Value = value;
    }

    public override string LocalName => this.Element.Name.LocalName;

    public override string NamespaceURI => this.Element.Name.NamespaceName;

    public string GetPrefixOfNamespace(string namespaceURI) => this.Element.GetPrefixOfNamespace((XNamespace) namespaceURI);
  }
}
