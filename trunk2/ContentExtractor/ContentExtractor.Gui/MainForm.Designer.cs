//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 06.07.2007
// Time: 19:06
//
namespace ContentExtractor.Gui
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.totalSplitContainer = new System.Windows.Forms.SplitContainer();
      this.leftSplitContainer = new System.Windows.Forms.SplitContainer();
      this.rightSplitContainer = new System.Windows.Forms.SplitContainer();
      this.urlsListBox1 = new ContentExtractor.Gui.UrlsListBox();
      this.browser = new ContentExtractor.Gui.MarkingBrowser();
      this.resultsView1 = new ContentExtractor.Gui.ResultsView();
      this.docTreeView = new ContentExtractor.Gui.DocTreeView();
      this.totalSplitContainer.Panel1.SuspendLayout();
      this.totalSplitContainer.Panel2.SuspendLayout();
      this.totalSplitContainer.SuspendLayout();
      this.leftSplitContainer.Panel2.SuspendLayout();
      this.leftSplitContainer.SuspendLayout();
      this.rightSplitContainer.Panel1.SuspendLayout();
      this.rightSplitContainer.Panel2.SuspendLayout();
      this.rightSplitContainer.SuspendLayout();
      this.SuspendLayout();
      // 
      // totalSplitContainer
      // 
      this.totalSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.totalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.totalSplitContainer.Location = new System.Drawing.Point(0, 0);
      this.totalSplitContainer.Name = "totalSplitContainer";
      // 
      // totalSplitContainer.Panel1
      // 
      this.totalSplitContainer.Panel1.Controls.Add(this.leftSplitContainer);
      // 
      // totalSplitContainer.Panel2
      // 
      this.totalSplitContainer.Panel2.Controls.Add(this.rightSplitContainer);
      this.totalSplitContainer.Size = new System.Drawing.Size(991, 563);
      this.totalSplitContainer.SplitterDistance = 330;
      this.totalSplitContainer.TabIndex = 0;
      // 
      // leftSplitContainer
      // 
      this.leftSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.leftSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.leftSplitContainer.Location = new System.Drawing.Point(0, 0);
      this.leftSplitContainer.Name = "leftSplitContainer";
      this.leftSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // leftSplitContainer.Panel1
      // 
      this.leftSplitContainer.Panel1.Controls.Add(this.docTreeView);
      // 
      // leftSplitContainer.Panel2
      // 
      this.leftSplitContainer.Panel2.Controls.Add(this.urlsListBox1);
      this.leftSplitContainer.Size = new System.Drawing.Size(330, 563);
      this.leftSplitContainer.SplitterDistance = 269;
      this.leftSplitContainer.TabIndex = 0;
      // 
      // rightSplitContainer
      // 
      this.rightSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.rightSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.rightSplitContainer.Location = new System.Drawing.Point(0, 0);
      this.rightSplitContainer.Name = "rightSplitContainer";
      this.rightSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // rightSplitContainer.Panel1
      // 
      this.rightSplitContainer.Panel1.Controls.Add(this.resultsView1);
      // 
      // rightSplitContainer.Panel2
      // 
      this.rightSplitContainer.Panel2.Controls.Add(this.browser);
      this.rightSplitContainer.Size = new System.Drawing.Size(657, 563);
      this.rightSplitContainer.SplitterDistance = 219;
      this.rightSplitContainer.TabIndex = 0;
      // 
      // urlsListBox1
      // 
      this.urlsListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.urlsListBox1.Location = new System.Drawing.Point(0, 0);
      this.urlsListBox1.Name = "urlsListBox1";
      this.urlsListBox1.Size = new System.Drawing.Size(326, 286);
      this.urlsListBox1.TabIndex = 0;
      // 
      // browser
      // 
      this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
      this.browser.Location = new System.Drawing.Point(0, 0);
      this.browser.Name = "browser";
      this.browser.Size = new System.Drawing.Size(653, 336);
      this.browser.TabIndex = 0;
      // 
      // resultsView1
      // 
      this.resultsView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.resultsView1.Location = new System.Drawing.Point(0, 0);
      this.resultsView1.Name = "resultsView1";
      this.resultsView1.Size = new System.Drawing.Size(653, 215);
      this.resultsView1.TabIndex = 0;
      // 
      // docTreeView
      // 
      this.docTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.docTreeView.Location = new System.Drawing.Point(0, 0);
      this.docTreeView.Name = "docTreeView";
      this.docTreeView.Size = new System.Drawing.Size(653, 215);
      this.docTreeView.TabIndex = 0;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(991, 563);
      this.Controls.Add(this.totalSplitContainer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MainForm";
      this.Text = "ContentExtractor.Gui";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
      this.totalSplitContainer.Panel1.ResumeLayout(false);
      this.totalSplitContainer.Panel2.ResumeLayout(false);
      this.totalSplitContainer.ResumeLayout(false);
      this.leftSplitContainer.Panel2.ResumeLayout(false);
      this.leftSplitContainer.ResumeLayout(false);
      this.rightSplitContainer.Panel1.ResumeLayout(false);
      this.rightSplitContainer.Panel2.ResumeLayout(false);
      this.rightSplitContainer.ResumeLayout(false);
      this.ResumeLayout(false);

		}
		private ContentExtractor.Gui.UrlsListBox urlsListBox1;
		private System.Windows.Forms.SplitContainer totalSplitContainer;
		private System.Windows.Forms.SplitContainer leftSplitContainer;
		private System.Windows.Forms.SplitContainer rightSplitContainer;
    private MarkingBrowser browser;
    private ResultsView resultsView1;
    private DocTreeView docTreeView;
	}
}
