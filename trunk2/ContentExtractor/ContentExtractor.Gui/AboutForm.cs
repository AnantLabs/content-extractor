using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ContentExtractor.Gui
{
  public partial class AboutForm : Form
  {
    public AboutForm()
    {
      InitializeComponent();
    }

    private void AboutForm_Load(object sender, EventArgs e)
    {
      this.Text = string.Format(Properties.Resources.AboutCaption,
                                Application.ProductName,
                                "1.0 Beta");
    }

    private void linkLabel1_Click(object sender, EventArgs e)
    {
      System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
      info.UseShellExecute = true;
      info.CreateNoWindow = true;
      info.Verb = "open";
      info.FileName = Properties.Resources.HomePageUrl;
      System.Diagnostics.Process.Start(info);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.Close();
    }

  }
}