// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XObjectWrapper
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
  internal class XObjectWrapper : IXmlNode
  {
    private readonly XObject _xmlObject;

    public XObjectWrapper(XObject xmlObject) => this._xmlObject = xmlObject;

    public object WrappedNode => (object) this._xmlObject;

    public virtual XmlNodeType NodeType => this._xmlObject.NodeType;

    public virtual string LocalName => (string) null;

    public virtual IList<IXmlNode> ChildNodes => (IList<IXmlNode>) new List<IXmlNode>();

    public virtual IList<IXmlNode> Attributes => (IList<IXmlNode>) null;

    public virtual IXmlNode ParentNode => (IXmlNode) null;

    public virtual string Value
    {
      get => (string) null;
      set => throw new InvalidOperationException();
    }

    public virtual IXmlNode AppendChild(IXmlNode newChild) => throw new InvalidOperationException();

    public virtual string NamespaceURI => (string) null;
  }
}
