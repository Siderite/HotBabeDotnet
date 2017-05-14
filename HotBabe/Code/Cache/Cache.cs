using System;
using System.Collections.Generic;
using System.Drawing;
using HotBabe.Code.Helpers;

namespace HotBabe.Code
{
  [Serializable]
  public class ImageCache : Cache<Image>
  {
    public ImageCache()
    {
      MaxSize = long.MaxValue;
    }

    public long MaxSize { get; set; }
    public override Image Get(string key, Func<string, Image> predicate)
    {
      var image = base.Get(key, predicate);
      CheckCacheSize();
      return image;
    }

    public void CheckCacheSize()
    {
      List<CacheItem<Image>> list = new List<CacheItem<Image>>();
      foreach (KeyValuePair<string, CacheItem<Image>> pair in this)
      {
        pair.Value.Tag = pair.Key;
        list.Add(pair.Value);
      }
      list.Sort(new Comparison<CacheItem<Image>>((x, y) => x.TimeAccessed.CompareTo(y.TimeAccessed)));
      long sum = 0;
      foreach (CacheItem<Image> item in list)
      {
        sum += item.Value.GetMemorySize();
        if (sum > MaxSize) Remove((string)item.Tag);
      }
    }

  }

  [Serializable]
  public class Cache<T>:SerializableDictionary<string,CacheItem<T>>
  {
    public virtual T Get(string key,Func<string,T> predicate)
    {
      CacheItem<T> item;
      if (TryGetValue(key,out item))
      {
        item.TimeAccessed = DateTime.Now;
        return item.Value;
      }
      var value = predicate(key);
      item = new CacheItem<T>(value);
      this[key] = item;
      return value;
    }
  }

  [Serializable]
  public class CacheItem<T>
  {
    public CacheItem(T value)
    {
      Value = value;
      TimeChanged = DateTime.Now;
      TimeAccessed = DateTime.Now;
    }

    public DateTime TimeAccessed { get; set; }
    public DateTime TimeChanged { get; set; }
    public T Value { get; set; }
    public object Tag { get; set; }
  }
}