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
