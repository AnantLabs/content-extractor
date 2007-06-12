using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Core;

namespace WebExtractor_Testing.Utils
{
  //[TestFixture]
  //public class CacheTests
  //{
  //  [Test]
  //  public void Cache()
  //  {
  //    CachedConverter<double, int> cache = new CachedConverter<double, int>();
  //    cache.Operation = delegate(double value) { return (int)(value * value); };

  //    Assert.AreEqual(0, cache.GetValue(0.0));
  //    Assert.AreEqual(4, cache.GetValue(2.1));
  //  }
  //  [Test]
  //  public void DefaultValue()
  //  {
  //    CachedConverter<double, int> cache = new CachedConverter<double, int>();
  //    cache.DefaultValue = 4;

  //    Assert.AreEqual(cache.DefaultValue, cache.GetValue(0.0));
  //  }

  //  [Test]
  //  public void Null()
  //  {
  //    CachedConverter<string, string> cache = new CachedConverter<string, string>();
  //    cache.Operation = delegate(string val)
  //    {
  //      if (val == null)
  //        return null;
  //      return val.Insert(0, "! ");
  //    };
  //    cache.DefaultValue = null;

  //    Assert.AreEqual(null, cache.GetValue(null));
  //    Assert.AreEqual(null, cache.GetValue(null));
  //  }
  //}

  //[TestFixture]
  //public class ActionCacheTests
  //{
  //  [Test]
  //  public void TestCase()
  //  {
  //    int result = 0;
  //    CachedSynchronization<int> cache = new CachedSynchronization<int>(delegate(int val) { result = val; });

  //    cache.Synchronize(4);
  //    Assert.AreEqual(4, result);
  //    result = 0;
  //    cache.Synchronize(4);
  //    Assert.AreEqual(0, result);

  //    cache.Synchronize(5);
  //    Assert.AreEqual(5, result);

  //  }
  //}
}
