// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XContainerWrapper
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XContainerWrapper : XObjectWrapper
  {
    private XContainer Container => (XContainer) this.WrappedNode;

    public XContainerWrapper(XContainer container)
      : base((XObject) container)
    {
    }

    public override IList<IXmlNode> ChildNodes => (IList<IXmlNode>) this.Container.Nodes().Select<XNode, IXmlNode>((Func<XNode, IXmlNode>) (n => XContainerWrapper.WrapNode((XObject) n))).ToList<IXmlNode>();

    public override IXmlNode ParentNode => this.Container.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Container.Parent);

    internal static IXmlNode WrapNode(XObject node)
    {
      switch (node)
      {
        case XDocument _:
          return (IXmlNode) new XDocumentWrapper((XDocument) node);
        case XElement _:
          return (IXmlNode) new XElementWrapper((XElement) node);
        case XContainer _:
          return (IXmlNode) new XContainerWrapper((XContainer) node);
        case XProcessingInstruction _:
          return (IXmlNode) new XProcessingInstructionWrapper((XProcessingInstruction) node);
        case XText _:
          return (IXmlNode) new XTextWrapper((XText) node);
        case XComment _:
          return (IXmlNode) new XCommentWrapper((XComment) node);
        case XAttribute _:
          return (IXmlNode) new XAttributeWrapper((XAttribute) node);
        default:
          return (IXmlNode) new XObjectWrapper(node);
      }
    }

    public override IXmlNode AppendChild(IXmlNode newChild)
    {
      this.Container.Add(newChild.WrappedNode);
      return newChild;
    }
  }
}
