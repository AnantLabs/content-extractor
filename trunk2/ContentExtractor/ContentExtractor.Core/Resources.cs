//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 29.06.2007
// Time: 19:00
//

using System;
using System.IO;

namespace ContentExtractor.Core
{
	/// <summary>
	/// Description of Resources.
	/// </summary>
	public static class Resources
	{
	  public static string WeakHtmlDTD
	  {
	    get
	    {
	      using(Stream stream = typeof(Data._Dummy).Assembly.GetManifestResourceStream(typeof(Data._Dummy), "weak.dtd"))
        using(StreamReader reader = new StreamReader(stream))
	        return reader.ReadToEnd();
	    }
	  }
	}
}
