// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.FastScheme.BoardInfo
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Y.Metro.ServiceLayer.FastScheme
{
  [DataContract]
  public class BoardInfo
  {
    [DataMember]
    public List<int> Exit;
    [DataMember]
    public TransferBoardPositions[] Transfer;
  }
}
