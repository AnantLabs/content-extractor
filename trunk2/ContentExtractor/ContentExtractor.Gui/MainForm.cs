//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 06.07.2007
// Time: 19:06
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ContentExtractor.Gui
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
		private ContentExtractor.Gui.State state;
		private MarkingBrowser browser;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		  
			state = new State();
			browser = new MarkingBrowser(state);
			browser.Dock = DockStyle.Fill;
			rightSplitContainer.Panel2.Controls.Add(browser);
			
			urlsListBox1.SetState(state);
		}
	}
}
