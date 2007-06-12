using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Windows.Forms;
using ContentExtractor.Core;
using System.Xml;
using ContentExtractor.Gui;

namespace WebExtractor_Testing.Utils
{
  [TestFixture]
  public class ParentXmlNodeTests
  {
    XmlDocument doc;
    [SetUp]
    public void SetUp()
    {
      doc = new XmlDocument();
      doc.LoadXml("<Doc><Element1 att='value'>text</Element1></Doc>");
    }

    [Test]
    public void Attribute()
    {
      Assert.AreEqual(doc.SelectSingleNode("/Doc/Element1"), WebExtractorHlp.ParentXmlNode(doc.SelectSingleNode("/Doc/Element1/@att")));
    }
    [Test]
    public void Element()
    {
      Assert.AreEqual(doc.SelectSingleNode("/Doc"), WebExtractorHlp.ParentXmlNode(doc.SelectSingleNode("/Doc/Element1")));
    }
    [Test]
    public void Text()
    {
      Assert.AreEqual(doc.SelectSingleNode("/Doc/Element1"), WebExtractorHlp.ParentXmlNode(doc.SelectSingleNode("/Doc/Element1/text()")));
    }
  }
  [TestFixture]
  public class WebExtractorHlpTests
  {
    [Test]
    public void FilteredXPath()
    {
      string[] xpath = "/a/b/c/d/e/f".Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
      Assert.AreEqual("/a/b/c/d/e/f", WebExtractorHlp.FilteredXPath(xpath, new int[0]));
      Assert.AreEqual("/a/c/d/e/f", WebExtractorHlp.FilteredXPath(xpath, new int[] { 1 }));
      Assert.AreEqual("/a/b/d/e/f", WebExtractorHlp.FilteredXPath(xpath, new int[] { 2 }));
      Assert.AreEqual("/a/b/d/f", WebExtractorHlp.FilteredXPath(xpath, new int[] { 2, 4 }));
      Assert.AreEqual("", WebExtractorHlp.FilteredXPath(xpath, new int[] { 0, 1, 2, 3, 4, 5 }));
    }
    [Test]
    public void NextIndexes()
    {
      int[] indexes = new int[] { 1, 3, 6 };
      Assert.AreEqual(true, WebExtractorHlp.NextIndexes(ref indexes, 7));
      AssertArray(indexes, 1, 4, 5);

      indexes = new int[] { 1, 3, 4 };
      Assert.AreEqual(true, WebExtractorHlp.NextIndexes(ref indexes, 5));
      AssertArray(indexes, 2, 3, 4);
      Assert.AreEqual(false, WebExtractorHlp.NextIndexes(ref indexes, 5));
      AssertArray(indexes, 2, 3, 4);


      indexes = new int[] { 2, 3, 4 };
      Assert.AreEqual(true, WebExtractorHlp.NextIndexes(ref indexes, 6));
      AssertArray(indexes, 2, 3, 5);
      Assert.AreEqual(true, WebExtractorHlp.NextIndexes(ref indexes, 6));
      AssertArray(indexes, 2, 4, 5);
      Assert.AreEqual(true, WebExtractorHlp.NextIndexes(ref indexes, 6));
      AssertArray(indexes, 3, 4, 5);

      indexes = new int[0];
      Assert.AreEqual(false, WebExtractorHlp.NextIndexes(ref indexes, 0));

      indexes = new int[] { 0 };
      Assert.AreEqual(true, WebExtractorHlp.NextIndexes(ref indexes, 4));
      AssertArray(indexes, 1);
      Assert.AreEqual(true, WebExtractorHlp.NextIndexes(ref indexes, 4));
      AssertArray(indexes, 2);
    }

    public string Array2String(int[] array)
    {
      return string.Join(".", Array.ConvertAll<int, string>(array, delegate(int val) { return val.ToString(); }));
    }

    private void AssertArray(int[] actual, params int[] expected)
    {
      Assert.AreEqual(actual.Length, expected.Length);
      for (int i = 0; i < actual.Length; i++)
        Assert.AreEqual(expected[i], actual[i]);
    }

    [Test]
    public void IsCachedXmlActual_NullVsNull()
    {
      Assert.AreEqual(true, WebExtractorHlp.IsCachedXmlActual(null, null));
    }

    [Test]
    public void XmlNodeIndexToParent()
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml("<Doc><test attr='empty'/><test/><test/></Doc>");
      Assert.AreEqual(3, WebExtractorHlp.XmlNodeIndexToParent(doc.SelectSingleNode("/Doc/test[3]")));
      Assert.AreEqual(-1, WebExtractorHlp.XmlNodeIndexToParent(doc.SelectSingleNode("/Doc/test/@attr")));
    }

  }
}