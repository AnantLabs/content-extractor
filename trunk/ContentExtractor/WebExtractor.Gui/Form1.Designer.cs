namespace ContentExtractor.Gui
{
  partial class MainForm
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.panel1 = new WeifenLuo.WinFormsUI.DockPanel();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.contentExtractorHomepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      this.viewsTimer = new System.Windows.Forms.Timer(this.components);
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.openStripButton = new System.Windows.Forms.ToolStripButton();
      this.saveStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.undoStripButton = new System.Windows.Forms.ToolStripButton();
      this.redoStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.browserModeButton = new System.Windows.Forms.ToolStripButton();
      this.parseModeButton = new System.Windows.Forms.ToolStripButton();
      this.menuStrip1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.ActiveAutoHideContent = null;
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
      this.panel1.Location = new System.Drawing.Point(0, 49);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(893, 708);
      this.panel1.TabIndex = 1;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(893, 24);
      this.menuStrip1.TabIndex = 3;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.openHS;
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.openToolStripMenuItem.Text = "Open ...";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.saveHS;
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.saveToolStripMenuItem.Text = "Save";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // saveAsToolStripMenuItem
      // 
      this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.saveAsToolStripMenuItem.Text = "Save as ...";
      this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(135, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.Critical;
      this.exitToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // editToolStripMenuItem
      // 
      this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.editToolStripMenuItem.Text = "Edit";
      // 
      // undoToolStripMenuItem
      // 
      this.undoToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.Edit_UndoHS;
      this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
      this.undoToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
      this.undoToolStripMenuItem.Text = "Undo";
      this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
      // 
      // redoToolStripMenuItem
      // 
      this.redoToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.Edit_RedoHS;
      this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
      this.redoToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
      this.redoToolStripMenuItem.Text = "Redo";
      this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentExtractorHomepageToolStripMenuItem,
            this.toolStripMenuItem2,
            this.aboutToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
      this.helpToolStripMenuItem.Text = "Help";
      // 
      // contentExtractorHomepageToolStripMenuItem
      // 
      this.contentExtractorHomepageToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.home;
      this.contentExtractorHomepageToolStripMenuItem.Name = "contentExtractorHomepageToolStripMenuItem";
      this.contentExtractorHomepageToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
      this.contentExtractorHomepageToolStripMenuItem.Text = "Content Extractor homepage";
      this.contentExtractorHomepageToolStripMenuItem.Click += new System.EventHandler(this.contentExtractorHomepageToolStripMenuItem_Click);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(222, 6);
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
      this.aboutToolStripMenuItem.Text = "About";
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      this.openFileDialog1.Filter = "WebExtractor files|*.wxp";
      // 
      // saveFileDialog1
      // 
      this.saveFileDialog1.Filter = "WebExtractor files|*.wxp";
      // 
      // viewsTimer
      // 
      this.viewsTimer.Interval = 150;
      this.viewsTimer.Tick += new System.EventHandler(this.viewsTimer_Tick);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openStripButton,
            this.saveStripButton,
            this.toolStripSeparator1,
            this.undoStripButton,
            this.redoStripButton,
            this.toolStripSeparator2,
            this.browserModeButton,
            this.parseModeButton});
      this.toolStrip1.Location = new System.Drawing.Point(0, 24);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(893, 25);
      this.toolStrip1.TabIndex = 4;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // openStripButton
      // 
      this.openStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.openStripButton.Image = global::ContentExtractor.Gui.Properties.Resources.openHS;
      this.openStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.openStripButton.Name = "openStripButton";
      this.openStripButton.Size = new System.Drawing.Size(23, 22);
      this.openStripButton.Text = "Open";
      this.openStripButton.Click += new System.EventHandler(this.toolStripButton1_Click);
      // 
      // saveStripButton
      // 
      this.saveStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.saveStripButton.Image = global::ContentExtractor.Gui.Properties.Resources.saveHS;
      this.saveStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.saveStripButton.Name = "saveStripButton";
      this.saveStripButton.Size = new System.Drawing.Size(23, 22);
      this.saveStripButton.Text = "Save";
      this.saveStripButton.Click += new System.EventHandler(this.toolStripButton2_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // undoStripButton
      // 
      this.undoStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.undoStripButton.Image = global::ContentExtractor.Gui.Properties.Resources.Edit_UndoHS;
      this.undoStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.undoStripButton.Name = "undoStripButton";
      this.undoStripButton.Size = new System.Drawing.Size(23, 22);
      this.undoStripButton.Text = "Undo";
      this.undoStripButton.Click += new System.EventHandler(this.toolStripButton3_Click);
      // 
      // redoStripButton
      // 
      this.redoStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.redoStripButton.Image = global::ContentExtractor.Gui.Properties.Resources.Edit_RedoHS;
      this.redoStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.redoStripButton.Name = "redoStripButton";
      this.redoStripButton.Size = new System.Drawing.Size(23, 22);
      this.redoStripButton.Text = "Redo";
      this.redoStripButton.Click += new System.EventHandler(this.toolStripButton4_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // browserModeButton
      // 
      this.browserModeButton.Image = global::ContentExtractor.Gui.Properties.Resources.AlignTableCellMiddleLeftJustHS;
      this.browserModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.browserModeButton.Name = "browserModeButton";
      this.browserModeButton.Size = new System.Drawing.Size(95, 22);
      this.browserModeButton.Text = "Browser Mode";
      this.browserModeButton.ToolTipText = "Switch to browser Mode";
      this.browserModeButton.Click += new System.EventHandler(this.browserModeButton_Click);
      // 
      // parseModeButton
      // 
      this.parseModeButton.Image = global::ContentExtractor.Gui.Properties.Resources.ActualSizeHS;
      this.parseModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.parseModeButton.Name = "parseModeButton";
      this.parseModeButton.Size = new System.Drawing.Size(83, 22);
      this.parseModeButton.Text = "Parse Mode";
      this.parseModeButton.ToolTipText = "Switch to parse mode";
      this.parseModeButton.Click += new System.EventHandler(this.parseModeButton_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(893, 757);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "MainForm";
      this.Text = "Content Extractor 1.0";
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private WeifenLuo.WinFormsUI.DockPanel panel1;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
    private System.Windows.Forms.Timer viewsTimer;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton openStripButton;
    private System.Windows.Forms.ToolStripButton saveStripButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton undoStripButton;
    private System.Windows.Forms.ToolStripButton redoStripButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton browserModeButton;
    private System.Windows.Forms.ToolStripButton parseModeButton;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem contentExtractorHomepageToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
  }
}

