//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 13.07.2007
// Time: 19:29
//
namespace ContentExtractor.Gui
{
	partial class UrlsListBox
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
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
      this.components = new System.ComponentModel.Container();
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
      this.label1 = new System.Windows.Forms.Label();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.delButton = new System.Windows.Forms.Button();
      this.downButton = new System.Windows.Forms.Button();
      this.upButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // listBox1
      // 
      this.listBox1.AllowDrop = true;
      this.listBox1.DataSource = this.bindingSource1;
      this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listBox1.FormattingEnabled = true;
      this.listBox1.Location = new System.Drawing.Point(0, 13);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new System.Drawing.Size(252, 277);
      this.listBox1.TabIndex = 0;
      this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListBox1DragDrop);
      this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListBox1DragEnter);
      this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
      // 
      // bindingSource1
      // 
      this.bindingSource1.CurrentItemChanged += new System.EventHandler(this.bindingSource1_CurrentItemChanged);
      // 
      // label1
      // 
      this.label1.Dock = System.Windows.Forms.DockStyle.Top;
      this.label1.Location = new System.Drawing.Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(252, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Drag file or URL here";
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel1.Controls.Add(this.delButton, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.downButton, 2, 0);
      this.tableLayoutPanel1.Controls.Add(this.upButton, 1, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 297);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(252, 30);
      this.tableLayoutPanel1.TabIndex = 4;
      // 
      // delButton
      // 
      this.delButton.Image = global::ContentExtractor.Gui.Properties.Resources.DeleteHS;
      this.delButton.Location = new System.Drawing.Point(3, 3);
      this.delButton.Name = "delButton";
      this.delButton.Size = new System.Drawing.Size(75, 23);
      this.delButton.TabIndex = 0;
      this.delButton.Text = "Delete";
      this.delButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.delButton.UseVisualStyleBackColor = true;
      this.delButton.Click += new System.EventHandler(this.delButton_Click);
      // 
      // downButton
      // 
      this.downButton.Image = global::ContentExtractor.Gui.Properties.Resources.down;
      this.downButton.Location = new System.Drawing.Point(171, 3);
      this.downButton.Name = "downButton";
      this.downButton.Size = new System.Drawing.Size(75, 23);
      this.downButton.TabIndex = 1;
      this.downButton.Text = "Down";
      this.downButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.downButton.UseVisualStyleBackColor = true;
      this.downButton.Click += new System.EventHandler(this.downButton_Click);
      // 
      // upButton
      // 
      this.upButton.Image = global::ContentExtractor.Gui.Properties.Resources.up;
      this.upButton.Location = new System.Drawing.Point(87, 3);
      this.upButton.Name = "upButton";
      this.upButton.Size = new System.Drawing.Size(75, 23);
      this.upButton.TabIndex = 2;
      this.upButton.Text = "Up";
      this.upButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.upButton.UseVisualStyleBackColor = true;
      this.upButton.Click += new System.EventHandler(this.upButton_Click);
      // 
      // UrlsListBox
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.listBox1);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "UrlsListBox";
      this.Size = new System.Drawing.Size(252, 327);
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }
		private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Button delButton;
    private System.Windows.Forms.Button downButton;
    private System.Windows.Forms.Button upButton;
    private System.Windows.Forms.BindingSource bindingSource1;
	}
}
