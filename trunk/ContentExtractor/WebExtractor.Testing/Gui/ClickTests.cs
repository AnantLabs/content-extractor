using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Windows.Forms;

namespace WebExtractor_Testing.Gui
{
  [TestFixture]
  public class ClickOnForm
  {
    class TestForm : Form
    {
      public TestForm()
      {
        InitializeComponent();
      }
      private System.ComponentModel.IContainer components = null;

      protected override void Dispose(bool disposing)
      {
        if (disposing && (components != null))
        {
          components.Dispose();
        }
        base.Dispose(disposing);
      }

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
        this.Browser = new System.Windows.Forms.WebBrowser();
        this.SuspendLayout();

        this.Browser.Dock = System.Windows.Forms.DockStyle.Fill;
        this.Browser.Location = new System.Drawing.Point(0, 0);
        this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
        this.Browser.Name = "webBrowser1";
        this.Browser.Size = new System.Drawing.Size(292, 266);
        this.Browser.TabIndex = 0;

        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(292, 266);
        this.Controls.Add(this.Browser);
        this.Location = new System.Drawing.Point(0, 0);
        this.Name = "Form1";
        this.Text = "Form1";
        this.ResumeLayout(false);
        this.Click +=new EventHandler(TestForm_Click);

        this.SetStyle(ControlStyles.UserMouse, true);
      }

      public bool clicked = false;
      public void TestForm_Click(object sender, EventArgs e)
      {
        clicked = true;
      }

      public void InvokeClick()
      {
        EventArgs e = new EventArgs();
        OnClick(e);
      }

      public WebBrowser Browser;
    }

    [Test]
    public void ClickTest()
    {
      TestForm form = new TestForm();
      form.Browser.DocumentText = "<html><body>Hello world!</body></html>";
      TestUtils.DoSomeEvents();

      form.Browser.Document.Click += new HtmlElementEventHandler(form.TestForm_Click);

      //SendMessage(form, 0x0201, new IntPtr(0x0001), MakeLParam(100, 100));
      //SendMessage(form, 0x0202, new IntPtr(0x0001), MakeLParam(100, 100));

      form.InvokeClick();
      Assert.IsTrue(form.clicked, "Нажатие не обработалось");
    }

  }
}
