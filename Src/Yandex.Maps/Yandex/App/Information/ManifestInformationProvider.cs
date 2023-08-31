// Decompiled with JetBrains decompiler
// Type: Yandex.App.Information.ManifestInformationProvider
// Assembly: Yandex.Maps, Version=1.2.4721.1342, Culture=neutral, PublicKeyToken=null
// MVID: 33D344FB-313B-41ED-9CE8-725920EAA345
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Maps.dll

using JetBrains.Annotations;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Xml.Linq;
using Yandex.Common;

namespace Yandex.App.Information
{
  [UsedImplicitly]
  internal class ManifestInformationProvider : IManifestInformationProvider
  {
    private const string ManifestXmlFileName = "WMAppManifest.xml";

    public ManifestInformationProvider()
    {
      this.Info = new ManifestInformation();
      StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("WMAppManifest.xml", UriKind.Relative));
      if (resourceStream == null)
        return;
      try
      {
        this.Initialize(resourceStream.Stream);
      }
      catch (InvalidOperationException ex)
      {
        Logger.TrackException((Exception) ex);
      }
    }

    public ManifestInformation Info { get; private set; }

    private void Initialize(Stream stream)
    {
      XElement element = ManifestInformationProvider.GetElement((XContainer) ManifestInformationProvider.GetElement((XContainer) XDocument.Load(stream), "Deployment"), "App");
      this.Info.ApplicationName = ManifestInformationProvider.GetAttributeValue(element, "Title");
      this.Info.ProductId = ManifestInformationProvider.GetAttributeValue(element, "ProductID");
      this.Info.ImageUri = ManifestInformationProvider.GetElement((XContainer) ManifestInformationProvider.GetElement((XContainer) element, "Tokens").Elements((XName) "PrimaryToken").First<XElement>((Func<XElement, bool>) (x => ManifestInformationProvider.GetAttributeValue(x, "TaskName") == "_default")).Elements().First<XElement>((Func<XElement, bool>) (x => x.Name.LocalName == "TemplateType5" || x.Name.LocalName == "TemplateFlip")), "BackgroundImageURI").Value;
      this.Info.Image = new BitmapImage(new Uri("/" + this.Info.ImageUri, UriKind.Relative));
    }

    private static XElement GetElement(XContainer container, string name)
    {
      if (container == null)
        throw new ArgumentNullException(nameof (container));
      return container.Elements().FirstOrDefault<XElement>((Func<XElement, bool>) (x => x.Name.LocalName == name)) ?? throw new InvalidOperationException("Incorrect manifest format");
    }

    private static string GetAttributeValue(XElement element, string name)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      return element.Attribute((XName) name)?.Value;
    }
  }
}
