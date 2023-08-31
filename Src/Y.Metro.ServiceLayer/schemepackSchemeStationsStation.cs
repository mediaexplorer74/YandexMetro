// Decompiled with JetBrains decompiler
// Type: schemepackSchemeStationsStation
// Assembly: Y.Metro.ServiceLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A3B0825-7B56-4826-9B0E-51B7B9B4422B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Y.Metro.ServiceLayer.dll

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Xml.Schema;
using System.Xml.Serialization;

[DebuggerDisplay("{id}-l:{line}")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true)]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class schemepackSchemeStationsStation : BaseObject
{
  private int lineField;
  private string typeField;
  private schemepackSchemeStationsStationSchemePosition schemePositionField;
  private schemepackSchemeStationsStationGeoCoordinates geoCoordinatesField;
  private schemepackSchemeStationsStationName nameField;
  private schemepackSchemeStationsStationBoardInfo boardInfoField;
  private schemepackSchemeStationsStationOldNamesOldName[] oldNamesField;
  private schemepackSchemeStationsStationWeightsWeight[] weightsField;
  private int idField;
  private int _zIndex;
  private int _zIndexRoot;
  private bool _isSelect;
  private SolidColorBrush _selectBackground;
  private SolidColorBrush _background;
  private SolidColorBrush _selectForeground;
  private SolidColorBrush _foreground;
  private string _routeTime;
  private string _intervalTime;

  [XmlIgnore]
  public schemepackSchemeLinesLine SchemeLine { get; set; }

  [XmlArray(Form = XmlSchemaForm.Unqualified)]
  [XmlArrayItem("weight", typeof (schemepackSchemeStationsStationWeightsWeight), Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeStationsStationWeightsWeight[] weights
  {
    get => this.weightsField;
    set => this.weightsField = value;
  }

  [XmlElement(Form = XmlSchemaForm.Unqualified)]
  public int line
  {
    get => this.lineField;
    set => this.lineField = value;
  }

  [XmlElement(Form = XmlSchemaForm.Unqualified)]
  public string type
  {
    get => this.typeField;
    set => this.typeField = value;
  }

  [XmlElement("schemePosition", Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeStationsStationSchemePosition schemePosition
  {
    get => this.schemePositionField;
    set => this.schemePositionField = value;
  }

  [XmlElement("geoCoordinates", Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeStationsStationGeoCoordinates geoCoordinates
  {
    get => this.geoCoordinatesField;
    set => this.geoCoordinatesField = value;
  }

  [XmlElement("name", Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeStationsStationName name
  {
    get => this.nameField;
    set => this.nameField = value;
  }

  [XmlElement("boardInfo", Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeStationsStationBoardInfo boardInfo
  {
    get => this.boardInfoField;
    set => this.boardInfoField = value;
  }

  [XmlArrayItem("oldName", typeof (schemepackSchemeStationsStationOldNamesOldName), Form = XmlSchemaForm.Unqualified)]
  [XmlArray(Form = XmlSchemaForm.Unqualified)]
  public schemepackSchemeStationsStationOldNamesOldName[] oldNames
  {
    get => this.oldNamesField;
    set => this.oldNamesField = value;
  }

  [XmlAttribute]
  public int id
  {
    get => this.idField;
    set => this.idField = value;
  }

  [XmlIgnore]
  public int ZIndexRoute
  {
    get => this._zIndex;
    set
    {
      if (this._zIndex == value)
        return;
      this._zIndex = value;
      this.RaisePropertyChanged(nameof (ZIndexRoute));
    }
  }

  [XmlIgnore]
  public int ZIndexRoot
  {
    get => this._zIndexRoot;
    set
    {
      if (this._zIndexRoot == value)
        return;
      this._zIndexRoot = value;
      this.RaisePropertyChanged(nameof (ZIndexRoot));
    }
  }

  [XmlIgnore]
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
      this.RaisePropertyChanged(nameof (IsSelect));
    }
  }

  [XmlIgnore]
  public Point TextPosition { get; set; }

  [XmlIgnore]
  public SolidColorBrush Background
  {
    get => this.IsSelect ? this._selectBackground ?? (this._selectBackground = new SolidColorBrush("#C8000000".ToColor())) : this._background ?? (this._background = new SolidColorBrush("#D8ffffff".ToColor()));
    set
    {
      if (this._background == value)
        return;
      this._background = value;
      this.RaisePropertyChanged(nameof (Background));
    }
  }

  [XmlIgnore]
  public SolidColorBrush Foreground
  {
    get => this.IsSelect ? this._selectForeground ?? (this._selectForeground = new SolidColorBrush(Colors.White)) : this._foreground ?? (this._foreground = new SolidColorBrush(Colors.Black));
    set
    {
      if (this._foreground == value)
        return;
      this._foreground = value;
      this.RaisePropertyChanged(nameof (Foreground));
    }
  }

  [XmlIgnore]
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

  [XmlIgnore]
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

  [XmlIgnore]
  public bool GpsLocationDefined => this.geoCoordinates != null;
}
