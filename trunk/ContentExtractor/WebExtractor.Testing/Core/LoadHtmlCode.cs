using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using System.Windows.Forms;
using System.Xml;
using ContentExtractor.Core;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using MetaTech.Library;
using ContentExtractor.Gui;

namespace WebExtractor_Testing.Core
{
  [TestFixture]
  public class LoadHtmlCode
  {
    private XmlDocument LoadDocument(string code)
    {
      return XmlHlp.HtmlDocFromNavigable(WebExtractorHlp.LoadHtmlCode(code));
    }
    
    [Test]
    public void BuildTreeTest()
    {
      XmlDocument doc = LoadDocument(Properties.Resources.BuildTreeTest);
      TestUtils.XmlAssertEqual(XmlHlp2.XmlDocFromString(Properties.Resources.BuildTreeTestResult), doc);
    }

    [Test]
    public void TwoRootsDocument()
    {
      XmlDocument doc = LoadDocument(@"<script language='JavaScript'>var a = 0;</script><html><body>text</body></html>");
      Console.WriteLine("__" + XmlHlp2.GetFormatedOuterXml(doc));
      TestUtils.XmlAssertEqual(XmlHlp2.XmlDocFromString("<html><body>text</body></html>"), doc);
    }
    
    [Test]
    public void EmptyDocument()
    {
      XmlDocument doc = XmlHlp.HtmlDocFromNavigable(XmlHlp.HtmlDocFromNavigable(WebExtractorHlp.LoadHtmlCode("")));
      Assert.IsNotNull(doc, "Для пустого документа DocumentTree не должно быть равно null");
      Assert.AreEqual(null, doc.DocumentElement);
    }

    [Test]
    public void XmlHeaderAndEncodingHtml()
    {
      XmlDocument doc = XmlHlp.HtmlDocFromNavigable(WebExtractorHlp.LoadHtmlCode(Properties.Resources.mambo));
      Assert.IsNotNull(doc, "XmlDocument не инициализировался");
      Assert.IsNotNull(doc.DocumentElement, "DocumentElement в XmlDocument не инициализировался");
      Assert.AreEqual("html", doc.DocumentElement.Name);
    }

    [Test]
    public void NotClosedDocument()
    {
      try
      {
        XmlDocument doc = XmlHlp.HtmlDocFromNavigable(WebExtractorHlp.LoadHtmlCode("<html><body><div class='asd"));
      }
      catch
      {
        Assert.Fail("PageTree не инициализируется при плохих данных");
      }
    }
    [Test]
    public void TestFromInternet1()
    {
      XmlDocument doc = XmlHlp.HtmlDocFromNavigable(WebExtractorHlp.LoadHtmlCode(Properties.Resources.LoadHtmlTest1));
      //Assert.IsNotNull(doc, "Не удалось разобрать документ");
      Assert.IsNotNull(doc.DocumentElement, "Не удалось разобрать документ");
    }

    [Ignore("The html parser should be recoded to fit this test")]
    [Test]
    public void NotClosedTags()
    {
      XmlDocument doc = XmlHlp.HtmlDocFromNavigable(WebExtractorHlp.LoadHtmlCode(Properties.Resources.NotClosedTags));
      Console.WriteLine(XmlHlp2.GetFormatedOuterXml(doc));
      Assert.IsNotNull(doc.DocumentElement, "Документ совсем не распознался");
      Assert.IsNotNull(doc.SelectSingleNode("/html[1]/body[1]/table[1]/tr[6]"), "Нужная вершина в документ не добавлена (1)");
      Assert.IsNotNull(doc.SelectSingleNode("/html[1]/body[1]/table[1]/tr[6]/td[3]/table[1]/tr[1]/td[4]/index[1]/table[1]/tr[1]/td[1]/ul[1]/li[1]"), "Нужная вершина в документ не добавлена (2)");
    }
  }

}
