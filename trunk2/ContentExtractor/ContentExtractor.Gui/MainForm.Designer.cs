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
      this.docTreeView = new ContentExtractor.Gui.DocTreeView();
      this.urlsListBox1 = new ContentExtractor.Gui.UrlsListBox();
      this.rightSplitContainer = new System.Windows.Forms.SplitContainer();
      this.resultsView1 = new ContentExtractor.Gui.ResultsView();
      this.browser = new ContentExtractor.Gui.MarkingBrowser();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.totalSplitContainer.Panel1.SuspendLayout();
      this.totalSplitContainer.Panel2.SuspendLayout();
      this.totalSplitContainer.SuspendLayout();
      this.leftSplitContainer.Panel1.SuspendLayout();
      this.leftSplitContainer.Panel2.SuspendLayout();
      this.leftSplitContainer.SuspendLayout();
      this.rightSplitContainer.Panel1.SuspendLayout();
      this.rightSplitContainer.Panel2.SuspendLayout();
      this.rightSplitContainer.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // totalSplitContainer
      // 
      this.totalSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.totalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.totalSplitContainer.Location = new System.Drawing.Point(0, 49);
      this.totalSplitContainer.Name = "totalSplitContainer";
      // 
      // totalSplitContainer.Panel1
      // 
      this.totalSplitContainer.Panel1.Controls.Add(this.leftSplitContainer);
      // 
      // totalSplitContainer.Panel2
      // 
      this.totalSplitContainer.Panel2.Controls.Add(this.rightSplitContainer);
      this.totalSplitContainer.Size = new System.Drawing.Size(991, 514);
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
      this.leftSplitContainer.Size = new System.Drawing.Size(330, 514);
      this.leftSplitContainer.SplitterDistance = 245;
      this.leftSplitContainer.TabIndex = 0;
      // 
      // docTreeView
      // 
      this.docTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.docTreeView.Location = new System.Drawing.Point(0, 0);
      this.docTreeView.Name = "docTreeView";
      this.docTreeView.Size = new System.Drawing.Size(326, 241);
      this.docTreeView.TabIndex = 0;
      // 
      // urlsListBox1
      // 
      this.urlsListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.urlsListBox1.Location = new System.Drawing.Point(0, 0);
      this.urlsListBox1.Name = "urlsListBox1";
      this.urlsListBox1.Size = new System.Drawing.Size(326, 261);
      this.urlsListBox1.TabIndex = 0;
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
      this.rightSplitContainer.Size = new System.Drawing.Size(657, 514);
      this.rightSplitContainer.SplitterDistance = 199;
      this.rightSplitContainer.TabIndex = 0;
      // 
      // resultsView1
      // 
      this.resultsView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.resultsView1.Location = new System.Drawing.Point(0, 0);
      this.resultsView1.Name = "resultsView1";
      this.resultsView1.Size = new System.Drawing.Size(653, 195);
      this.resultsView1.TabIndex = 0;
      // 
      // browser
      // 
      this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
      this.browser.Location = new System.Drawing.Point(0, 0);
      this.browser.Name = "browser";
      this.browser.Size = new System.Drawing.Size(653, 307);
      this.browser.TabIndex = 0;
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.saveToolStripButton});
      this.toolStrip1.Location = new System.Drawing.Point(0, 24);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(991, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // saveToolStripButton
      // 
      this.saveToolStripButton.Image = global::ContentExtractor.Gui.Properties.Resources.saveHS;
      this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.saveToolStripButton.Name = "saveToolStripButton";
      this.saveToolStripButton.Size = new System.Drawing.Size(96, 22);
      this.saveToolStripButton.Text = "Save template";
      this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
      // 
      // saveFileDialog2
      // 
      this.saveFileDialog2.DefaultExt = "cet";
      this.saveFileDialog2.Filter = "Content Extractor templates|*.cet";
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(991, 24);
      this.menuStrip1.TabIndex = 2;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveTemplateToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // saveTemplateToolStripMenuItem
      // 
      this.saveTemplateToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.saveHS;
      this.saveTemplateToolStripMenuItem.Name = "saveTemplateToolStripMenuItem";
      this.saveTemplateToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.saveTemplateToolStripMenuItem.Text = "Save template";
      this.saveTemplateToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripButton_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(151, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
      this.helpToolStripMenuItem.Text = "Help";
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.infoBubble;
      this.aboutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
      this.aboutToolStripMenuItem.Text = "About";
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // toolStripButton1
      // 
      this.toolStripButton1.Image = global::ContentExtractor.Gui.Properties.Resources.openHS;
      this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton1.Name = "toolStripButton1";
      this.toolStripButton1.Size = new System.Drawing.Size(130, 22);
      this.toolStripButton1.Text = "Open saved template";
      this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      this.openFileDialog1.Filter = "Content Extractor templates|*.cet";
      this.openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(991, 563);
      this.Controls.Add(this.totalSplitContainer);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "MainForm";
      this.Text = "ContentExtractor.Gui";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
      this.totalSplitContainer.Panel1.ResumeLayout(false);
      this.totalSplitContainer.Panel2.ResumeLayout(false);
      this.totalSplitContainer.ResumeLayout(false);
      this.leftSplitContainer.Panel1.ResumeLayout(false);
      this.leftSplitContainer.Panel2.ResumeLayout(false);
      this.leftSplitContainer.ResumeLayout(false);
      this.rightSplitContainer.Panel1.ResumeLayout(false);
      this.rightSplitContainer.Panel2.ResumeLayout(false);
      this.rightSplitContainer.ResumeLayout(false);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

		}
		private ContentExtractor.Gui.UrlsListBox urlsListBox1;
		private System.Windows.Forms.SplitContainer totalSplitContainer;
		private System.Windows.Forms.SplitContainer leftSplitContainer;
		private System.Windows.Forms.SplitContainer rightSplitContainer;
    private MarkingBrowser browser;
    private ResultsView resultsView1;
    private DocTreeView docTreeView;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton saveToolStripButton;
    private System.Windows.Forms.SaveFileDialog saveFileDialog2;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveTemplateToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton toolStripButton1;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}
