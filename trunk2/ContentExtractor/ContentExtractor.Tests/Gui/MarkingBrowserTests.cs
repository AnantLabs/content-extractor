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
    private Uri pos;
    
    [SetUp]
    public void SetUp()
    {
      state = new State();
      browser = new MarkingBrowser(state);
      File.WriteAllText(filename,
                        "<html><body>Some text here <p> Hello world!</html>");
      pos = Utils.ParseUrl(Path.GetFullPath(filename));
    }

    [TearDown]
    public void TearDown()
    {
      File.Delete(filename);
    }

    private void DoEvents()
    {
      for (int i = 0; i < 3; i++)
        System.Windows.Forms.Application.DoEvents();
    }

    [Test]
    public void BrowseWhenUriChanged()
    {
      state.BrowserUri = pos;
      DoEvents();

      Assert.AreEqual(pos, browser.Browser.Url);
    }

    [Test]
    public void NotExpectedBrowseIsForbiden()
    {
      state.BrowserUri = pos;
      DoEvents();

      browser.Browser.Navigate("http://www.google.com");
      DoEvents();
      Assert.AreEqual(pos, browser.Browser.Url);
    }
  }
}
