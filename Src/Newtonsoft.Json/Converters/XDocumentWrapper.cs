// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XDocumentWrapper
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XDocumentWrapper : XContainerWrapper, IXmlDocument, IXmlNode
  {
    private XDocument Document => (XDocument) this.WrappedNode;

    public XDocumentWrapper(XDocument document)
      : base((XContainer) document)
    {
    }

    public override IList<IXmlNode> ChildNodes
    {
      get
      {
        IList<IXmlNode> childNodes = base.ChildNodes;
        if (this.Document.Declaration != null)
          childNodes.Insert(0, (IXmlNode) new XDeclarationWrapper(this.Document.Declaration));
        return childNodes;
      }
    }

    public IXmlNode CreateComment(string text) => (IXmlNode) new XObjectWrapper((XObject) new XComment(text));

    public IXmlNode CreateTextNode(string text) => (IXmlNode) new XObjectWrapper((XObject) new XText(text));

    public IXmlNode CreateCDataSection(string data) => (IXmlNode) new XObjectWrapper((XObject) new XCData(data));

    public IXmlNode CreateWhitespace(string text) => (IXmlNode) new XObjectWrapper((XObject) new XText(text));

    public IXmlNode CreateSignificantWhitespace(string text) => (IXmlNode) new XObjectWrapper((XObject) new XText(text));

    public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone) => (IXmlNode) new XDeclarationWrapper(new XDeclaration(version, encoding, standalone));

    public IXmlNode CreateProcessingInstruction(string target, string data) => (IXmlNode) new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));

    public IXmlElement CreateElement(string elementName) => (IXmlElement) new XElementWrapper(new XElement((XName) elementName));

    public IXmlElement CreateElement(string qualifiedName, string namespaceURI) => (IXmlElement) new XElementWrapper(new XElement(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceURI)));

    public IXmlNode CreateAttribute(string name, string value) => (IXmlNode) new XAttributeWrapper(new XAttribute((XName) name, (object) value));

    public IXmlNode CreateAttribute(string qualifiedName, string namespaceURI, string value) => (IXmlNode) new XAttributeWrapper(new XAttribute(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceURI), (object) value));

    public IXmlElement DocumentElement => this.Document.Root == null ? (IXmlElement) null : (IXmlElement) new XElementWrapper(this.Document.Root);

    public override IXmlNode AppendChild(IXmlNode newChild)
    {
      if (!(newChild is XDeclarationWrapper xdeclarationWrapper))
        return base.AppendChild(newChild);
      this.Document.Declaration = xdeclarationWrapper._declaration;
      return (IXmlNode) xdeclarationWrapper;
    }
  }
}
