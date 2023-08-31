// Decompiled with JetBrains decompiler
// Type: Yandex.Collections.ConvertingObservableCollection`2
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Yandex.Threading.Interfaces;

namespace Yandex.Collections
{
  public class ConvertingObservableCollection<T, TSource> : ObservableCollection<T>
  {
    private readonly IObservableEnumerable<TSource> _source;
    private readonly Func<TSource, T> _converter;
    private readonly Func<T, TSource, bool> _isSameSource;
    private readonly IUiDispatcher _uiDispatcher;

    public ConvertingObservableCollection(
      [NotNull] IObservableEnumerable<TSource> source,
      [NotNull] Func<TSource, T> converter,
      [NotNull] Func<T, TSource, bool> isSameSource,
      [NotNull] IUiDispatcher uiDispatcher)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (converter == null)
        throw new ArgumentNullException(nameof (converter));
      if (uiDispatcher == null)
        throw new ArgumentNullException(nameof (uiDispatcher));
      this._source = source;
      this._converter = converter;
      this._isSameSource = isSameSource;
      this._uiDispatcher = uiDispatcher;
      this.AddItems((IEnumerable<TSource>) this._source);
      this._source.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SourceCollectionChanged);
    }

    private void AddItems(IEnumerable<TSource> items) => this._uiDispatcher.BeginInvoke((Action) (() =>
    {
      foreach (TSource source in items)
      {
        // ISSUE: explicit non-virtual call
        __nonvirtual (((Collection<T>) this).Add(this._converter(source)));
      }
    }));

    private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          this.AddItems(e.NewItems.Cast<TSource>());
          break;
        case NotifyCollectionChangedAction.Remove:
          using (IEnumerator<TSource> enumerator = e.OldItems.Cast<TSource>().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              TSource sourceItem = enumerator.Current;
              // ISSUE: explicit non-virtual call
              __nonvirtual (((Collection<T>) this).Remove(((IEnumerable<T>) this).First<T>((Func<T, bool>) (item => this._isSameSource(item, sourceItem)))));
            }
            break;
          }
        case NotifyCollectionChangedAction.Replace:
          for (int newStartingIndex = e.NewStartingIndex; newStartingIndex < e.NewItems.Count; ++newStartingIndex)
          {
            // ISSUE: explicit non-virtual call
            __nonvirtual (((Collection<T>) this)[newStartingIndex]) = this._converter((TSource) e.NewItems[newStartingIndex]);
          }
          break;
        case NotifyCollectionChangedAction.Reset:
          this._uiDispatcher.BeginInvoke((Action) (() =>
          {
            // ISSUE: explicit non-virtual call
            __nonvirtual (((Collection<T>) this).Clear());
            this.AddItems((IEnumerable<TSource>) this._source);
          }));
          break;
      }
    }
  }
}
