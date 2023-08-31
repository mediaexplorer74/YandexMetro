// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonSerializerInternalBase
// Assembly: Newtonsoft.Json, Version=4.0.3.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: B19688F6-798A-4C62-919A-DE3F6BC2913B
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
  internal abstract class JsonSerializerInternalBase
  {
    private ErrorContext _currentErrorContext;
    private BidirectionalDictionary<string, object> _mappings;

    internal JsonSerializer Serializer { get; private set; }

    protected JsonSerializerInternalBase(JsonSerializer serializer)
    {
      ValidationUtils.ArgumentNotNull((object) serializer, nameof (serializer));
      this.Serializer = serializer;
    }

    internal BidirectionalDictionary<string, object> DefaultReferenceMappings
    {
      get
      {
        if (this._mappings == null)
          this._mappings = new BidirectionalDictionary<string, object>((IEqualityComparer<string>) EqualityComparer<string>.Default, (IEqualityComparer<object>) new JsonSerializerInternalBase.ReferenceEqualsEqualityComparer());
        return this._mappings;
      }
    }

    protected ErrorContext GetErrorContext(object currentObject, object member, Exception error)
    {
      if (this._currentErrorContext == null)
        this._currentErrorContext = new ErrorContext(currentObject, member, error);
      if (this._currentErrorContext.Error != error)
        throw new InvalidOperationException("Current error context error is different to requested error.");
      return this._currentErrorContext;
    }

    protected void ClearErrorContext() => this._currentErrorContext = this._currentErrorContext != null ? (ErrorContext) null : throw new InvalidOperationException("Could not clear error context. Error context is already null.");

    protected bool IsErrorHandled(
      object currentObject,
      JsonContract contract,
      object keyValue,
      Exception ex)
    {
      ErrorContext errorContext = this.GetErrorContext(currentObject, keyValue, ex);
      contract.InvokeOnError(currentObject, this.Serializer.Context, errorContext);
      if (!errorContext.Handled)
        this.Serializer.OnError(new ErrorEventArgs(currentObject, errorContext));
      return errorContext.Handled;
    }

    private class ReferenceEqualsEqualityComparer : IEqualityComparer<object>
    {
      bool IEqualityComparer<object>.Equals(object x, object y) => object.ReferenceEquals(x, y);

      int IEqualityComparer<object>.GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
    }
  }
}
