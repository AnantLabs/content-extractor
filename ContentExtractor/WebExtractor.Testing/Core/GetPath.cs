using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Xml;
using MetaTech.Library;
using ContentExtractor.Core;

namespace WebExtractor_Testing.Core
{
  [TestFixture]
  public class GetPath
  {
    [Test]
    public void SimpleElement()
    {
      XmlDocument doc = XmlHlp2.XmlDocFromString("<html><body></body></html>");
      XmlNode test = doc.SelectSingleNode("/html/body");
      Assert.AreEqual("/html[1]/body[1]", XmlHlp.GetPath(test));
    }
    [Test]
    public void SimpleText()
    {
      XmlDocument doc = XmlHlp2.XmlDocFromString("<html><body>123</body></html>");
      XmlNode test = doc.SelectSingleNode("/html/body/text()");
      Assert.AreEqual("/html[1]/body[1]/text()[1]", XmlHlp.GetPath(test));
    }
    [Test]
    public void SimpleAttribute()
    {
      XmlDocument doc = XmlHlp2.XmlDocFromString("<html><body class='asdf'>123</body></html>");
      XmlNode test = doc.SelectSingleNode("/html/body/@class");
      Assert.AreEqual("/html[1]/body[1]/@class", XmlHlp.GetPath(test));
    }
    [Test]
    public void NamespaceInStart()
    {
      XmlDocument doc = XmlHlp2.XmlDocFromString("<html xmlns='http://www.go.com'><body></body></html>");
      XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
      manager.AddNamespace("x", "http://www.go.com");
      XmlNode test = doc.SelectSingleNode("/x:html/x:body",manager);
      Assert.IsNotNull(test, "Test node is null");
      Assert.AreEqual("/{http://www.go.com}:html[1]/body[1]", XmlHlp.GetPath(test));
    }
    [Test]
    public void NamespaceInEnd()
    {
      XmlDocument doc = XmlHlp2.XmlDocFromString("<html ><body xmlns='http://www.go.com'></body></html>");
      XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
      manager.AddNamespace("x", "http://www.go.com");
      XmlNode test = doc.SelectSingleNode("/html/x:body", manager);
      Assert.IsNotNull(test, "Test node is null");
      Assert.AreEqual("/html[1]/{http://www.go.com}:body[1]", XmlHlp.GetPath(test));
    }
    [Test]
    public void NamespaceInTextWrapper()
    {
      XmlDocument doc = XmlHlp2.XmlDocFromString("<html ><body xmlns='http://www.go.com'>some text</body></html>");
      XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
      manager.AddNamespace("x", "http://www.go.com");
      XmlNode test = doc.SelectSingleNode("/html/x:body/text()", manager);
      Assert.IsNotNull(test, "Test node is null");
      Assert.AreEqual("/html[1]/{http://www.go.com}:body[1]/text()[1]", XmlHlp.GetPath(test));
    }
    [Test]
    public void NamespaceInAttributeOwner()
    {
      XmlDocument doc = XmlHlp2.XmlDocFromString("<html xmlns='http://www.go.com'><body class='none'>some text</body></html>");
      XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
      manager.AddNamespace("x", "http://www.go.com");
      XmlNode test = doc.SelectSingleNode("/x:html/x:body/@class", manager);
      Assert.IsNotNull(test, "Test node is null");
      Assert.AreEqual("/{http://www.go.com}:html[1]/body[1]/{}:@class", XmlHlp.GetPath(test));
    }
  }
}
