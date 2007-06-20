using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ContentExtractor.Gui;

namespace WebExtractor_Testing
{
  public class BrowserForm : Form
  {
    public BrowserForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
      this.BrowserWrapper = new ContentExtractor.Gui.WebBrowserWrapper();
      this.TreeViewWrapper = new ContentExtractor.Gui.TreeViewWrapper();
      this.GridWrapper = new GridWrapper();
      this.PagePanel = new PagesPanel();
      this.SuspendLayout();
      //
      // GridWrapper
      //
      this.GridWrapper.Dock = DockStyle.Bottom;
      //
      // PagePanel
      //
      this.PagePanel.Dock = DockStyle.Top;
      // 
      // TreeViewWrapper
      // 
      this.TreeViewWrapper.Dock = System.Windows.Forms.DockStyle.Left;
      this.TreeViewWrapper.Location = new System.Drawing.Point(0, 0);
      this.TreeViewWrapper.MinimumSize = new System.Drawing.Size(20, 20);
      this.TreeViewWrapper.Name = "TreeViewWrapper";
      this.TreeViewWrapper.Size = new System.Drawing.Size(77, 266);
      this.TreeViewWrapper.TabIndex = 1;
      this.TreeViewWrapper.TreeView.AfterSelect += new TreeViewEventHandler(TreeView_AfterSelect);

      this.GridWrapper.Dock = DockStyle.Right;
      // 
      // BrowserWrapper
      // 
      this.BrowserWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
      this.BrowserWrapper.Location = new System.Drawing.Point(77, 0);
      this.BrowserWrapper.MinimumSize = new System.Drawing.Size(20, 20);
      this.BrowserWrapper.Name = "BrowserWrapper";
      this.BrowserWrapper.Size = new System.Drawing.Size(215, 266);
      this.BrowserWrapper.TabIndex = 0;
      // 
      // BrowserForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 266);
      this.Controls.Add(this.BrowserWrapper);
      this.Controls.Add(this.TreeViewWrapper);
      this.Controls.Add(this.GridWrapper);
      this.Name = "BrowserForm";
      this.Text = "Form1";
      this.ResumeLayout(false);
    }

    void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
      TreeViewSelectCalled = true;
    }

    public bool TreeViewSelectCalled = false;

    public void InvokeClick()
    {
      EventArgs e = new EventArgs();
      this.OnClick(e);
    }

    public TreeViewWrapper TreeViewWrapper;
    public WebBrowserWrapper BrowserWrapper;
    public GridWrapper GridWrapper;
    public PagesPanel PagePanel;

    public WebBrowser Browser
    {
      get { return BrowserWrapper.Browser; }
    }
  }
}
