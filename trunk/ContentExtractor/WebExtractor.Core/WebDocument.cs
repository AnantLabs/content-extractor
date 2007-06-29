//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 26.06.2007
// Time: 18:58
//

using System;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace ContentExtractor.Core
{
	/// <summary>
	/// Description of WebDocument.
	/// </summary>
	public class WebDocument
	{
	  public static WebDocument Load(WebPosition position)
	  {
	    string content = AsyncLoader.Instance.Load(position);
	    using( ExtendedWebBrowser browser = new ExtendedWebBrowser())
	    {
  	    browser.DocumentText = content;
  	    browser.IsWebBrowserContextMenuEnabled = false;
  	    // WARNING!! this can cause problems when certificate approval needed!
  	    browser.ScriptErrorsSuppressed = true;
        System.Windows.Forms.Application.DoEvents();
  	    while (browser.IsBusy)
  	      System.Windows.Forms.Application.DoEvents();
  	    return new WebDocument(DOMTreeToXml(browser.Document));
	    }
	  }
	  
	  private static XmlDocument DOMTreeToXml(HtmlDocument htmlDoc)
	  {
  	  XmlDocument result = new XmlDocument();
	    if(htmlDoc != null && 
	       htmlDoc.Body != null &&
	       htmlDoc.Body.Parent != null)
	    {
  	    HtmlElement topHtml = htmlDoc.Body.Parent;
  	    using (StringReader sReader = new StringReader(topHtml.OuterHtml))
  	    {
  	      using (StringWriter errorLog = new StringWriter())
  	      {
  	        Sgml.SgmlReader reader = new Sgml.SgmlReader();
  	        reader.ErrorLog = errorLog;
  	        reader.InputStream = sReader;
  	        using (StringReader dtdReader = new StringReader(Properties.Resources.WeakHtml))
  	          reader.Dtd = Sgml.SgmlDtd.Parse(null, "HTML", null, dtdReader, null, null, reader.NameTable);

  	        result.Load(reader);
  	        errorLog.Flush();
  	        Console.WriteLine(errorLog.ToString());
  	      }
  	    }
	    }
  	  return result;
	  }
	  
	  private WebDocument(XmlDocument doc)
	  {
	    xml_ = doc;
	  }
	  
	  public XmlDocument AsXml
	  {
	    get
	    {
  	    return xml_;
	    }
	  }
	  private XmlDocument xml_ = null;
	  
	  public string Content
	  {
	    get
	    {
	      return xml_.OuterXml;  
	    }
	  }
	}
}
