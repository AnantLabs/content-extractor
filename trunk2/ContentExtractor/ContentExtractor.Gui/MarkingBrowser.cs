//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 06.07.2007
// Time: 19:22
//

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ContentExtractor.Core;

namespace ContentExtractor.Gui
{
	/// <summary>
	/// Description of MarkingBrowser.
	/// </summary>
	public partial class MarkingBrowser : UserControl
	{
	  // HACK: Need to make Sharp Develop designer work properly
//	  internal MarkingBrowser()
//	  {
//	    
//	  }
	  
		public MarkingBrowser(State state)
		{
			InitializeComponent();
			this.state = state;
			
			state.BrowserUriChanged += new EventHandler(BrowserUriChanged);
		}
		
		private State state;
		
		public ExtendedWebBrowser Browser
		{
		  get { return webBrowser1; }
		}
		
		private void BrowserUriChanged(object sender, EventArgs e)
		{
		  Browser.Navigate(state.BrowserUri);
		}
	}
}
