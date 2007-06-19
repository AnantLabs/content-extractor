using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Core;

namespace WebExtractor_Testing.Core
{
  [TestFixture]
  public class XPathPartTests
  {
    [Test]
    public void Element()
    {
      XPathPart part = new XPathPart("element");
      Assert.AreEqual(XPathPartType.Element, part.Type);
      Assert.AreEqual("element", part.LocalName);
      Assert.AreEqual(null, part.Index);
      Assert.AreEqual(null, part.Namespace);
    }
    [Test]
    public void NamespaceElement()
    {
      XPathPart part = new XPathPart("{asd}:element");
      Assert.AreEqual(XPathPartType.Element, part.Type);
      Assert.AreEqual("element", part.LocalName);
      Assert.AreEqual(null, part.Index);
      Assert.AreEqual("asd", part.Namespace);
    }
    [Test]
    public void Index()
    {
      XPathPart part = new XPathPart("element[2]");
      Assert.AreEqual(XPathPartType.Element, part.Type);
      Assert.AreEqual("element", part.LocalName);
      Assert.AreEqual(2, part.Index);
      Assert.AreEqual(null, part.Namespace);
    }
    [Test]
    public void AttributeWithNamespace()
    {
      XPathPart part = new XPathPart("{http://go.ru}:@attr");
      Assert.AreEqual(XPathPartType.Attribute, part.Type);
      Assert.AreEqual("attr", part.LocalName);
      Assert.AreEqual(null, part.Index);
      Assert.AreEqual("http://go.ru", part.Namespace);
    }
    [Test]
    public void Text()
    {
      XPathPart part = new XPathPart("text()[5]");
      Assert.AreEqual(XPathPartType.Text, part.Type);
      Assert.AreEqual("text()", part.LocalName);
      Assert.AreEqual(5, part.Index);
      Assert.AreEqual(null, part.Namespace);
    }
    [Test]
    public void AllXPathParsing()
    {
      XPathInfo info = XPathInfo.Parse("/html[1]/body[1]");
      XPathPart[] parts = info.XParts;
      Assert.AreEqual(3, parts.Length);
      Assert.AreEqual(null, parts[0]);
      Assert.AreEqual("html[1]", parts[1].FullName);
      Assert.AreEqual("body[1]", parts[2].FullName);
    }
    [Test]
    public void SimpleXPath()
    {
      XPathInfo info = XPathInfo.Parse("{}:@adf");
      XPathPart[] parts = info.XParts;
      Assert.AreEqual(1, parts.Length);
      Assert.AreEqual("adf", parts[0].LocalName);
      Assert.AreEqual(string.Empty, parts[0].Namespace);
      Assert.AreEqual(XPathPartType.Attribute, parts[0].Type);
    }
  }
}
