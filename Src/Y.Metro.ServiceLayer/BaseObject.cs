// Decompiled with JetBrains decompiler
// Type: BaseObject
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System.ComponentModel;

public class BaseObject : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler PropertyChanged;

  protected void RaisePropertyChanged(string property)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(property));
  }
}
