using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Xml;
using MetaTech.Library;
using ContentExtractor.Core;

namespace WebExtractor_Testing.Functions
{
  [TestFixture]
  public class BestContentNodeTests
  {
    private void AssertBestNode(string doc, string xpath)
    {
      Assert.AreEqual(XmlHlp.GetPath(ContentExtractor.Core.Functions.BestContentNode(XmlHlp2.XmlDocFromString(doc))), xpath);
    }
    [Test]
    public void _001_OneTagDoc()
    {
      AssertBestNode("<html>sdaf</html>", "/html[1]");
    }
    [Test]
    public void _002_TwoTagsDoc()
    {
      AssertBestNode("<html><head>sdf</head><body>sdaf</body></html>", "/html[1]");
    }
    [Test]
    public void _003_InnerTagWithText()
    {
      AssertBestNode("<html><body>sdaf</body></html>", "/html[1]/body[1]");
    }
    [Test]
    public void _004_BestInDeep()
    {
      AssertBestNode("<html><head>sdf</head><body><p>sda asdfasdf</p><p>sdfsagag asdf asd</p></body></html>",
        "/html[1]/body[1]");
    }
    [Test]
    public void _005_3dNews()
    {
      AssertBestNode(Properties.Resources.Best3DNews,
        "/html[1]/body[1]/table[2]/tr[3]/td[1]/table[1]/tr[1]/td[3]/table[1]/tr[1]/td[1]");
    }
    [Ignore]
    [Test]
    public void _006_MyJane()
    {
      AssertBestNode(Properties.Resources.BestMyJane,
        "/html[1]/body[1]/table[2]/tr[3]/td[1]/table[1]/tr[1]/td[3]/table[1]/tr[1]/td[1]");
    }
  }
}
