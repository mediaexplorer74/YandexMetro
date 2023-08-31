// Decompiled with JetBrains decompiler
// Type: Yandex.StringUtils.AnnotatedValue`1
// Assembly: Yandex.Portable, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3077E92B-8BC4-423A-A839-D70ABCEB82DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Portable.dll

namespace Yandex.StringUtils
{
  public class AnnotatedValue<T> : AnnotatedObject
  {
    public AnnotatedValue(T value, string annotation)
      : base((object) value, annotation)
    {
    }

    public T Value => (T) base.Value;
  }
}
