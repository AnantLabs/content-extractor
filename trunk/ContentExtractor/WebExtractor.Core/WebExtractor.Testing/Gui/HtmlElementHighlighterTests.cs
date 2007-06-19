using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Core;
using ContentExtractor.Gui;

namespace WebExtractor_Testing.Gui
{
  [TestFixture]
  public class HtmlElementHighlighterTests : BrowserTestBase
  {
    HtmlElementsHighlighter highlighter;
    List<HtmlElementWrapper> results;

    [SetUp]
    public new void SetUp()
    {
      base.SetUp();
      highlighter = new HtmlElementsHighlighter(Browser);
      Init("<html><body><p id='pTag'> Some text </p></body></html>");
      results = new List<HtmlElementWrapper>();
    }

    private List<HtmlElementWrapper> GetResults()
    {
      return results;
    }

    private HtmlElementWrapper Body
    {
      get
      {
        return new HtmlElementWrapper(Browser.Document.Body);
      }
    }

    [Test]
    public void OneElement()
    {
      results.Add(GetElementById("pTag"));

      string transparent = "FILTER: progid:DXImageTransform.Microsoft.Alpha(opacity=50)";
      string back = "BACKGROUND-COLOR: #ff0000";
      string style = string.Format("{0};{1};", transparent, back);
      highlighter.Links[style] = GetResults;
      highlighter.ForceSyncronize();

      HtmlElementWrapper div = Body.SelectSingleNode("div[1]");
      Assert.IsNotNull(div, "Элемент подсветки не был создан");

      Console.WriteLine(div.Element.Style);
      StringAssert.Contains(transparent, div.Element.Style);
      StringAssert.Contains(back, div.Element.Style);
      StringAssert.Contains("POSITION: absolute", div.Element.Style);
    }

    [Test]
    public void CachingDivs()
    {
      highlighter.Links[""] = GetResults;
      results.Add(GetElementById("pTag"));
      highlighter.ForceSyncronize();
      highlighter.ForceSyncronize();

      Assert.IsNotNull(Body.SelectSingleNode("div[1]"), "Div с подсветкой не был создан!");
      Assert.IsNull(Body.SelectSingleNode("div[2]"), "Было создано два Div-а с подсветкой на один элемент");
    }

    [Test]
    public void DeleteDivs()
    {
      highlighter.Links[""] = GetResults;
      results.Add(GetElementById("pTag"));
      highlighter.ForceSyncronize();
      Assert.IsNotNull(Body.SelectSingleNode("div[1]"), "Div с подсветкой не был создан!");

      results.Clear();
      highlighter.ForceSyncronize();
      Assert.IsNull(Body.SelectSingleNode("div[1]"), "Div с подсветкой не был удален!");
    }

    [Test]
    public void NavigatingClearsAll()
    {
      highlighter.Links[""] = GetResults;
      results.Add(GetElementById("pTag"));
      highlighter.ForceSyncronize();

      Browser.Navigate("");
      TestUtils.DoSomeEvents();
      //Init("<html><body>Text</body></html>");
      Assert.IsNull(Body.SelectSingleNode("div[1]"), "При синхронизации не были обнулены div-ы");
    }
  }
}

