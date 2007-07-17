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
using System.IO;

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
    private readonly string statePath = Core.Utils.ApplicationMapPath("content-extractor.state"); 
    //private MarkingBrowser browser;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

      state = new State();
      if (File.Exists(statePath))
      {
        try
        {
          state = Core.XmlUtils.Deserialize<State>(statePath);
        }
        catch (Exception exc)
        {
          //TODO: Should log to WARNING
          Console.WriteLine("Cannot load saved state:\r\n{0}", exc);
        }
      }
      browser.SetState(state);
      //rightSplitContainer.Panel2.Controls.Add(browser);
			
			urlsListBox1.SetState(state);
		}

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      Core.XmlUtils.Serialize(statePath, state);
    }
	}
}
