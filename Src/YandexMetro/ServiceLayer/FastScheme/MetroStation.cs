// Decompiled with JetBrains decompiler
// Type: Y.Metro.ServiceLayer.FastScheme.MetroStation
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using Yandex.Metro.Logic.FastScheme;
using Yandex.Metro.Views;

namespace Y.Metro.ServiceLayer.FastScheme
{
  [DataContract(IsReference = true)]
  public class MetroStation : BaseObject
  {
    [DataMember]
    public int Id;
    [DataMember]
    public int OrderId;
    [DataMember]
    public int LineId;
    [DataMember]
    public Point SchemePosition;
    [DataMember]
    public GeoPoint Coordinates;
    [DataMember]
    public BoardInfo BoardInfo;
    [DataMember]
    public PointWeights Weights;
    [DataMember]
    public bool IsTransfer;
    [DataMember]
    public OldName[] OldNames;
    public int tempMark;
    public int constMark;
    public string Letter;
    public Point TextPosition;
    public SolidColorBrush SameEllipseBrush;
    private bool _isSelect;
    private SolidColorBrush _selectBackground;
    private SolidColorBrush _background;
    private SolidColorBrush _selectForeground;
    private SolidColorBrush _foreground;
    private string _routeTime;
    private string _intervalTime;
    private string _shortLine;

    [DataMember]
    public int SameAsId { get; set; }

    [DataMember]
    public MetroStationName Name { get; set; }

    public LineForGroup LineForGroup { get; set; }

    public MetroLine LineReference { get; set; }

    public bool IsSelect
    {
      get => this._isSelect;
      set
      {
        if (this._isSelect == value)
          return;
        this._isSelect = value;
        this.RaisePropertyChanged("Background");
        this.RaisePropertyChanged("Foreground");
        this.RaisePropertyChanged("EllipseVisibility");
        this.RaisePropertyChanged("EllipseBrush");
        this.RaisePropertyChanged(nameof (IsSelect));
      }
    }

    public SolidColorBrush EllipseBrush => this.SameEllipseBrush ?? MapGenerator.GetBrush(this.LineReference.Color);

    public Visibility EllipseVisibility => !this.IsSelect ? (Visibility) 1 : (Visibility) 0;

    public SolidColorBrush Background
    {
      get => this.IsSelect ? this._selectBackground ?? (this._selectBackground = MapGenerator.GetBrush("#C8000000".ToColor())) : this._background ?? (this._background = MapGenerator.GetBrush("#D8ffffff".ToColor()));
      set
      {
        if (this._background == value)
          return;
        this._background = value;
        this.RaisePropertyChanged(nameof (Background));
      }
    }

    public SolidColorBrush Foreground
    {
      get => this.IsSelect ? this._selectForeground ?? (this._selectForeground = MapGenerator.GetBrush(Colors.White)) : this._foreground ?? (this._foreground = MapGenerator.GetBrush(Colors.Black));
      set
      {
        if (this._foreground == value)
          return;
        this._foreground = value;
        this.RaisePropertyChanged(nameof (Foreground));
      }
    }

    public string ArrivalTime
    {
      get => this._routeTime;
      set
      {
        if (this._routeTime == value)
          return;
        this._routeTime = value;
        this.RaisePropertyChanged(nameof (ArrivalTime));
      }
    }

    public string IntervalTime
    {
      get => this._intervalTime;
      set
      {
        if (this._intervalTime == value)
          return;
        this._intervalTime = value;
        this.RaisePropertyChanged(nameof (IntervalTime));
      }
    }

    public Visibility SameAsIdVisible => this.SameAsId <= 0 ? (Visibility) 1 : (Visibility) 0;

    public string LineShortName
    {
      get
      {
        if (this._shortLine == null)
        {
          string title = this.LineReference.Title;
          if (!string.IsNullOrWhiteSpace(title))
          {
            List<string> list = ((IEnumerable<string>) title.Split(' ')).Select<string, string>((Func<string, string>) (s => char.ToUpper(s[0]).ToString())).ToList<string>();
            list.Insert(0, " (");
            list.Add(")");
            this._shortLine = string.Join(string.Empty, (IEnumerable<string>) list);
          }
        }
        return this._shortLine;
      }
    }
  }
}
