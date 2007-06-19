namespace ContentExtractor.Gui
{
  partial class WebBrowserWrapper
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebBrowserWrapper));
      this.webBrowser1 = new ContentExtractor.Core.ExtendedWebBrowser();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.backButton = new System.Windows.Forms.ToolStripButton();
      this.forwardButton = new System.Windows.Forms.ToolStripButton();
      this.urlComboBox = new System.Windows.Forms.ToolStripComboBox();
      this.goButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.label1 = new System.Windows.Forms.Label();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // webBrowser1
      // 
      this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
      this.webBrowser1.Location = new System.Drawing.Point(0, 25);
      this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
      this.webBrowser1.Name = "webBrowser1";
      this.webBrowser1.ScriptErrorsSuppressed = true;
      this.webBrowser1.Size = new System.Drawing.Size(497, 381);
      this.webBrowser1.TabIndex = 0;
      this.webBrowser1.BeforeNewWindow += new System.EventHandler<ContentExtractor.Core.ExtendedNavigatingEventArgs>(this.webBrowser1_BeforeNewWindow);
      this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
      this.webBrowser1.NewWindow += new System.ComponentModel.CancelEventHandler(this.webBrowser1_NewWindow);
      // 
      // toolStrip1
      // 
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backButton,
            this.forwardButton,
            this.urlComboBox,
            this.goButton,
            this.toolStripSeparator1,
            this.toolStripButton1});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
      this.toolStrip1.Size = new System.Drawing.Size(497, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // backButton
      // 
      this.backButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.backButton.Image = global::ContentExtractor.Gui.Properties.Resources.NavBack;
      this.backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.backButton.Name = "backButton";
      this.backButton.Size = new System.Drawing.Size(23, 22);
      this.backButton.Text = "back";
      this.backButton.Click += new System.EventHandler(this.toolStripButton1_Click);
      // 
      // forwardButton
      // 
      this.forwardButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.forwardButton.Image = global::ContentExtractor.Gui.Properties.Resources.NavForward;
      this.forwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.forwardButton.Name = "forwardButton";
      this.forwardButton.Size = new System.Drawing.Size(23, 22);
      this.forwardButton.Text = "forward";
      this.forwardButton.Click += new System.EventHandler(this.toolStripButton2_Click);
      // 
      // urlComboBox
      // 
      this.urlComboBox.Items.AddRange(new object[] {
            "www.yandex.ru",
            "C:\\Documents and Settings\\Сергей\\Рабочий стол\\Incoming\\ReGet\\Яндекс.htm",
            "www.download.com/3101-20-0-1.html?tag=pop",
            "demo.sugarondemand.com/sugarcrm_os/index.php?action=Login&module=Users",
            "www.stat.ru/cgi-bin/awstats.pl?config=www.implart.com",
            "www.sugar.ru",
            "www.increase.ru"});
      this.urlComboBox.Name = "urlComboBox";
      this.urlComboBox.Size = new System.Drawing.Size(221, 25);
      this.urlComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.urlComboBox_KeyDown);
      this.urlComboBox.SelectedIndexChanged += new System.EventHandler(this.urlComboBox_SelectedIndexChanged);
      // 
      // goButton
      // 
      this.goButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.goButton.Image = global::ContentExtractor.Gui.Properties.Resources.go;
      this.goButton.ImageTransparentColor = System.Drawing.Color.LightGray;
      this.goButton.Name = "goButton";
      this.goButton.Size = new System.Drawing.Size(23, 22);
      this.goButton.Text = "Go";
      this.goButton.Click += new System.EventHandler(this.goButton_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripButton1
      // 
      this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
      this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton1.Name = "toolStripButton1";
      this.toolStripButton1.Size = new System.Drawing.Size(102, 22);
      this.toolStripButton1.Text = "Add page to list";
      this.toolStripButton1.Visible = false;
      this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click_1);
      // 
      // progressBar1
      // 
      this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.progressBar1.Location = new System.Drawing.Point(0, 406);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(497, 17);
      this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
      this.progressBar1.TabIndex = 2;
      this.progressBar1.Visible = false;
      // 
      // label1
      // 
      this.label1.BackColor = System.Drawing.SystemColors.Window;
      this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.label1.Location = new System.Drawing.Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(497, 406);
      this.label1.TabIndex = 3;
      this.label1.Text = "The page is loading ...";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // WebBrowserWrapper
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.webBrowser1);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.progressBar1);
      this.Name = "WebBrowserWrapper";
      this.Size = new System.Drawing.Size(497, 423);
      this.DragDrop += new System.Windows.Forms.DragEventHandler(this.WebBrowserWrapper_DragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(this.WebBrowserWrapper_DragEnter);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private ContentExtractor.Core.ExtendedWebBrowser webBrowser1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripComboBox urlComboBox;
    private System.Windows.Forms.ToolStripButton backButton;
    private System.Windows.Forms.ToolStripButton forwardButton;
    private System.Windows.Forms.ToolStripButton goButton;
    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripButton1;
  }
}
