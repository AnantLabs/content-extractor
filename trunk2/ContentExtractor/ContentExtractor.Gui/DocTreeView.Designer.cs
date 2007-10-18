namespace ContentExtractor.Gui
{
  partial class DocTreeView
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocTreeView));
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addColumnMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.imageList1 = new System.Windows.Forms.ImageList(this.components);
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
      this.contextMenuStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // treeView1
      // 
      this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
      this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView1.FullRowSelect = true;
      this.treeView1.HideSelection = false;
      this.treeView1.HotTracking = true;
      this.treeView1.ImageIndex = 0;
      this.treeView1.ImageList = this.imageList1;
      this.treeView1.Location = new System.Drawing.Point(0, 0);
      this.treeView1.Name = "treeView1";
      this.treeView1.SelectedImageIndex = 0;
      this.treeView1.ShowNodeToolTips = true;
      this.treeView1.Size = new System.Drawing.Size(453, 535);
      this.treeView1.TabIndex = 0;
      this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
      this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addColumnMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(213, 26);
      // 
      // addColumnMenuItem
      // 
      this.addColumnMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.AddTableHS;
      this.addColumnMenuItem.Name = "addColumnMenuItem";
      this.addColumnMenuItem.Size = new System.Drawing.Size(212, 22);
      this.addColumnMenuItem.Text = "Add column from this node";
      this.addColumnMenuItem.Click += new System.EventHandler(this.addColumnMenuItem_Click);
      // 
      // imageList1
      // 
      this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
      this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "attribute");
      this.imageList1.Images.SetKeyName(1, "tag");
      this.imageList1.Images.SetKeyName(2, "text");
      // 
      // timer1
      // 
      this.timer1.Interval = 500;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // statusStrip1
      // 
      this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
      this.statusStrip1.Location = new System.Drawing.Point(0, 513);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(453, 22);
      this.statusStrip1.TabIndex = 1;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // toolStripStatusLabel2
      // 
      this.toolStripStatusLabel2.AutoToolTip = true;
      this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      this.toolStripStatusLabel2.Size = new System.Drawing.Size(51, 17);
      this.toolStripStatusLabel2.Text = "/HTML[1]";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(285, 17);
      this.toolStripStatusLabel1.Spring = true;
      this.toolStripStatusLabel1.Text = "Complete";
      this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // toolStripProgressBar1
      // 
      this.toolStripProgressBar1.Name = "toolStripProgressBar1";
      this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
      this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
      // 
      // DocTreeView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.treeView1);
      this.Name = "DocTreeView";
      this.Size = new System.Drawing.Size(453, 535);
      this.contextMenuStrip1.ResumeLayout(false);
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TreeView treeView1;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.ImageList imageList1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem addColumnMenuItem;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
  }
}
