using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Core;
using System.IO;
using MetaTech.Library;

namespace WebExtractor_Testing.Core
{
  [TestFixture]
  public class BrowserAsyncLoaderTests
  {
    [TearDown]
    public void TearDown()
    {
      if (File.Exists(ApplicationHlp.MapPath(fileName)))
        File.Delete(ApplicationHlp.MapPath(fileName));
    }
    private const string fileName = "test.html";

    private string Load(string url)
    {
      WebPosition position = WebPosition.Parse(url);
      if (position != null)
      {
        BrowserAsyncLoader.GetDocumentCode(position);
        while (BrowserAsyncLoader.HasWork)
        {
          System.Threading.Thread.Sleep(100);
        }
      }
      return BrowserAsyncLoader.GetDocumentCode(position);
    }

    private string TestUrl
    {
      get { return ApplicationHlp.MapPath(fileName); }
    }

    private void InitTest(string code)
    {
      File.WriteAllText(ApplicationHlp.MapPath(fileName), code);
    }

    [Test]
    public void _001_SimpleCode()
    {
      string code = @"<html><head><title>adsf</title></head><body>code</body></html>";
      InitTest(code);
      Assert.AreEqual("<HTML><HEAD><TITLE>adsf</TITLE></HEAD><BODY>code</BODY></HTML>", Load(TestUrl));
    }

    [Test]
    public void _002_NoClosingBody()
    {
      string code = "<html><body><p>text</html>";
      InitTest(code);
      Assert.AreEqual("<HTML><HEAD></HEAD><BODY><P>text</P></BODY></html>", Load(TestUrl));
    }

  }
}
