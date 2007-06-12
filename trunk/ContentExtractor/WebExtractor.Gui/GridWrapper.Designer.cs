namespace ContentExtractor.Gui
{
  partial class GridWrapper
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridWrapper));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.xlsButton = new System.Windows.Forms.ToolStripButton();
      this.xmlButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
      this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.dataGridView = new System.Windows.Forms.DataGridView();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.columnPage = new System.Windows.Forms.TabPage();
      this.columnPropertyGrid = new System.Windows.Forms.PropertyGrid();
      this.templatePage = new System.Windows.Forms.TabPage();
      this.templatePropertyGrid = new System.Windows.Forms.PropertyGrid();
      this.toolStrip1.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
      this.tabControl1.SuspendLayout();
      this.columnPage.SuspendLayout();
      this.templatePage.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xlsButton,
            this.xmlButton,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton1});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(849, 25);
      this.toolStrip1.TabIndex = 2;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // xlsButton
      // 
      this.xlsButton.Image = ((System.Drawing.Image)(resources.GetObject("xlsButton.Image")));
      this.xlsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.xlsButton.Name = "xlsButton";
      this.xlsButton.Size = new System.Drawing.Size(92, 22);
      this.xlsButton.Text = "Save to Excel";
      this.xlsButton.Click += new System.EventHandler(this.xlsButton_Click);
      // 
      // xmlButton
      // 
      this.xmlButton.Image = global::ContentExtractor.Gui.Properties.Resources.VSProject_xml;
      this.xmlButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.xmlButton.Name = "xmlButton";
      this.xmlButton.Size = new System.Drawing.Size(86, 22);
      this.xmlButton.Text = "Save to XML";
      this.xmlButton.Click += new System.EventHandler(this.xmlButton_Click);
      // 
      // toolStripButton2
      // 
      this.toolStripButton2.Image = global::ContentExtractor.Gui.Properties.Resources.HTMLPageHS;
      this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton2.Name = "toolStripButton2";
      this.toolStripButton2.Size = new System.Drawing.Size(88, 22);
      this.toolStripButton2.Text = "Save to Html";
      this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripButton1
      // 
      this.toolStripButton1.Image = global::ContentExtractor.Gui.Properties.Resources.DeleteHS;
      this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton1.Name = "toolStripButton1";
      this.toolStripButton1.Size = new System.Drawing.Size(97, 22);
      this.toolStripButton1.Text = "Clear template";
      this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator2,
            this.deleteToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(117, 98);
      this.contextMenuStrip1.Text = "Columns";
      this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
      // 
      // cutToolStripMenuItem
      // 
      this.cutToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.CutHS;
      this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
      this.cutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
      this.cutToolStripMenuItem.Text = "Cut";
      this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
      // 
      // copyToolStripMenuItem
      // 
      this.copyToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.CopyHS;
      this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
      this.copyToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
      this.copyToolStripMenuItem.Text = "Copy";
      this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
      // 
      // pasteToolStripMenuItem
      // 
      this.pasteToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.PasteHS;
      this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
      this.pasteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
      this.pasteToolStripMenuItem.Text = "Paste";
      this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(113, 6);
      // 
      // deleteToolStripMenuItem
      // 
      this.deleteToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.DeleteHS;
      this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
      this.deleteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
      this.deleteToolStripMenuItem.Text = "Delete";
      this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
      // 
      // splitContainer1
      // 
      this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.splitContainer1.DataBindings.Add(new System.Windows.Forms.Binding("SplitterDistance", global::ContentExtractor.Gui.Properties.Settings.Default, "GridSplitterDistance", true));
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 25);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.dataGridView);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
      this.splitContainer1.Size = new System.Drawing.Size(849, 576);
      this.splitContainer1.SplitterDistance = global::ContentExtractor.Gui.Properties.Settings.Default.GridSplitterDistance;
      this.splitContainer1.TabIndex = 3;
      // 
      // dataGridView
      // 
      this.dataGridView.AllowDrop = true;
      this.dataGridView.AllowUserToAddRows = false;
      this.dataGridView.AllowUserToDeleteRows = false;
      this.dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridView.EnableHeadersVisualStyles = false;
      this.dataGridView.Location = new System.Drawing.Point(0, 0);
      this.dataGridView.MultiSelect = false;
      this.dataGridView.Name = "dataGridView";
      this.dataGridView.ReadOnly = true;
      this.dataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.dataGridView.RowTemplate.Height = 19;
      this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect;
      this.dataGridView.ShowEditingIcon = false;
      this.dataGridView.Size = new System.Drawing.Size(682, 574);
      this.dataGridView.TabIndex = 1;
      this.dataGridView.VirtualMode = true;
      this.dataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseDown);
      this.dataGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragOver);
      this.dataGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragEnter);
      this.dataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragDrop);
      this.dataGridView.DragLeave += new System.EventHandler(this.dataGridView_DragLeave);
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.columnPage);
      this.tabControl1.Controls.Add(this.templatePage);
      this.tabControl1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedIndex", global::ContentExtractor.Gui.Properties.Settings.Default, "TemplatePropertyGridSelectedTab", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.HotTrack = true;
      this.tabControl1.Location = new System.Drawing.Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = global::ContentExtractor.Gui.Properties.Settings.Default.TemplatePropertyGridSelectedTab;
      this.tabControl1.Size = new System.Drawing.Size(159, 574);
      this.tabControl1.TabIndex = 1;
      // 
      // columnPage
      // 
      this.columnPage.Controls.Add(this.columnPropertyGrid);
      this.columnPage.Location = new System.Drawing.Point(4, 22);
      this.columnPage.Name = "columnPage";
      this.columnPage.Padding = new System.Windows.Forms.Padding(3);
      this.columnPage.Size = new System.Drawing.Size(151, 548);
      this.columnPage.TabIndex = 0;
      this.columnPage.Text = "Column";
      this.columnPage.UseVisualStyleBackColor = true;
      // 
      // columnPropertyGrid
      // 
      this.columnPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.columnPropertyGrid.Location = new System.Drawing.Point(3, 3);
      this.columnPropertyGrid.Name = "columnPropertyGrid";
      this.columnPropertyGrid.Size = new System.Drawing.Size(145, 542);
      this.columnPropertyGrid.TabIndex = 0;
      this.columnPropertyGrid.ToolbarVisible = false;
      this.columnPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
      // 
      // templatePage
      // 
      this.templatePage.Controls.Add(this.templatePropertyGrid);
      this.templatePage.Location = new System.Drawing.Point(4, 22);
      this.templatePage.Name = "templatePage";
      this.templatePage.Padding = new System.Windows.Forms.Padding(3);
      this.templatePage.Size = new System.Drawing.Size(151, 548);
      this.templatePage.TabIndex = 1;
      this.templatePage.Text = "Template";
      this.templatePage.UseVisualStyleBackColor = true;
      // 
      // templatePropertyGrid
      // 
      this.templatePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.templatePropertyGrid.Location = new System.Drawing.Point(3, 3);
      this.templatePropertyGrid.Name = "templatePropertyGrid";
      this.templatePropertyGrid.Size = new System.Drawing.Size(145, 542);
      this.templatePropertyGrid.TabIndex = 0;
      this.templatePropertyGrid.ToolbarVisible = false;
      // 
      // GridWrapper
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.toolStrip1);
      this.Name = "GridWrapper";
      this.Size = new System.Drawing.Size(849, 601);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.contextMenuStrip1.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
      this.tabControl1.ResumeLayout(false);
      this.columnPage.ResumeLayout(false);
      this.templatePage.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridView;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton xlsButton;
    private System.Windows.Forms.ToolStripButton xmlButton;
    private System.Windows.Forms.ToolStripButton toolStripButton1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripButton2;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.PropertyGrid columnPropertyGrid;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage columnPage;
    private System.Windows.Forms.TabPage templatePage;
    private System.Windows.Forms.PropertyGrid templatePropertyGrid;
  }
}
