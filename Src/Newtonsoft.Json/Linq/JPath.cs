// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JPath
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
  internal class JPath
  {
    private readonly string _expression;
    private int _currentIndex;

    public List<object> Parts { get; private set; }

    public JPath(string expression)
    {
      ValidationUtils.ArgumentNotNull((object) expression, nameof (expression));
      this._expression = expression;
      this.Parts = new List<object>();
      this.ParseMain();
    }

    private void ParseMain()
    {
      int startIndex = this._currentIndex;
      bool flag = false;
      for (; this._currentIndex < this._expression.Length; ++this._currentIndex)
      {
        char indexerOpenChar = this._expression[this._currentIndex];
        switch (indexerOpenChar)
        {
          case '(':
          case '[':
            if (this._currentIndex > startIndex)
              this.Parts.Add((object) this._expression.Substring(startIndex, this._currentIndex - startIndex));
            this.ParseIndexer(indexerOpenChar);
            startIndex = this._currentIndex + 1;
            flag = true;
            break;
          case ')':
          case ']':
            throw new Exception("Unexpected character while parsing path: " + (object) indexerOpenChar);
          case '.':
            if (this._currentIndex > startIndex)
              this.Parts.Add((object) this._expression.Substring(startIndex, this._currentIndex - startIndex));
            startIndex = this._currentIndex + 1;
            flag = false;
            break;
          default:
            if (flag)
              throw new Exception("Unexpected character following indexer: " + (object) indexerOpenChar);
            break;
        }
      }
      if (this._currentIndex <= startIndex)
        return;
      this.Parts.Add((object) this._expression.Substring(startIndex, this._currentIndex - startIndex));
    }

    private void ParseIndexer(char indexerOpenChar)
    {
      ++this._currentIndex;
      char ch = indexerOpenChar == '[' ? ']' : ')';
      int currentIndex = this._currentIndex;
      int length = 0;
      bool flag = false;
      for (; this._currentIndex < this._expression.Length; ++this._currentIndex)
      {
        char c = this._expression[this._currentIndex];
        if (char.IsDigit(c))
        {
          ++length;
        }
        else
        {
          if ((int) c != (int) ch)
            throw new Exception("Unexpected character while parsing path indexer: " + (object) c);
          flag = true;
          break;
        }
      }
      if (!flag)
        throw new Exception("Path ended with open indexer. Expected " + (object) ch);
      if (length == 0)
        throw new Exception("Empty path indexer.");
      this.Parts.Add((object) Convert.ToInt32(this._expression.Substring(currentIndex, length), (IFormatProvider) CultureInfo.InvariantCulture));
    }

    internal JToken Evaluate(JToken root, bool errorWhenNoMatch)
    {
      JToken jtoken = root;
      foreach (object part in this.Parts)
      {
        if (part is string propertyName)
        {
          if (jtoken is JObject jobject)
          {
            jtoken = jobject[propertyName];
            if (jtoken == null && errorWhenNoMatch)
              throw new Exception("Property '{0}' does not exist on JObject.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName));
          }
          else
          {
            if (errorWhenNoMatch)
              throw new Exception("Property '{0}' not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName, (object) jtoken.GetType().Name));
            return (JToken) null;
          }
        }
        else
        {
          int index = (int) part;
          if (jtoken is JArray jarray)
          {
            if (jarray.Count <= index)
            {
              if (errorWhenNoMatch)
                throw new IndexOutOfRangeException("Index {0} outside the bounds of JArray.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) index));
              return (JToken) null;
            }
            jtoken = jarray[index];
          }
          else
          {
            if (errorWhenNoMatch)
              throw new Exception("Index {0} not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) index, (object) jtoken.GetType().Name));
            return (JToken) null;
          }
        }
      }
      return jtoken;
    }
  }
}
