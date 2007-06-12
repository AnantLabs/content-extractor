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
  public class Select
  {
    [Test]
    public void Classical()
    {
      XmlDocument doc = XmlHlp2.XmlDocFromString("<html><body class='none'>text</body></html>");
      XmlNode element = doc.SelectSingleNode("/html/body");
      XmlNode attribute = doc.SelectSingleNode("/html/body/@class");
      XmlNode text = doc.SelectSingleNode("/html/body/text()");

      Assert.AreEqual(element, XmlHlp.SelectSingleNode(doc, XmlHlp.GetPath(element)));
      Assert.AreEqual(attribute, XmlHlp.SelectSingleNode(doc, XmlHlp.GetPath(attribute)));
      Assert.AreEqual(text, XmlHlp.SelectSingleNode(doc, XmlHlp.GetPath(text)));
    }

    [Test]
    public void Namespace()
    {
      XmlDocument doc = XmlHlp2.XmlDocFromString("<html xmlns='http://www.go.com'><body class='none'>text</body></html>");
      XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
      manager.AddNamespace("x", "http://www.go.com");
      XmlNode element = doc.SelectSingleNode("/x:html/x:body",manager);
      XmlNode attribute = doc.SelectSingleNode("/x:html/x:body/@class", manager);
      XmlNode text = doc.SelectSingleNode("/x:html/x:body/text()", manager);
      Assert.IsNotNull(element, "Test element node is null");
      Assert.IsNotNull(attribute, "Test attribute node is null");
      Assert.IsNotNull(text, "Test text node is null");

      Assert.AreEqual(element, XmlHlp.SelectSingleNode(doc, XmlHlp.GetPath(element)),"Element selection failed");
      Assert.AreEqual(attribute, XmlHlp.SelectSingleNode(doc, XmlHlp.GetPath(attribute)), "Attribute selection failed");
      Assert.AreEqual(text, XmlHlp.SelectSingleNode(doc, XmlHlp.GetPath(text)), "Text selection failed");

    }
  }
}
