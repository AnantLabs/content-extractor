//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 06.07.2007
// Time: 19:15
//

using System;
using ContentExtractor.Core;
using System.Xml.Serialization;

namespace ContentExtractor.Gui
{
	/// <summary>
	/// Description of State.
	/// </summary>
	public class State
	{
		public State()
		{
		}
		
		[XmlIgnore]
		public Uri BrowserUri
		{
		  get
		  {
		    return browserUri_;
		  }
		  set
		  {
		    if(!Uri.Equals(value, browserUri_))
		    {
		      browserUri_ = value;
		      if(BrowserUriChanged != null)
		        BrowserUriChanged(this, EventArgs.Empty);
		    }
		  }
		}

		private Uri browserUri_ = null;
		
		#region XmlHelpers
		public string XmlBrowserUri
		{
		  get { return BrowserUri.AbsoluteUri; }
		  set { BrowserUri = Utils.ParseUrl(value); }
		}
		#endregion
		
		public ScrapingProject Project = new ScrapingProject();
		
		public bool IsParseMode = false;
		
		public event EventHandler BrowserUriChanged;
		
	}
}
