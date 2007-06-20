namespace ContentExtractor.Gui
{
  partial class PagesPanel
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PagesPanel));
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addFilesToListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteFromListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.viewInBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.button1 = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.toolStrip2 = new System.Windows.Forms.ToolStrip();
      this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
      this.addLinkedPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addLinkedPageRecursivlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
      this.deleteButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.moveUpButton = new System.Windows.Forms.ToolStripButton();
      this.moveDownButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.viewUrlButton = new System.Windows.Forms.ToolStripButton();
      this.label1 = new System.Windows.Forms.Label();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
      this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.contextMenuStrip1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.toolStrip2.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // listBox1
      // 
      this.listBox1.AllowDrop = true;
      this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
      this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listBox1.FormattingEnabled = true;
      this.listBox1.HorizontalScrollbar = true;
      this.listBox1.Location = new System.Drawing.Point(0, 63);
      this.listBox1.Name = "listBox1";
      this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.listBox1.Size = new System.Drawing.Size(430, 524);
      this.listBox1.TabIndex = 0;
      this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
      this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
      this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
      this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFilesToListToolStripMenuItem,
            this.deleteFromListToolStripMenuItem,
            this.toolStripMenuItem1,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripMenuItem2,
            this.viewInBrowserToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(168, 148);
      // 
      // addFilesToListToolStripMenuItem
      // 
      this.addFilesToListToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.AddFromFile;
      this.addFilesToListToolStripMenuItem.Name = "addFilesToListToolStripMenuItem";
      this.addFilesToListToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.addFilesToListToolStripMenuItem.Text = "Add files to list...";
      this.addFilesToListToolStripMenuItem.Click += new System.EventHandler(this.addFilesToList_Click);
      // 
      // deleteFromListToolStripMenuItem
      // 
      this.deleteFromListToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.delete;
      this.deleteFromListToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.deleteFromListToolStripMenuItem.Name = "deleteFromListToolStripMenuItem";
      this.deleteFromListToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.deleteFromListToolStripMenuItem.Text = "Delete from list";
      this.deleteFromListToolStripMenuItem.Click += new System.EventHandler(this.deleteButton_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(164, 6);
      // 
      // moveUpToolStripMenuItem
      // 
      this.moveUpToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.up;
      this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
      this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.moveUpToolStripMenuItem.Text = "Move up";
      this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpButton_Click);
      // 
      // moveDownToolStripMenuItem
      // 
      this.moveDownToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.down;
      this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
      this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.moveDownToolStripMenuItem.Text = "Move down";
      this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownButton_Click);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(164, 6);
      // 
      // viewInBrowserToolStripMenuItem
      // 
      this.viewInBrowserToolStripMenuItem.Image = global::ContentExtractor.Gui.Properties.Resources.SearchWebHS;
      this.viewInBrowserToolStripMenuItem.Name = "viewInBrowserToolStripMenuItem";
      this.viewInBrowserToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.viewInBrowserToolStripMenuItem.Text = "View in browser";
      this.viewInBrowserToolStripMenuItem.Click += new System.EventHandler(this.viewUrlButton_Click);
      // 
      // toolTip1
      // 
      this.toolTip1.IsBalloon = true;
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.Image = global::ContentExtractor.Gui.Properties.Resources.go;
      this.button1.Location = new System.Drawing.Point(398, 37);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(29, 23);
      this.button1.TabIndex = 3;
      this.toolTip1.SetToolTip(this.button1, "Add URL to list");
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.toolStrip2);
      this.panel1.Controls.Add(this.button1);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Controls.Add(this.comboBox1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(430, 63);
      this.panel1.TabIndex = 1;
      // 
      // toolStrip2
      // 
      this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1,
            this.toolStripButton2,
            this.deleteButton,
            this.toolStripSeparator1,
            this.moveUpButton,
            this.moveDownButton,
            this.toolStripSeparator2,
            this.viewUrlButton});
      this.toolStrip2.Location = new System.Drawing.Point(0, 0);
      this.toolStrip2.Name = "toolStrip2";
      this.toolStrip2.Size = new System.Drawing.Size(430, 25);
      this.toolStrip2.TabIndex = 4;
      this.toolStrip2.Text = "toolStrip2";
      // 
      // toolStripSplitButton1
      // 
      this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLinkedPageToolStripMenuItem,
            this.addLinkedPageRecursivlyToolStripMenuItem});
      this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
      this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripSplitButton1.Name = "toolStripSplitButton1";
      this.toolStripSplitButton1.Size = new System.Drawing.Size(115, 22);
      this.toolStripSplitButton1.Text = "Add linked page";
      this.toolStripSplitButton1.Visible = false;
      this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
      // 
      // addLinkedPageToolStripMenuItem
      // 
      this.addLinkedPageToolStripMenuItem.Name = "addLinkedPageToolStripMenuItem";
      this.addLinkedPageToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
      this.addLinkedPageToolStripMenuItem.Text = "Add linked page";
      this.addLinkedPageToolStripMenuItem.Click += new System.EventHandler(this.addLinkedPageToolStripMenuItem_Click);
      // 
      // addLinkedPageRecursivlyToolStripMenuItem
      // 
      this.addLinkedPageRecursivlyToolStripMenuItem.Name = "addLinkedPageRecursivlyToolStripMenuItem";
      this.addLinkedPageRecursivlyToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
      this.addLinkedPageRecursivlyToolStripMenuItem.Text = "Add linked page recursivly";
      this.addLinkedPageRecursivlyToolStripMenuItem.Click += new System.EventHandler(this.addLinkedPageRecursivlyToolStripMenuItem_Click);
      // 
      // toolStripButton2
      // 
      this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButton2.Image = global::ContentExtractor.Gui.Properties.Resources.AddFromFile;
      this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton2.Name = "toolStripButton2";
      this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
      this.toolStripButton2.Text = "Add files to list";
      this.toolStripButton2.Click += new System.EventHandler(this.addFilesToList_Click);
      // 
      // deleteButton
      // 
      this.deleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.deleteButton.Image = global::ContentExtractor.Gui.Properties.Resources.delete;
      this.deleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.deleteButton.Name = "deleteButton";
      this.deleteButton.Size = new System.Drawing.Size(23, 22);
      this.deleteButton.Text = "Delete selected";
      this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // moveUpButton
      // 
      this.moveUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.moveUpButton.Enabled = false;
      this.moveUpButton.Image = global::ContentExtractor.Gui.Properties.Resources.up;
      this.moveUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.moveUpButton.Name = "moveUpButton";
      this.moveUpButton.Size = new System.Drawing.Size(23, 22);
      this.moveUpButton.Text = "Move up";
      this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
      // 
      // moveDownButton
      // 
      this.moveDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.moveDownButton.Enabled = false;
      this.moveDownButton.Image = global::ContentExtractor.Gui.Properties.Resources.down;
      this.moveDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.moveDownButton.Name = "moveDownButton";
      this.moveDownButton.Size = new System.Drawing.Size(23, 22);
      this.moveDownButton.Text = "Move down";
      this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // viewUrlButton
      // 
      this.viewUrlButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.viewUrlButton.Image = global::ContentExtractor.Gui.Properties.Resources.SearchWebHS;
      this.viewUrlButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.viewUrlButton.Name = "viewUrlButton";
      this.viewUrlButton.Size = new System.Drawing.Size(23, 22);
      this.viewUrlButton.Text = "View URL in browser";
      this.viewUrlButton.Click += new System.EventHandler(this.viewUrlButton_Click);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 40);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(54, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Add URL:";
      // 
      // comboBox1
      // 
      this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBox1.Items.AddRange(new object[] {
            "www.yandex.ru",
            "C:\\Documents and Settings\\Сергей\\Рабочий стол\\Incoming\\ReGet\\Яндекс.htm",
            "www.download.com/3101-20-0-1.html?tag=pop",
            "demo.sugarondemand.com/sugarcrm_os/index.php?action=Login&module=Users",
            "www.stat.ru/cgi-bin/awstats.pl?config=www.implart.com",
            "www.sugar.ru",
            "www.increase.ru"});
      this.comboBox1.Location = new System.Drawing.Point(63, 37);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(329, 21);
      this.comboBox1.TabIndex = 1;
      this.comboBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripButton1});
      this.toolStrip1.Location = new System.Drawing.Point(0, 563);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(430, 25);
      this.toolStrip1.TabIndex = 2;
      this.toolStrip1.Text = "toolStrip1";
      this.toolStrip1.Visible = false;
      // 
      // toolStripLabel1
      // 
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new System.Drawing.Size(104, 22);
      this.toolStripLabel1.Text = "Pages are loading...";
      // 
      // toolStripButton1
      // 
      this.toolStripButton1.AutoToolTip = false;
      this.toolStripButton1.Image = global::ContentExtractor.Gui.Properties.Resources.stop;
      this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButton1.Name = "toolStripButton1";
      this.toolStripButton1.Size = new System.Drawing.Size(49, 22);
      this.toolStripButton1.Text = "Stop";
      this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.DefaultExt = "htm";
      this.openFileDialog1.FileName = "openFileDialog1";
      this.openFileDialog1.Filter = "Html files(*.htm;*.html)|*.htm;*.html";
      this.openFileDialog1.Multiselect = true;
      this.openFileDialog1.Title = "Add files to list";
      // 
      // PagesPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.listBox1);
      this.Controls.Add(this.panel1);
      this.Name = "PagesPanel";
      this.Size = new System.Drawing.Size(430, 588);
      this.contextMenuStrip1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.toolStrip2.ResumeLayout(false);
      this.toolStrip2.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    private System.Windows.Forms.ToolStripButton toolStripButton1;
    private System.Windows.Forms.ToolStrip toolStrip2;
    private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
    private System.Windows.Forms.ToolStripMenuItem addLinkedPageToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addLinkedPageRecursivlyToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton moveDownButton;
    private System.Windows.Forms.ToolStripButton moveUpButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton deleteButton;
    private System.Windows.Forms.ToolStripButton toolStripButton2;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton viewUrlButton;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem addFilesToListToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteFromListToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem viewInBrowserToolStripMenuItem;
  }
}
