using System;
using System.IO;
using NUnit.Framework;
using ContentExtractor.Core;

namespace WebExtractor_Testing.Core
{
	/// <summary>
	/// Tests for WebDocument class
	/// </summary>
	[TestFixture]
	public class WebDocumentTests
	{
	  [Test]
	  public void XmlCode()
	  {
	    string filename = Path.GetFullPath("document.html");
	    if(File.Exists(filename))
	      File.Delete(filename);
	    try
	    {
  	    File.WriteAllText(filename, "<html><body><p>code</p></body></html>");
        WebDocument doc = WebDocument.Load(WebPosition.Parse(filename));
        Assert.AreEqual("<html><body><p>code</p></body></html>", doc.Content);
        Assert.AreEqual("<html><body><p>code</p></body></html>", doc.AsXml.OuterXml);
	    }
	    finally
	    {
	      File.Delete(filename);
	    }
	  }
	}
}
