using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Core;
using ContentExtractor.Gui;
using System.Windows.Forms;
using System.Xml;
using MetaTech.Library;

namespace WebExtractor_Testing.Core
{
  public class MapTestsBase
  {
    [SetUp]
    public void SetUp()
    {
      browseWrapper = new WebBrowserWrapper();
      Map = new BrowserHtmlMap();
    }

    [TearDown]
    public void TearDown()
    {
      string stat = XmlSerialization2.SaveToText(SoftTech.Diagnostics.MultiPeriodTimeMeasurer.Default.GetAllStatistic());
      Console.WriteLine(stat);
    }

    WebBrowserWrapper browseWrapper;
    protected BrowserHtmlMap Map;
    protected WebBrowser Browser
    {
      get { return browseWrapper.Browser; }
    }

    protected void Init(string htmlCode)
    {
      Browser.DocumentText = htmlCode;
      do
      {
        TestUtils.DoSomeEvents();
      } while (Browser.IsBusy);
      //Map.Init(Browser, WebExtractorHlp.LoadHtmlCode(WebExtractorHlp.DocumentText(Browser)));
    }

    protected HtmlElementWrapper GetElementById(string id)
    {
      return new HtmlElementWrapper(Browser.Document.GetElementById(id));
    }

    protected XmlNode GetXmlNode(string xpath)
    {
      return XmlHlp.SelectSingleNode(XmlHlp.HtmlDocFromNavigable(WebExtractorHlp.LoadHtmlCode(
        WebExtractorHlp.DocumentText(Browser))), xpath);
    }
  }

  [TestFixture]
  public class HtmlElement2XmlElement : MapTestsBase
  {
    protected string MappedXmlNodePath(HtmlElementWrapper testElement)
    {
      XmlDocument doc = XmlHlp.HtmlDocFromNavigable(WebExtractorHlp.LoadHtmlCode(WebExtractorHlp.DocumentText(Browser)));
      return Map.GetXmlNode(doc, testElement);
    }

    [Test]
    public void HtmlElement2XmlNode()
    {
      Init("<html><body><table><tr><td>1</td><td id=\"toTest\">2</td></table></body></html>");
      HtmlElementWrapper testElement = GetElementById("toTest");

      Assert.AreEqual("/html[1]/body[1]/table[1]/tr[1]/td[2]", MappedXmlNodePath(testElement));
    }

    [Test]
    public void HtmlElement2XmlNode_Deeply_Nested()
    {
      Init(@"<html><body><table>
                                      <tr><td>1</td>
                                      <td> <table><tr><td>
                                               <table>
                                                  <tr><td></td><td></td></tr>
                                                  <tr><td></td><td id=""toTest"">text</td></tr>
                                                  <tr><td></td><td></td></tr>
                                               </table>
                                           </table></td>
                                    </table></body></html>");
      HtmlElementWrapper testElement = GetElementById("toTest");

      Assert.AreEqual("/html[1]/body[1]/table[1]/tr[1]/td[2]/table[1]/tr[1]/td[1]/table[1]/tr[2]/td[2]",
                      MappedXmlNodePath(testElement));
    }

  }

  [TestFixture]
  public class XmlNode2HtmlElement : MapTestsBase
  {
    protected HtmlElementWrapper MappedHtmlElement(XmlNode node)
    {
      return Map.GetHtmlElement(Browser, node);
    }

    [Test]
    public void Simple()
    {
      Init(@"<html><head><title>Title</title></head> 
                                    <body>
                                      <table width=""300px"">
                                         <tr height=""300px""> <td id=""toTest""> Here are some text</td> </tr>
                                      </table>
                                    </body> </html>");
      HtmlElementWrapper testElement = GetElementById("toTest");

      XmlNode node = GetXmlNode("html/body/table/tr/td");
      Assert.AreEqual(testElement, MappedHtmlElement(node));
    }

    [Test]
    public void Attribute()
    {
      Init(@"<html><head><title>Title</title></head> 
                                    <body>
                                      <table width=""300px"">
                                         <tr height=""300px""> <td id=""toTest""> Here are some text</td> </tr>
                                      </table>
                                    </body> </html>");
      HtmlElementWrapper testElement = GetElementById("toTest");

      XmlNode node = GetXmlNode("html/body/table/tr/td/@id");
      Assert.AreEqual(testElement, MappedHtmlElement(node));
    }

    [Test]
    public void Yandex1()
    {
      Init(Properties.Resources.XmlNode2HtmlNode_yandex);
      HtmlElementWrapper testElement = GetElementById("toTest");

      XmlNode node = GetXmlNode("/html/body[1]/div[2]/div[1]/table[1]/tr[1]/td[4]/noindex[1]/ol[1]");
      HtmlElementWrapper actual = MappedHtmlElement(node);
      Console.WriteLine(actual.Path);
      Assert.AreEqual(testElement, actual);
    }

    [Test]
    public void Yandex2()
    {
      Init(Properties.Resources.XmlNode2HtmlNode_yandex);
      HtmlElementWrapper testElement = GetElementById("trToTest");

      XmlNode node = GetXmlNode("/html/body[1]/div[2]/div[1]/table[1]/tr[1]");
      HtmlElementWrapper actual = MappedHtmlElement(node);
      Console.WriteLine(actual.Path);
      Assert.AreEqual(testElement, actual);
    }

    [Test]
    public void Null2Null()
    {
      Assert.AreEqual(null, MappedHtmlElement(null));
    }

    [Test]
    public void Yandex3()
    {
      Init(Properties.Resources.XmlNode2HtmlElementTest3);
      HtmlElementWrapper testElement = GetElementById("toTest");

      HtmlElementWrapper actual = MappedHtmlElement(GetXmlNode("/html/body/table/tr[3]"));
      Assert.AreEqual(testElement, actual);
    }

    //    [Test]
    //    public void ClickOnHtmlChangesSelectedNode()
    //    {
    //      Init(@"<html><head><title>Title</title></head> 
    //                                    <body>
    //                                      <table width=""300px"">
    //                                         <tr height=""300px""> <td id=""elementToTest""> Here are some text</td> </tr>
    //                                      </table>
    //                                    </body> </html>");
    //      HtmlElementWrapper tdElem = GetElementById("elementToTest");

    //      XmlNode tdNode = PageDoc.SelectSingleNode("/html/body/table/tr/td");
    //      tdElem.Click();

    //      TestUtils.XmlAssertEqual(tdNode, GetXmlNode(SelectedNode));
    //    }

  }
}
