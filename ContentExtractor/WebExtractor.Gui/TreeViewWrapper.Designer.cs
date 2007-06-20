namespace ContentExtractor.Gui
{
  partial class TreeViewWrapper
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TreeViewWrapper));
      this.treeView = new System.Windows.Forms.TreeView();
      this.imageList1 = new System.Windows.Forms.ImageList(this.components);
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toParentButton = new System.Windows.Forms.ToolStripButton();
      this.toPrevButton = new System.Windows.Forms.ToolStripButton();
      this.toNextButton = new System.Windows.Forms.ToolStripButton();
      this.toChildButton = new System.Windows.Forms.ToolStripButton();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.selectedNodeLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.markNodeAsNewColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // treeView
      // 
      this.treeView.AllowDrop = true;
      this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView.HideSelection = false;
      this.treeView.HotTracking = true;
      this.treeView.ImageKey = "attribute";
      this.treeView.ImageList = this.imageList1;
      this.treeView.Location = new System.Drawing.Point(0, 25);
      this.treeView.Name = "treeView";
      this.treeView.SelectedImageIndex = 0;
      this.treeView.Size = new System.Drawing.Size(593, 413);
      this.treeView.TabIndex = 0;
      this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
      this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
      this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
      // 
      // imageList1
      // 
      this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
      this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "attribute");
      this.imageList1.Images.SetKeyName(1, "tag");
      this.imageList1.Images.SetKeyName(2, "text");
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toParentButton,
            this.toPrevButton,
            this.toNextButton,
            this.toChildButton});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(593, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // toParentButton
      // 
      this.toParentButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toParentButton.Image = global::ContentExtractor.Gui.Properties.Resources.FillLeftHS;
      this.toParentButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toParentButton.Name = "toParentButton";
      this.toParentButton.Size = new System.Drawing.Size(23, 22);
      this.toParentButton.Text = "Go to the parent element";
      this.toParentButton.ToolTipText = "Go to the parent element";
      this.toParentButton.Click += new System.EventHandler(this.toParentButton_Click);
      // 
      // toPrevButton
      // 
      this.toPrevButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toPrevButton.Image = global::ContentExtractor.Gui.Properties.Resources.FillUpHS;
      this.toPrevButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toPrevButton.Name = "toPrevButton";
      this.toPrevButton.Size = new System.Drawing.Size(23, 22);
      this.toPrevButton.Text = "Go to the previous element";
      this.toPrevButton.ToolTipText = "Go to the previous element";
      this.toPrevButton.Click += new System.EventHandler(this.toPrevButton_Click);
      // 
      // toNextButton
      // 
      this.toNextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toNextButton.Image = global::ContentExtractor.Gui.Properties.Resources.FillDownHS;
      this.toNextButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toNextButton.Name = "toNextButton";
      this.toNextButton.Size = new System.Drawing.Size(23, 22);
      this.toNextButton.Text = "Go to the next element";
      this.toNextButton.ToolTipText = "Go to the next element";
      this.toNextButton.Click += new System.EventHandler(this.toNextButton_Click);
      // 
      // toChildButton
      // 
      this.toChildButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toChildButton.Image = global::ContentExtractor.Gui.Properties.Resources.FillRightHS;
      this.toChildButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toChildButton.Name = "toChildButton";
      this.toChildButton.Size = new System.Drawing.Size(23, 22);
      this.toChildButton.Text = "Go to the first child";
      this.toChildButton.ToolTipText = "Go to the first child";
      this.toChildButton.Click += new System.EventHandler(this.toChildButton_Click);
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedNodeLabel});
      this.statusStrip1.Location = new System.Drawing.Point(0, 438);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.ShowItemToolTips = true;
      this.statusStrip1.Size = new System.Drawing.Size(593, 22);
      this.statusStrip1.TabIndex = 2;
      this.statusStrip1.Text = "statusStrip1";
      this.statusStrip1.Resize += new System.EventHandler(this.statusStrip1_Resize);
      // 
      // selectedNodeLabel
      // 
      this.selectedNodeLabel.AutoSize = false;
      this.selectedNodeLabel.AutoToolTip = true;
      this.selectedNodeLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.selectedNodeLabel.Name = "selectedNodeLabel";
      this.selectedNodeLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
      this.selectedNodeLabel.Size = new System.Drawing.Size(79, 17);
      this.selectedNodeLabel.Text = "Selected node:";
      this.selectedNodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.markNodeAsNewColumnToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(207, 48);
      // 
      // markNodeAsNewColumnToolStripMenuItem
      // 
      this.markNodeAsNewColumnToolStripMenuItem.Name = "markNodeAsNewColumnToolStripMenuItem";
      this.markNodeAsNewColumnToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
      this.markNodeAsNewColumnToolStripMenuItem.Text = "Create column from node";
      this.markNodeAsNewColumnToolStripMenuItem.Click += new System.EventHandler(this.markNodeAsNewColumnToolStripMenuItem_Click);
      // 
      // TreeViewWrapper
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.treeView);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.toolStrip1);
      this.Name = "TreeViewWrapper";
      this.Size = new System.Drawing.Size(593, 460);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TreeView treeView;
    private System.Windows.Forms.ImageList imageList1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toParentButton;
    private System.Windows.Forms.ToolStripButton toPrevButton;
    private System.Windows.Forms.ToolStripButton toNextButton;
    private System.Windows.Forms.ToolStripButton toChildButton;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel selectedNodeLabel;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem markNodeAsNewColumnToolStripMenuItem;
  }
}
