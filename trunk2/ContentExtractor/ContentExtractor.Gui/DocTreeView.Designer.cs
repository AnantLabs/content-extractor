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
      this.imageList1 = new System.Windows.Forms.ImageList(this.components);
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.SuspendLayout();
      // 
      // treeView1
      // 
      this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView1.HideSelection = false;
      this.treeView1.HotTracking = true;
      this.treeView1.ImageIndex = 0;
      this.treeView1.ImageList = this.imageList1;
      this.treeView1.Location = new System.Drawing.Point(0, 0);
      this.treeView1.Name = "treeView1";
      this.treeView1.SelectedImageIndex = 0;
      this.treeView1.Size = new System.Drawing.Size(453, 535);
      this.treeView1.TabIndex = 0;
      this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
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
      this.timer1.Enabled = true;
      this.timer1.Interval = 500;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // DocTreeView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.treeView1);
      this.Name = "DocTreeView";
      this.Size = new System.Drawing.Size(453, 535);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView treeView1;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.ImageList imageList1;
  }
}
