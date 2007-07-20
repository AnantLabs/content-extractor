using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Xml;
using ContentExtractor.Core;

namespace ContentExtractorTests.Core
{
  [TestFixture]
  public class XmlUtilsTests
  {
    [Test]
    public void SimpleElement()
    {
      XmlDocument doc = XmlUtils.LoadXml("<html><body></body></html>");
      XmlNode test = doc.SelectSingleNode("/html/body");
      Assert.AreEqual("/html[1]/body[1]", XmlUtils.GetXPath(test));
    }
    [Test]
    public void SimpleText()
    {
      XmlDocument doc = XmlUtils.LoadXml("<html><body>123</body></html>");
      XmlNode test = doc.SelectSingleNode("/html/body/text()");
      Assert.AreEqual("/html[1]/body[1]/text()[1]", XmlUtils.GetXPath(test));
    }
    [Test]
    public void SimpleAttribute()
    {
      XmlDocument doc = XmlUtils.LoadXml("<html><body class='asdf'>123</body></html>");
      XmlNode test = doc.SelectSingleNode("/html/body/@class");
      Assert.AreEqual("/html[1]/body[1]/@class", XmlUtils.GetXPath(test));
    }
  }
}
