// Decompiled with JetBrains decompiler
// Type: Y.Common.Extensions.GetState
// Assembly: Y.Common, Version=1.0.6124.20828, Culture=neutral, PublicKeyToken=null
// MVID: A51713EB-DF7B-476D-8033-D13B637B3481
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Common.dll

using System;
using System.Net;

namespace Y.Common.Extensions
{
  public class GetState
  {
    public HttpWebRequest Request;
    public object UserInputData;
    public string ResponseContent;
    public Exception ResponseError;

    public GetState(HttpWebRequest request, object userInputData = null)
    {
      this.Request = request;
      this.UserInputData = userInputData;
    }
  }
}
