//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 26.06.2007
// Time: 18:58
//

using System;
using System.Xml;

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
	    return new WebDocument(content);
	  }
	  
	  private WebDocument(string sourceContent)
	  {
	    content_ = SyncLoad(sourceContent);
	  }
	  
	  private static string SyncLoad(string source)
	  {
	    return source;
	  }
	  
	  public XmlDocument AsXml
	  {
	    get
	    {
	      if(xml_ == null)
	      {
	        xml_ = XmlHlp.LoadXml(content_);
	      }
  	    return xml_;
	    }
	  }
	  private XmlDocument xml_ = null;
	  
	  public string Content
	  {
	    get
	    {
	      return content_;  
	    }
	  }
	  private string content_;
	}
}
