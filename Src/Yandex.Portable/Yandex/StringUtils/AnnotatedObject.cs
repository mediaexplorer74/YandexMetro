// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.AnnotatedObject
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

namespace Yandex.StringUtils
{
  public class AnnotatedObject
  {
    public AnnotatedObject(object value, string annotation)
    {
      this.Value = value;
      this.Annotation = annotation;
    }

    public object Value { get; private set; }

    public string Annotation { get; private set; }
  }
}
