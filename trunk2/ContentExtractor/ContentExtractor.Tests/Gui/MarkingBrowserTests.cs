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
	  private State state;
	  private MarkingBrowser browser;
	  [SetUp]
	  public void SetUp()
	  {
	    state = new State();
	    browser = new MarkingBrowser(state);
	  }
	  
	  private void DoEvents()
	  {
	    for(int i = 0; i < 3 ; i++)
	      System.Windows.Forms.Application.DoEvents();
	  }
	  
		[Test]
		public void TestMethod()
		{
		  string filename = "test.html";
		  try
		  {
		    File.WriteAllText(filename,
		                      "<html><body>Some text here <p> Hello world!</html>");
		    Uri pos = Utils.ParseUrl(Path.GetFullPath(filename));
		    
		    state.BrowserUri = pos;
		    DoEvents();
		    
		    Assert.AreEqual(pos, browser.Browser.Url);
		  }
		  finally
		  {
		    File.Delete(filename);
		  }
		}
	}
}
