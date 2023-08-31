// Decompiled with JetBrains decompiler
// Type: Yandex.Maps.PrinterClient.RequestState
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using Yandex.Maps.PrinterClient.Tiles;
using Yandex.WebUtils;
using Yandex.WebUtils.Interfaces;

namespace Yandex.Maps.PrinterClient
{
  internal class RequestState
  {
    public RequestState() => this.PostData = new PostData();

    public IHttpWebRequest WebRequest { get; set; }

    public TilesRequest TilesRequest { get; set; }

    public string Boundary { get; set; }

    public PostData PostData { get; private set; }
  }
}
