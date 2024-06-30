// Decompiled with JetBrains decompiler
// Type: Yandex.Metro.Logic.IsolatedStorageHelper
// Assembly: Yandex.Metro, Version=1.1.4746.20832, Culture=neutral, PublicKeyToken=null
// MVID: F8AF277A-B809-4F0F-A562-4767E3B0ED08
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.Metro.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using Y.Common;
using Y.Metro.ServiceLayer.Entities;
using Y.Metro.ServiceLayer.Enums;
using Y.Metro.ServiceLayer.FastScheme;

namespace Yandex.Metro.Logic
{
  public static class IsolatedStorageHelper
  {
    private const int FileCopyBlockSize = 2048;

    internal static void EnsureShemasCopied()
    {
      if (MetroService.Instance.AppSettings.IsShemasCopiedToIso)
        return;
      using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
      {
        storeForApplication.CreateDirectory("Scheme");
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_1_ru.js", "Scheme\\scheme_1_ru.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_1_en.js", "Scheme\\scheme_1_en.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_2_ru.js", "Scheme\\scheme_2_ru.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_2_en.js", "Scheme\\scheme_2_en.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_8_ru.js", "Scheme\\scheme_8_ru.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_8_en.js", "Scheme\\scheme_8_en.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_8_uk.js", "Scheme\\scheme_8_uk.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_9_ru.js", "Scheme\\scheme_9_ru.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_9_en.js", "Scheme\\scheme_9_en.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_9_uk.js", "Scheme\\scheme_9_uk.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_13_ru.js", "Scheme\\scheme_13_ru.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_13_en.js", "Scheme\\scheme_13_en.js", true);
        storeForApplication.CopyBinaryFile("/Y.Metro.ServiceLayer;component/Scheme/scheme_13_be.js", "Scheme\\scheme_13_be.js", true);
      }
      MetroService.Instance.AppSettings.IsShemasCopiedToIso = true;
    }

    internal static MetroScheme ReadScheme(SchemeType city, string language)
    {
      using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
      {
        string str = string.Format("Scheme\\scheme_{0}_{1}.js", (object) (int) city, (object) language);
        if (!storeForApplication.FileExists(str))
          return (MetroScheme) null;
        using (IsolatedStorageFileStream storageFileStream = storeForApplication.OpenFile(str, (FileMode) 3, (FileAccess) 1, (FileShare) 1))
        {
          using (StreamReader streamReader = new StreamReader((Stream) storageFileStream))
            return JsonConvert.DeserializeObject<MetroScheme>(streamReader.ReadToEnd());
        }
      }
    }

