namespace ContentExtractor.Gui
{
  partial class ResultsView
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultsView));
      this.dataGrid = new System.Windows.Forms.DataGridView();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.rowsTextBox = new System.Windows.Forms.TextBox();
      this.columnTextBox = new System.Windows.Forms.TextBox();
      this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
      this.clearTemplateToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.addColumnToolStripButton = new System.Windows.Forms.ToolStripButton();
      this.saveResultButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
      this.deleteColumnToolStripButton = new System.Windows.Forms.ToolStripButton();
      ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
      this.toolStrip1.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // dataGrid
      // 
      this.dataGrid.AllowUserToAddRows = false;
      this.dataGrid.AllowUserToDeleteRows = false;
      this.dataGrid.AllowUserToOrderColumns = true;
      this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGrid.DefaultCellStyle = dataGridViewCellStyle1;
      this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGrid.Location = new System.Drawing.Point(0, 81);
      this.dataGrid.MultiSelect = false;
      this.dataGrid.Name = "dataGrid";
      this.dataGrid.ReadOnly = true;
      this.dataGrid.RowHeadersVisible = false;
      this.dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullColumnSelect;
      this.dataGrid.Size = new System.Drawing.Size(609, 282);
      this.dataGrid.TabIndex = 0;
      this.dataGrid.VirtualMode = true;
      this.dataGrid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGrid_CellValueNeeded);
      this.dataGrid.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGrid_ColumnAdded);
      // 
      // timer1
      // 
      this.timer1.Enabled = true;
      this.timer1.Interval = 500;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearTemplateToolStripButton,
            this.addColumnToolStripButton,
            this.deleteColumnToolStripButton,
            this.toolStripSeparator1,
            this.saveResultButton,
            this.toolStripSeparator2,
            this.toolStripButton1});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(609, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.rowsTextBox, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.columnTextBox, 1, 1);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(609, 56);
      this.tableLayoutPanel1.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(11, 7);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(66, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Rows XPath";
      // 
      // label2
      // 
      this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 35);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(74, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Column XPath";
      // 
      // rowsTextBox
      // 
      this.rowsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.rowsTextBox.Location = new System.Drawing.Point(83, 5);
      this.rowsTextBox.Name = "rowsTextBox";
      this.rowsTextBox.Size = new System.Drawing.Size(523, 20);
      this.rowsTextBox.TabIndex = 1;
      this.rowsTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.rowsTextBox_PreviewKeyDown);
      // 
      // columnTextBox
      // 
      this.columnTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.columnTextBox.Location = new System.Drawing.Point(83, 33);
      this.columnTextBox.Name = "columnTextBox";
      this.columnTextBox.Size = new System.Drawing.Size(523, 20);
      this.columnTextBox.TabIndex = 2;
      this.columnTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.columnTextBox_KeyDown);
      // 
      // saveFileDialog1
      // 
      this.saveFileDialog1.DefaultExt = "xml";
      this.saveFileDialog1.Filter = "Xml files|*.xml";
      // 
      // saveFileDialog2
      // 
      this.saveFileDialog2.DefaultExt = "cet";
      this.saveFileDialog2.Filter = "Content Extractor templates|*.cet";
      // 
      // clearTemplateToolStripButton
      // 
      this.clearTemplateToolStripButton.Image = global::ContentExtractor.Gui.Properties.Resources.DeleteHS;
      this.clearTemplateToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.clearTemplateToolStripButton.Name = "clearTemplateToolStripButton";
      this.clearTemplateToolStripButton.Size = new System.Drawing.Size(97, 22);
      this.clearTemplateToolStripButton.Text = "Clear template";
      this.clearTemplateToolStripButton.Click += new System.EventHandler(this.clearTemplateToolStripButton_Click);
      // 
      // addColumnToolStripButton
      // 
      this.addColumnToolStripButton.Image = global::ContentExtractor.Gui.Properties.Resources.AddTableHS;
      this.addColumnToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addColumnToolStripButton.Name = "addColumnToolStripButton";
      this.addColumnToolStripButton.Size = new System.Drawing.Size(82, 22);
      this.addColumnToolStripButton.Text = "Add column";
      this.addColumnToolStripButton.Click += new System.EventHandler(this.addColumnToolStripButton_Click);
      // 
      // saveResultButton
      // 
      this.saveResultButton.Image = ((System.Drawing.Image)(resources.GetObject("saveResultButton.Image")));
      this.saveResultButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.saveResultButton.Name = "saveResultButton";
      this.saveResultButton.Size = new System.Drawing.Size(81, 22);
      this.saveResultButton.Text = "Save result";
      this.saveResultButton.Click += new System.EventHandler(this.saveResultButton_Click);
      // 
      // toolStripButton1
      // 
      this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
      this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton1.Name = "toolStripButton1";
      this.toolStripButton1.Size = new System.Drawing.Size(98, 22);
      this.toolStripButton1.Text = "Save Template";
      this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
      // 
      // deleteColumnToolStripButton
      // 
      this.deleteColumnToolStripButton.Image = global::ContentExtractor.Gui.Properties.Resources.DelTableHS;
      this.deleteColumnToolStripButton.ImageTransparentColor = System.Drawing.Color.White;
      this.deleteColumnToolStripButton.Name = "deleteColumnToolStripButton";
      this.deleteColumnToolStripButton.Size = new System.Drawing.Size(94, 22);
      this.deleteColumnToolStripButton.Text = "Delete column";
      this.deleteColumnToolStripButton.Click += new System.EventHandler(this.deleteColumnToolStripButton_Click);
      // 
      // ResultsView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.dataGrid);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Controls.Add(this.toolStrip1);
      this.Name = "ResultsView";
      this.Size = new System.Drawing.Size(609, 363);
      ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGrid;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton clearTemplateToolStripButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox rowsTextBox;
    private System.Windows.Forms.TextBox columnTextBox;
    private System.Windows.Forms.ToolStripButton saveResultButton;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton toolStripButton1;
    private System.Windows.Forms.SaveFileDialog saveFileDialog2;
    private System.Windows.Forms.ToolStripButton addColumnToolStripButton;
    private System.Windows.Forms.ToolStripButton deleteColumnToolStripButton;
  }
}
