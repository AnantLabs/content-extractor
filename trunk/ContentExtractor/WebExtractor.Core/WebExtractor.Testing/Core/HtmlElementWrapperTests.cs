using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Core;
using System.Drawing;
using ContentExtractor.Gui;
using System.Windows.Forms;
using System.Xml;

namespace WebExtractor_Testing.Core
{

  [TestFixture]
  public class HtmlElementWrapperTest : BrowserTestBase
  {
    [Test]
    public void ParentWithNoIndex()
    {
      Init(@"<html><body><table><tr><td><div>some</div><noindex id=""noindex"">
              <ol id=""toTest""><li>item 1<li>item 2</ol></noindex></td></tr></table></body></html>");

      HtmlElementWrapper ol = GetElementById("toTest");
      HtmlElementWrapper noindex = GetElementById("noindex");

      Assert.AreEqual(noindex, ol.Parent);
    }

    [Test]
    public void Path()
    {
      Init("<html><body><table><tr><td>1</td><td id=\"toTest\">2</td></table></body></html>");
      HtmlElementWrapper testElement = GetElementById("toTest");
      Assert.IsNotNull(testElement);
      Assert.AreEqual("/HTML[1]/BODY[1]/TABLE[1]/TBODY[1]/TR[1]/TD[2]", testElement.Path);
    }

    [Test]
    public void NullHtmlElementPath()
    {
      Assert.AreEqual("", new HtmlElementWrapper(null).Path);
    }

    [Test]
    public void PathWithNoIndex()
    {
      Init("<html><body><table><tr><td><div>some</div><noindex><ol><li id=\"toTest\">item 1<li>item 2</ol></noindex></td></tr></table></body></html>");
      HtmlElementWrapper testElement = GetElementById("toTest");
      Assert.AreEqual("/HTML[1]/BODY[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/NOINDEX[1]/OL[1]/LI[1]", testElement.Path);
    }

    [Test]
    public void SelectSingleNode()
    {
      Init("<html><body><table><tr><td>1</td><td id=\"toTest\">2</td></table></body></html>");

      HtmlElementWrapper body = new HtmlElementWrapper(Browser.Document.Body);

      Assert.AreEqual(GetElementById("toTest"), body.SelectSingleNode("table[1]/tbody[1]/tr[1]/td[2]"));
      Assert.AreEqual(null, body.SelectSingleNode("table[1]/tbody[1]/tr[2]"));
    }

    [Test]
    public void CreateChild()
    {
      Init("<html><body id='body'></body></html>");
      HtmlElementWrapper body = GetElementById("body");
      body.CreateChild("p");
      Assert.IsNotNull(body.SelectSingleNode("p[1]"), "Не удалось создать дочерний элемент");
    }

    [Test]
    public void AbsoluteRectangleOfAbsolutePositionedElement()
    {
      string style = "position:absolute;left:{0}px;top:{1}px;width:{2}px;height:{3}px;";
      Rectangle rect = new Rectangle(20, 10, 50, 70);
      style = string.Format(style, rect.Left, rect.Top, rect.Width, rect.Height);
      Init(string.Format("<html><body><div id='div' style='{0}'>Text</div></body></html>", style));
      Assert.AreEqual(rect, GetElementById("div").AbsoluteRectangle);
    }

  }


}

