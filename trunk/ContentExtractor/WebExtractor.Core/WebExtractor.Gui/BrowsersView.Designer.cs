namespace ContentExtractor.Gui
{
  partial class BrowsersView
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
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.TabsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.TabsContextMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabControl1
      // 
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.HotTrack = true;
      this.tabControl1.Location = new System.Drawing.Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(687, 436);
      this.tabControl1.TabIndex = 0;
      this.tabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseDown);
      this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
      // 
      // TabsContextMenu
      // 
      this.TabsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTabToolStripMenuItem,
            this.closeToolStripMenuItem});
      this.TabsContextMenu.Name = "tabsCcontextMenu";
      this.TabsContextMenu.Size = new System.Drawing.Size(126, 48);
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.Critical;
      this.closeToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.closeToolStripMenuItem.Text = "Close";
      this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
      // 
      // newTabToolStripMenuItem
      // 
      this.newTabToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.AddTableHS;
      this.newTabToolStripMenuItem.Name = "newTabToolStripMenuItem";
      this.newTabToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.newTabToolStripMenuItem.Text = "New tab";
      this.newTabToolStripMenuItem.Click += new System.EventHandler(this.newTabToolStripMenuItem_Click);
      // 
      // BrowsersView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControl1);
      this.Name = "BrowsersView";
      this.Size = new System.Drawing.Size(687, 436);
      this.TabsContextMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    public System.Windows.Forms.ContextMenuStrip TabsContextMenu;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newTabToolStripMenuItem;
  }
}
