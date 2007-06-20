namespace ContentExtractor.Gui
{
  partial class BrowserPage
  {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.progressBar = new System.Windows.Forms.ProgressBar();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.backButton = new System.Windows.Forms.ToolStripButton();
      this.forwardButton = new System.Windows.Forms.ToolStripButton();
      this.urlComboBox = new System.Windows.Forms.ToolStripComboBox();
      this.goButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
      this.label1 = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.Browser = new ContentExtractor.Core.ExtendedWebBrowser();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.createColumnFromNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStrip1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // progressBar
      // 
      this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.progressBar.Location = new System.Drawing.Point(0, 456);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size(751, 23);
      this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
      this.progressBar.TabIndex = 1;
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backButton,
            this.forwardButton,
            this.urlComboBox,
            this.goButton,
            this.toolStripSeparator1,
            this.toolStripButton2});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(751, 25);
      this.toolStrip1.TabIndex = 2;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // backButton
      // 
      this.backButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.backButton.Image = global::ContentExtractor.Gui.Properties.Resources.NavBack;
      this.backButton.ImageTransparentColor = System.Drawing.Color.Silver;
      this.backButton.Name = "backButton";
      this.backButton.Size = new System.Drawing.Size(23, 22);
      this.backButton.Text = "Back";
      this.backButton.Click += new System.EventHandler(this.backButton_Click);
      // 
      // forwardButton
      // 
      this.forwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.forwardButton.Image = global::ContentExtractor.Gui.Properties.Resources.NavForward;
      this.forwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.forwardButton.Name = "forwardButton";
      this.forwardButton.Size = new System.Drawing.Size(23, 22);
      this.forwardButton.Text = "Forward";
      this.forwardButton.Click += new System.EventHandler(this.forwardButton_Click);
      // 
      // urlComboBox
      // 
      this.urlComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
      this.urlComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
      this.urlComboBox.Name = "urlComboBox";
      this.urlComboBox.Size = new System.Drawing.Size(200, 25);
      this.urlComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.urlComboBox_KeyDown);
      this.urlComboBox.SelectedIndexChanged += new System.EventHandler(this.urlComboBox_SelectedIndexChanged);
      // 
      // goButton
      // 
      this.goButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.goButton.Image = global::ContentExtractor.Gui.Properties.Resources.OK;
      this.goButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.goButton.Name = "goButton";
      this.goButton.Size = new System.Drawing.Size(23, 22);
      this.goButton.Text = "Go!";
      this.goButton.Click += new System.EventHandler(this.goButton_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripButton2
      // 
      this.toolStripButton2.Image = global::ContentExtractor.Gui.Properties.Resources.AddTableHS;
      this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton2.Name = "toolStripButton2";
      this.toolStripButton2.Size = new System.Drawing.Size(122, 22);
      this.toolStripButton2.Text = "Add this page to list";
      this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
      // 
      // label1
      // 
      this.label1.BackColor = System.Drawing.SystemColors.Window;
      this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label1.Location = new System.Drawing.Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(751, 431);
      this.label1.TabIndex = 3;
      this.label1.Text = "Please wait while the browser is loading...";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.Browser);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 25);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(751, 431);
      this.panel1.TabIndex = 4;
      // 
      // Browser
      // 
      this.Browser.Dock = System.Windows.Forms.DockStyle.Fill;
      this.Browser.IsWebBrowserContextMenuEnabled = false;
      this.Browser.Location = new System.Drawing.Point(0, 0);
      this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
      this.Browser.Name = "Browser";
      this.Browser.Size = new System.Drawing.Size(751, 431);
      this.Browser.TabIndex = 0;
      this.Browser.BeforeNewWindow += new System.EventHandler<ContentExtractor.Core.ExtendedNavigatingEventArgs>(this.Browser_BeforeNewWindow);
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createColumnFromNodeToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(207, 48);
      // 
      // createColumnFromNodeToolStripMenuItem
      // 
      this.createColumnFromNodeToolStripMenuItem.Name = "createColumnFromNodeToolStripMenuItem";
      this.createColumnFromNodeToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
      this.createColumnFromNodeToolStripMenuItem.Text = "Create column from node";
      this.createColumnFromNodeToolStripMenuItem.Click += new System.EventHandler(this.createColumnFromNodeToolStripMenuItem_Click);
      // 
      // BrowserPage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.progressBar);
      this.Name = "BrowserPage";
      this.Size = new System.Drawing.Size(751, 479);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ProgressBar progressBar;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ToolStripButton backButton;
    private System.Windows.Forms.ToolStripButton forwardButton;
    private System.Windows.Forms.ToolStripButton goButton;
    public System.Windows.Forms.ToolStripComboBox urlComboBox;
    public ContentExtractor.Core.ExtendedWebBrowser Browser;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripButton2;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem createColumnFromNodeToolStripMenuItem;
  }
}