    internal static void UpdateScheme(string schemeContent, int schemeId)
    {
      Scheme scheme = MetroService.Instance.AppSettings.SchemeData.Single<Scheme>((Func<Scheme, bool>) (r => r.Id == schemeId));
      schemepack schema1 = SerializeHelper.Deserialize<schemepack>(schemeContent);
      schemepackScheme schemepackScheme1 = ((IEnumerable<schemepackScheme>) schema1.scheme).FirstOrDefault<schemepackScheme>((Func<schemepackScheme, bool>) (r => r.type == "default"));
      LanguageCode languageCode1 = scheme.LanguageCodes.Single<LanguageCode>((Func<LanguageCode, bool>) (r => r.IsDefault));
      IsolatedStorageHelper.SaveScheme(schemeId, languageCode1, schema1);
      foreach (LanguageCode languageCode2 in scheme.LanguageCodes.Where<LanguageCode>((Func<LanguageCode, bool>) (r => !r.IsDefault)))
      {
        LanguageCode languageCode = languageCode2;
        schemepackScheme schemepackScheme2 = ((IEnumerable<schemepackScheme>) schema1.scheme).FirstOrDefault<schemepackScheme>((Func<schemepackScheme, bool>) (r => r.locale == languageCode.Code));
        if (schemepackScheme1 != null && schemepackScheme2 != null)
        {
          schemepackScheme1.locale = languageCode.Code;
          schemepackScheme1.type = "language addition";
          foreach (schemepackSchemeStationsStation station in schemepackScheme2.stations)
          {
            schemepackSchemeStationsStation lgStation = station;
            schemepackSchemeStationsStation schemeStationsStation = ((IEnumerable<schemepackSchemeStationsStation>) schemepackScheme1.stations).FirstOrDefault<schemepackSchemeStationsStation>((Func<schemepackSchemeStationsStation, bool>) (r => r.id == lgStation.id));
            if (schemeStationsStation != null)
            {
              schemepackSchemeStationsStationName name = lgStation.name;
              if (!string.IsNullOrWhiteSpace(name.text))
                schemeStationsStation.name.text = name.text;
              if (!string.IsNullOrWhiteSpace(name.sameAsForStation))
                schemeStationsStation.name.sameAsForStation = name.sameAsForStation;
              schemeStationsStation.name.textLines = name.textLines;
              if (name.customSchemePosition != null && name.customSchemePosition.x > 0.0 && name.customSchemePosition.y > 0.0)
                schemeStationsStation.name.customSchemePosition = name.customSchemePosition;
            }
          }
          foreach (schemepackSchemeLinesLine line in schemepackScheme2.lines)
          {
            schemepackSchemeLinesLine lgLine = line;
            schemepackSchemeLinesLine schemepackSchemeLinesLine = ((IEnumerable<schemepackSchemeLinesLine>) schemepackScheme1.lines).FirstOrDefault<schemepackSchemeLinesLine>((Func<schemepackSchemeLinesLine, bool>) (r => r.id == lgLine.id));
            if (schemepackSchemeLinesLine != null && !string.IsNullOrWhiteSpace(lgLine.name))
              schemepackSchemeLinesLine.name = lgLine.name;
          }
          foreach (schemepackSchemeOptions option in schemepackScheme2.options)
          {
            schemepackSchemeOptions schemepackSchemeOptions = ((IEnumerable<schemepackSchemeOptions>) schemepackScheme1.options).FirstOrDefault<schemepackSchemeOptions>();
            if (schemepackSchemeOptions != null && !string.IsNullOrWhiteSpace(option.name))
              schemepackSchemeOptions.name = option.name;
          }
        }
        List<schemepackScheme> schemepackSchemeList = new List<schemepackScheme>()
        {
          schemepackScheme1
        };
        schemepack schema2 = new schemepack()
        {
          id = schema1.id,
          ver = schema1.ver,
          scheme = schemepackSchemeList.ToArray()
        };
        IsolatedStorageHelper.SaveScheme(schemeId, languageCode, schema2);
      }
    }

    private static void SaveScheme(int schemeId, LanguageCode languageCode, schemepack schema)
    {
      using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
      {
        MetroScheme metroScheme = SchemeToFastSchemeConverter.Convert(schema);
        string str1 = string.Format("Scheme\\scheme_{0}_{1}.js", (object) schemeId, (object) languageCode.Code);
        string str2 = JsonConvert.SerializeObject((object) metroScheme);
        using (IsolatedStorageFileStream storageFileStream = storeForApplication.OpenFile(str1, (FileMode) 2, (FileAccess) 3, (FileShare) 3))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) storageFileStream))
            streamWriter.Write(str2);
        }
      }
    }

    internal static void CopyBinaryFile(
      this IsolatedStorageFile isolatedStorageFile,
      string url,
      string filename,
      bool replace = false)
    {
      if (isolatedStorageFile.FileExists(filename) && !replace)
        return;
      StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri(url, UriKind.Relative));
      if (resourceStream == null)
        return;
      using (BinaryReader binaryReader = new BinaryReader(resourceStream.Stream))
      {
        IsolatedStorageFileStream file = isolatedStorageFile.CreateFile(filename);
        bool flag = false;
        long length = binaryReader.BaseStream.Length;
        int count = 2048;
        while (!flag)
        {
          if (length < 2048L)
          {
            count = Convert.ToInt32(length);
            ((Stream) file).Write(binaryReader.ReadBytes(count), 0, count);
          }
          else
            ((Stream) file).Write(binaryReader.ReadBytes(count), 0, count);
          length -= 2048L;
          if (length <= 0L)
            flag = true;
        }
        binaryReader.Close();
        ((Stream) file).Close();
      }
    }
  }
}
