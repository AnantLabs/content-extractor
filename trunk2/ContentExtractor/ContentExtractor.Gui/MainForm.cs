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
using ContentExtractor.Core;
using log4net;

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
      try
      {
        Application.Run(new MainForm());
      }
      catch (Exception exc)
      {
        Logger.Fatal("Unhandled exception occuried.", exc);
      }
		}

    private static readonly ILog Logger = LogManager.GetLogger(typeof(MainForm));
		
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
          Logger.Warn("Cannot load saved state", exc);
        }
      }
      browser.SetState(state);
			urlsListBox1.SetState(state);
      resultsView1.SetState(state);
      docTreeView.SetState(state);
		}

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      Core.XmlUtils.Serialize(statePath, state);
    }

    private void saveToolStripButton_Click(object sender, EventArgs e)
    {
      if (saveFileDialog2.ShowDialog() == DialogResult.OK)
      {
        ScrapingProject.SaveProject(saveFileDialog2.FileName, state.Project);
      }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();  
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      new AboutForm().ShowDialog();
    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
      if (openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        this.state.Project = ScrapingProject.Load(openFileDialog1.FileName);
      }
    }
	}
}
