//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 06.07.2007
// Time: 19:22
//
namespace ContentExtractor.Gui
{
	partial class MarkingBrowser
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MarkingBrowser));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.backBtn = new System.Windows.Forms.ToolStripButton();
			this.fwdBtn = new System.Windows.Forms.ToolStripButton();
			this.goBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
			this.webBrowser1 = new ContentExtractor.Core.ExtendedWebBrowser();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.backBtn,
									this.fwdBtn,
									this.goBtn,
									this.toolStripComboBox1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(469, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// backBtn
			// 
			this.backBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.backBtn.Image = ((System.Drawing.Image)(resources.GetObject("backBtn.Image")));
			this.backBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.backBtn.Name = "backBtn";
			this.backBtn.Size = new System.Drawing.Size(33, 22);
			this.backBtn.Text = "Back";
			// 
			// fwdBtn
			// 
			this.fwdBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.fwdBtn.Image = ((System.Drawing.Image)(resources.GetObject("fwdBtn.Image")));
			this.fwdBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.fwdBtn.Name = "fwdBtn";
			this.fwdBtn.Size = new System.Drawing.Size(31, 22);
			this.fwdBtn.Text = "Fwd";
			// 
			// goBtn
			// 
			this.goBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.goBtn.Image = ((System.Drawing.Image)(resources.GetObject("goBtn.Image")));
			this.goBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.goBtn.Name = "goBtn";
			this.goBtn.Size = new System.Drawing.Size(24, 22);
			this.goBtn.Text = "Go";
			// 
			// toolStripComboBox1
			// 
			this.toolStripComboBox1.Name = "toolStripComboBox1";
			this.toolStripComboBox1.Size = new System.Drawing.Size(200, 25);
			// 
			// webBrowser1
			// 
			this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser1.Location = new System.Drawing.Point(0, 25);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size(469, 370);
			this.webBrowser1.TabIndex = 1;
			// 
			// MarkingBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.webBrowser1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MarkingBrowser";
			this.Size = new System.Drawing.Size(469, 395);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private ContentExtractor.Core.ExtendedWebBrowser webBrowser1;
		private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
		private System.Windows.Forms.ToolStripButton goBtn;
		private System.Windows.Forms.ToolStripButton fwdBtn;
		private System.Windows.Forms.ToolStripButton backBtn;
		private System.Windows.Forms.ToolStrip toolStrip1;
	}
}
