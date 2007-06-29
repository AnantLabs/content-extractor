//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 29.06.2007
// Time: 18:34
//

using System;

namespace ContentExtractor.Core
{
	/// <summary>
	/// Persist information about position of a document.
	/// Usually contains only URI.
	/// In the future, will store POSTDATA and other 
	/// </summary>
	public class WebPosition
	{
    public WebPosition(Uri url)
    {
      this.Url = url;
    }

    public static WebPosition Parse(string address)
    {
      Uri uri = null;
      if (!Uri.TryCreate(address, UriKind.Absolute, out uri))
        Uri.TryCreate(Uri.UriSchemeHttp + "://" + address, 
                      UriKind.Absolute, out uri);

      if (uri != null)
        return new WebPosition(uri);
      else
        return null;
    }
    
    public Uri Url = null;
	}
}
