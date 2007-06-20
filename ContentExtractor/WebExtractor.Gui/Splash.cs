using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ContentExtractor.Core;

namespace ContentExtractor.Gui
{
  public partial class Splash : Form
  {
    public Splash(Form mainForm, double span)
      : this(mainForm)
    {
      spanInSeconds = span;
    }

    public Splash(Form mainForm)
    {
      InitializeComponent();
      this.mainForm = mainForm;
      this.mainForm.Load += mainForm_Load;
    }

    void mainForm_Load(object sender, EventArgs e)
    {
      loaded = true;
      startTime = DateTime.UtcNow;
      this.mainForm.Load -= mainForm_Load;
    }
    bool loaded = false;
    Form mainForm;
    DateTime startTime;
    double spanInSeconds = 2;

    private void timer1_Tick(object sender, EventArgs e)
    {
      Size Diff = mainForm.Size - this.Size;
      Diff = new Size(Diff.Width / 2, Diff.Height / 2);
      Location = mainForm.Location + Diff;
      double seconds = (DateTime.UtcNow - startTime).TotalSeconds;
      if (seconds < 0 || !loaded)
        seconds = 0;
      double left = spanInSeconds - seconds;

      double startHideSeconds = 3.5;
      if (left < startHideSeconds && left >= 0)
        this.Opacity = left / startHideSeconds;
      if (TimeToCloseHasCome() && loaded)
        this.Close();
    }

    private bool TimeToCloseHasCome()
    {
      return DateTime.UtcNow - startTime > TimeSpan.FromSeconds(spanInSeconds);
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
