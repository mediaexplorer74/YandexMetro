// Decompiled with JetBrains decompiler
// Type: System.Windows.Controls.OrientedSize
// Assembly: Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b772ad94eb9ca604
// MVID: AC8405BF-150B-4799-8456-BB93CA6B91DC
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Microsoft.Phone.Controls.Toolkit.dll

namespace System.Windows.Controls
{
  internal struct OrientedSize
  {
    private Orientation _orientation;
    private double _direct;
    private double _indirect;

    public Orientation Orientation => this._orientation;

    public double Direct
    {
      get => this._direct;
      set => this._direct = value;
    }

    public double Indirect
    {
      get => this._indirect;
      set => this._indirect = value;
    }

    public double Width
    {
      get => this.Orientation != 1 ? this.Indirect : this.Direct;
      set
      {
        if (this.Orientation == 1)
          this.Direct = value;
        else
          this.Indirect = value;
      }
    }

    public double Height
    {
      get => this.Orientation == 1 ? this.Indirect : this.Direct;
      set
      {
        if (this.Orientation != 1)
          this.Direct = value;
        else
          this.Indirect = value;
      }
    }

    public OrientedSize(Orientation orientation)
      : this(orientation, 0.0, 0.0)
    {
    }

    public OrientedSize(Orientation orientation, double width, double height)
    {
      this._orientation = orientation;
      this._direct = 0.0;
      this._indirect = 0.0;
      this.Width = width;
      this.Height = height;
    }
  }
}
