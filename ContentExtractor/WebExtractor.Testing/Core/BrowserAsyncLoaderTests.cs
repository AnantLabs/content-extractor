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
    [SetUp]
    public void SetUp()
    {
    }
      
    [TearDown]
    public void TearDown()
    {
      if (File.Exists(ApplicationHlp.MapPath(fileName)))
        File.Delete(ApplicationHlp.MapPath(fileName));
      BrowserAsyncLoader.ClearCache();
    }
    private const string fileName = "test.html";

    private string Load(string code)
    {
      File.WriteAllText(ApplicationHlp.MapPath(fileName), code);
      WebPosition position = WebPosition.Parse(TestUrl);
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

    [Test]
    public void _001_SimpleCode()
    {
      string code = @"<html><head><title>adsf</title></head><body>code</body></html>";
      Assert.AreEqual("<HTML><HEAD><TITLE>adsf</TITLE></HEAD>\r\n<BODY>code</BODY></HTML>", Load(code));
    }

    [Test]
    public void _002_NoClosingBody()
    {
      string code = "<html><body><p>text</html>";
      Assert.AreEqual("<HTML><HEAD></HEAD>\r\n<BODY>\r\n<P>text</P></BODY></HTML>", Load(code));
    }

  }
}
