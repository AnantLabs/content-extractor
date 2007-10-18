//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 06.07.2007
// Time: 19:29
//

using NUnit.Framework;
using System;
using ContentExtractor.Core;
using ContentExtractor.Gui;
using System.IO;

namespace ContentExtractorTests.Gui
{
  [TestFixture]
  public class MarkingBrowserTests
  {
    private const string filename = "test.html";
    private State state;
    private MarkingBrowser browser;
    private readonly Uri pos = Utils.ParseUrl(Path.GetFullPath(filename));
    
    [SetUp]
    public void SetUp()
    {
      state = new State();
      browser = new MarkingBrowser();
      browser.SetState(state);
      File.WriteAllText(filename,
                        "<html><body>Some text here <p> Hello world!</html>");
    }

    [TearDown]
    public void TearDown()
    {
      File.Delete(filename);
    }

    [Test]
    public void BrowseWhenUriChangedToFile()
    {
      state.BrowserPosition = new DocPosition(pos);
      browser.ForceSynchronize();
      TestUtils.DoEvents(2);

      Assert.AreEqual(pos, browser.Browser.Url);
    }

    [Test]
    public void NotExpectedBrowseIsForbiden()
    {
      state.BrowserPosition = new DocPosition(pos);
      browser.ForceSynchronize();
      TestUtils.DoEvents(2);

      browser.Browser.Navigate("http://www.google.com");
      browser.ForceSynchronize();
      TestUtils.DoEvents(2);
      Assert.AreEqual(pos, browser.Browser.Url);
    }
  }
}
