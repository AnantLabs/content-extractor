//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 26.06.2007
// Time: 19:42
//

using System;
using System.IO;
using NUnit.Framework;

using ContentExtractor.Core;
using System.Windows.Forms;

namespace ContentExtractorTests.Core
{
  [TestFixture]
  public class UtilsTests
  {
    [Test]
    public void ParseFileName()
    {
      Uri position = Utils.ParseUrl(Path.GetFullPath("file.html"));
      Assert.IsNotNull(position, "Couldn't parse local file as Uri");
    }

    [Test]
    public void ParseDnsName()
    {
      Uri position = Utils.ParseUrl("www.google.com");
      Assert.IsNotNull(position, "Couldn't parse local file as Uri");
      Assert.AreEqual("http://www.google.com/", position.ToString());
    }
  }

  [TestFixture]
  public class HtmlElementXPathTests
  {
    private ExtendedWebBrowser browser;
    [SetUp]
    public void SetUp()
    {
      browser = new ExtendedWebBrowser();
    }

    private void LoadContent(string content)
    {
      browser.DocumentText = content;
      do
      {
        TestUtils.DoEvents(1);
      } while (browser.IsBusy);
    }

    /// <summary>
    /// There were an error with Internet Explorer html parser.
    /// The XPath collected by using HtmlElement.Parent and HtmlElement.Children
    /// properties was "/HTML[1]/BASE[0]/BODY[1]/P[1]". The parent of BODY is BASE,
    /// the parent of BASE is HTML, but HTML has only one child - HEAD.
    /// </summary>
    [Test]
    public void T001_BrokenParentChild()
    {
      LoadContent(@"<html><head>
<title>Open Directory - Arts: Animation: Anime: Multimedia</title>
<base target=""_top"">
</head>
<body> <p id='test_elem'> </body></html>");
      HtmlElement element = browser.Document.GetElementById("test_elem");
      // In fact the XPath must be equal to "/HTML[1]/BODY[1]/P[1]" but IE's parser
      // has an error so it considers BODY tag as a child of BASE.
      Assert.AreEqual("/HTML[1]/HEAD[1]/BASE[1]/BODY[1]/P[1]", Utils.HtmlElementXPath(element));
    }
  }

}
