using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ContentExtractor.Core;
using MetaTech.Library;
using SoftTech.Gui;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace ContentExtractor.Gui
{
  public partial class GridWrapper : UserControl, IView
  {
    public GridWrapper()
    {
      InitializeComponent();

      synchro = new DataGridViewSynchronizer(dataGridView);
      synchro.ColumnsConfigAutoSaveEnabled = false;
      synchro.ContextMenuEnabled = false;
      synchro.ApplyDisplayIndexOnNewColumns = false;
      synchro.UpdateInterval = TimeSpan.FromSeconds(0.5);
      synchro.DataLink = delegate
      {
        return DataTemplate.Apply(GetModel().Template, PreFixTemplate, GetModel().SourceTree);
      };
      synchro.Browser = new Templates.Browser();
      CellMarkers.SupportBackColor(synchro);
      synchro.ResetColumnConfigToDefault();

      dataGridView.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView_CellPainting);
    }
    private DataGridViewSynchronizer synchro;

    void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
      DataGridView view = (DataGridView)sender;
      view.ShowEditingIcon = false;
      if (e.RowIndex >= 0 && e.ColumnIndex == 0)
      {
        view.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 1).ToString();
      }
    }

    public DataGridView DataGrid
    {
      get { return dataGridView; }
    }

    DataGridView.HitTestInfo GetHitTestInfo(DragEventArgs e)
    {
      Point clientPoint = this.dataGridView.PointToClient(new Point(e.X, e.Y));
      return this.dataGridView.HitTest(clientPoint.X, clientPoint.Y);
    }

    FixPoint preFixPoint = null;
    private class FixPoint
    {
      public int Column;
      public int Row;
      public string XPath;

      public FixPoint() { }
      public FixPoint(int row, int col, string xpath)
      {
        this.Row = row;
        this.Column = col;
        this.XPath = xpath;
      }
    }

    private DataTemplate PreFixTemplate
    {
      get
      {
        if (Control.MouseButtons != MouseButtons.Left || preFixPoint == null)
          return null;
        else
        {
          DataTemplate t = Model.Clone<DataTemplate>(GetModel().Template);
          t.AddXPathColumnToPosition(preFixPoint.Row, preFixPoint.Column, preFixPoint.XPath);
          return t;
        }
      }
    }

    private void dataGridView_DragOver(object sender, DragEventArgs e)
    {
      string node = WebExtractorHlp.ExtractDragData(e.Data);
      if (node != null)
      {
        DataGridView.HitTestInfo info = GetHitTestInfo(e);
        if (preFixPoint == null || preFixPoint.Column != info.ColumnIndex || preFixPoint.Column != info.RowIndex)
        {
          preFixPoint = new FixPoint(info.RowIndex, info.ColumnIndex, node);
        }
      }
    }

    private void dataGridView_DragLeave(object sender, EventArgs e)
    {
      preFixPoint = null;
    }

    private void dataGridView_DragDrop(object sender, DragEventArgs e)
    {
      preFixPoint = null;
      ReceiveXmlNode((string)e.Data.GetData(typeof(string)), GetHitTestInfo(e));
      synchro.ForceSynchronize();
      dataGridView.Invalidate(true);
    }

    public void ReceiveXmlNode(string node, DataGridView.HitTestInfo info)
    {
      if (info.Type == DataGridViewHitTestType.Cell)
      {
        DataTemplate template = Model.Clone<DataTemplate>(GetModel().Template);
        template.AddXPathColumnToPosition(info.RowIndex, info.ColumnIndex, node);
        GetModel().Template = template;
      }
    }

    private void dataGridView_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = e.AllowedEffect;
    }

    public void ForceGridSynchronizer()
    {
      synchro.ForceSynchronize();
    }

    Getter<Model> GetModel = delegate { return new Model(); };

    #region IView Members

    public void InitModel(Getter<Model> modelGetter)
    {
      this.GetModel = modelGetter;
      templatePropertyGrid.SelectedObject = new TemplateProperties(delegate { return GetModel().Template; });
    }

    List<DataGridViewCell> selectedCellsMemory = null;

    public void ForceSynchronize()
    {
      this.Enabled = GetModel().Mode == Model.WorkMode.Parse;
      dataGridView.Visible = this.Enabled;
      if (!this.Enabled)
      {
        return;
      }
      SyncPropertyGrid();
      SyncGrid();

      if (PreFixTemplate != null)
      {
        if (selectedCellsMemory == null)
        {
          selectedCellsMemory = CollectionHlp.From<DataGridViewCell>(dataGridView.SelectedCells);
          foreach (DataGridViewCell cell in selectedCellsMemory)
            cell.Selected = false;
        }
      }
      else if (selectedCellsMemory != null)
      {
        foreach (DataGridViewCell cell in selectedCellsMemory)
          cell.Selected = true;
        selectedCellsMemory = null;
      }
    }

    private void SyncGrid()
    {
      foreach (DataGridViewColumn c in dataGridView.Columns)
      {
        if (c.ContextMenuStrip != contextMenuStrip1)
        {
          c.HeaderCell.ContextMenuStrip = contextMenuStrip1;
        }
        //IDataColumn dColumn = null;
        //if (0 <= c.Index && c.Index < GetModel().Template.Columns.Count)
        //  dColumn = GetModel().Template.Columns[c.Index];
        //if (!object.Equals(c.Tag, dColumn))
        //  c.Tag = dColumn;
      }
    }

    private void SyncPropertyGrid()
    {
      int columnIndex = SelectedColumnIndex;
      if (0 <= columnIndex && columnIndex < GetModel().Template.Columns.Count)
      {
        if (!object.Equals(columnPropertyGrid.SelectedObject, GetModel().Template.Columns[columnIndex]))
          columnPropertyGrid.SelectedObject = GetModel().Template.Columns[columnIndex];
        return;
      }
      columnPropertyGrid.SelectedObject = null;
    }

    #endregion

    private int SelectedColumnIndex
    {
      get
      {
        int columnIndex = -1;
        foreach (DataGridViewCell cell in dataGridView.SelectedCells)
          columnIndex = cell.ColumnIndex;
        return columnIndex;
      }
    }

    private void xlsButton_Click(object sender, EventArgs e)
    {
      saveFileDialog1.Filter = "Excel files|*.xml";
      if (saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        string toWrite = DataExport.ExportToExcelML(GetModel().Result);
        File.WriteAllText(saveFileDialog1.FileName, toWrite);
        //DataGridExport.ExportToExcelML(saveFileDialog1.FileName, synchro);
      }
    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
      GetModel().Template = new DataTemplate();
    }

    private void xmlButton_Click(object sender, EventArgs e)
    {
      saveFileDialog1.Filter = "Xml files|*.xml";
      if (saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        XmlDocument document = GetModel().Result;
        document.Save(saveFileDialog1.FileName);
      }
    }

    private void toolStripButton2_Click(object sender, EventArgs e)
    {
      saveFileDialog1.Filter = "Html files|*.htm,*.html";
      if (saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        XmlDocument document = GetModel().Result;
        DataExport.ExportToHtml(document);
      }
    }

    private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
    {
      //int index = SelectedColumnIndex;
      //if (0 <= index && index < GetModel().Template.Columns.Count)
      //{
      //  GetModel().Template.Columns[index] = (IDataColumn)propertyGrid1.SelectedObject;
      //}
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (0 <= SelectedColumnIndex && SelectedColumnIndex < GetModel().Template.Columns.Count)
      {
        GetModel().Template.Columns.RemoveAt(SelectedColumnIndex);
        dataGridView.Columns[SelectedColumnIndex].Selected = false;
      }
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      deleteToolStripMenuItem.Enabled = (0 <= SelectedColumnIndex && SelectedColumnIndex < GetModel().Template.Columns.Count);
    }

    private void dataGridView_MouseDown(object sender, MouseEventArgs e)
    {
      DataGridView.HitTestInfo info = dataGridView.HitTest(e.X, e.Y);
      if (info.RowIndex == -1 && 0 <= info.ColumnIndex && info.ColumnIndex < dataGridView.Columns.Count)
      {
        foreach (DataGridViewCell cell in dataGridView.SelectedCells)
          cell.Selected = false;
        dataGridView.Columns[info.ColumnIndex].Selected = true;
      }
    }

    private void cutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (0 <= SelectedColumnIndex && SelectedColumnIndex < GetModel().Template.Columns.Count)
        columnsCopyPasteInfo.Cut(GetModel().Template.Columns[SelectedColumnIndex]);
    }
    CutCopyPasteInfo<IDataColumn> columnsCopyPasteInfo = new CutCopyPasteInfo<IDataColumn>();

    private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!columnsCopyPasteInfo.CacheEmpty)
      {
        if (columnsCopyPasteInfo.IsCut)
          GetModel().Template.Columns.Remove(columnsCopyPasteInfo.CachedValue);

        int insertIndex = Math.Max(0, SelectedColumnIndex);
        insertIndex = Math.Min(GetModel().Template.Columns.Count, insertIndex);
        GetModel().Template.Columns.Insert(insertIndex, Model.Clone(columnsCopyPasteInfo.CachedValue));
        columnsCopyPasteInfo.Clear();
      }
    }

    private void copyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (0 <= SelectedColumnIndex && SelectedColumnIndex < GetModel().Template.Columns.Count)
        columnsCopyPasteInfo.Copy(GetModel().Template.Columns[SelectedColumnIndex]);
    }
  }

  public class TemplateProperties
  {
    public TemplateProperties(Getter<DataTemplate> templateGetter)
    {
      this.templateGetter = templateGetter;
    }
    private Getter<DataTemplate> templateGetter;
    private DataTemplate Template
    {
      get { return templateGetter(); }
    }

    private XPathDataRowRule XPathRule
    {
      get
      {
        return (XPathDataRowRule)Template.Rules.Find(delegate(IDataRowRule r) { return r is XPathDataRowRule; });
      }
    }

    [Category("Template properties")]
    [DisplayName("Row XPath")]
    public string RowXPath
    {
      get
      {
        if (XPathRule != null)
          return XPathRule.RowsXPath;
        else
          return string.Empty;
      }
      set
      {
        if (XPathRule == null)
          Template.Rules.Add(new XPathDataRowRule());
        XPathRule.RowsXPath = value;
      }
    }

    [Category("Template properties")]
    [DisplayName("Show column names")]
    public bool ShowNames
    {
      get
      {
        return Template.Rules.FindIndex(delegate(IDataRowRule r) { return r is NamesDataRowRule; }) != -1;
      }
      set
      {
        if (value == false)
        {
          Template.Rules.RemoveAll(delegate(IDataRowRule r) { return r is NamesDataRowRule; });
        }
        else if (!ShowNames)
        {
          Template.Rules.Insert(0, new NamesDataRowRule());
        }

      }
    }
  }

  public class CutCopyPasteInfo<T>
  {
    public CutCopyPasteInfo() { }
    public CutCopyPasteInfo(T defaultValue)
      : this()
    {
      this.defaultValue = defaultValue;
      cached = this.defaultValue;
    }

    T defaultValue = default(T);

    public T CachedValue
    {
      get
      {
        return cached;
      }
    }
    private T cached;

    private bool isCut = false;

    public bool CacheEmpty
    {
      get
      {
        return object.Equals(cached, defaultValue);
      }
    }

    public bool IsCut { get { return isCut; } }

    public void Clear()
    {
      cached = defaultValue;
    }

    public void Cut(T value)
    {
      cached = value;
      isCut = true;
    }
    public void Copy(T value)
    {
      cached = value;
      isCut = false;
    }

  }

}
