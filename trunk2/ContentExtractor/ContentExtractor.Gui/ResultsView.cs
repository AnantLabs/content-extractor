using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ContentExtractor.Core;

namespace ContentExtractor.Gui
{
  public partial class ResultsView : UserControl
  {
    public ResultsView()
    {
      InitializeComponent();

      //dataGrid.AutoGenerateColumns = true;
      //bindingSource.DataSource = new CustomList();
    }

    private State state;

    public void SetState(State state)
    {
      if (this.state == null)
      {
        this.state = state;
        //rows = new ResultRows(state);
        //bindingSource.DataSource = rows;
      }
      else
        //TODO: Should log to warning
        throw new InvalidOperationException("Cannot assign state twice");
    }
    //private ResultRows rows;

    private void timer1_Tick(object sender, EventArgs e)
    {
      RefreshGrid();
      RefreshXPaths();
    }

    private XmlDocument resultDoc;

    private void RefreshGrid()
    {
      if (!ColumnsAreSame())
      {
        Template template = state.Project.Template;
        dataGrid.Columns.Clear();
        foreach (string colXpath in template.Columns)
        {
          DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
          column.SortMode = DataGridViewColumnSortMode.NotSortable;
          column.HeaderText = colXpath;
          column.Tag = colXpath;
          dataGrid.Columns.Add(column);
        }
      }
      List<XmlDocument> documents = state.Project.SourceUrls.ConvertAll<XmlDocument>(
          delegate(Uri u) { return state.GetXmlAsync(u); });
      resultDoc = state.Project.Template.Transform(documents);
      dataGrid.RowCount = resultDoc.DocumentElement.ChildNodes.Count;
    }

    private bool ColumnsAreSame()
    {
      Template template = state.Project.Template;
      bool columnsAreSame = dataGrid.ColumnCount == template.Columns.Count;
      if (columnsAreSame)
      {
        for (int i = 0; i < dataGrid.ColumnCount; i++)
          columnsAreSame &= object.Equals(dataGrid.Columns[i].Tag, template.Columns[i]);
      }
      return columnsAreSame;
    }

    private void clearTemplateToolStripButton_Click(object sender, EventArgs e)
    {
      state.Project.Template = new Template();
    }

    private void dataGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      XmlNode row = resultDoc.DocumentElement.ChildNodes[e.RowIndex];
      XmlNode cell = row.ChildNodes[e.ColumnIndex];
      e.Value = cell != null ? cell.InnerXml : "";
    }

    private int lastColumnIndex = -1;

    private void RefreshXPaths()
    {
      if (!rowsTextBox.Focused && state.Project.Template.RowXPath != rowsTextBox.Text)
      {
        rowsTextBox.Text = state.Project.Template.RowXPath;
      }
      if (!columnTextBox.Focused &&
        dataGrid.SelectedColumns.Count > 0 &&
        lastColumnIndex != dataGrid.SelectedColumns[0].Index)
      {
        lastColumnIndex = dataGrid.SelectedColumns[0].Index;
        columnTextBox.Text = state.Project.Template.Columns[lastColumnIndex];
      }
    }

    private void rowsTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        string xpath = rowsTextBox.Text;
        if (state.Project.Template.CheckRowXPath(xpath))
          state.Project.Template.RowXPath = xpath;
        else
          MessageBox.Show(
            string.Format("Can't apply xpath: '{0}' to template rows", xpath),
            "XPath error");
      }
    }

    private void columnTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        string xpath = columnTextBox.Text;
        if (state.Project.Template.CheckColumnXPath(xpath))
          state.Project.Template.Columns[lastColumnIndex] = xpath;
        else
          MessageBox.Show(
            string.Format("Can't apply xpath: '{0}' to column", xpath),
            "XPath error");
      }
    }

    private void saveResultButton_Click(object sender, EventArgs e)
    {
      if (saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        resultDoc.Save(saveFileDialog1.FileName);
      }
    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
      if (saveFileDialog2.ShowDialog() == DialogResult.OK)
      {
        ScrapingProject.SaveProject(saveFileDialog2.FileName, state.Project);
      }
    }

    private void addColumnToolStripButton_Click(object sender, EventArgs e)
    {
      state.Project.Template.AddEmptyColumn();
    }

    private void deleteColumnToolStripButton_Click(object sender, EventArgs e)
    {
      if (Utils.IsIndexOk(lastColumnIndex, state.Project.Template.Columns))
      {
        state.Project.Template.Columns.RemoveAt(lastColumnIndex);
      }
    }

    const int kDefaultColumnWidth = 150;

    private void dataGrid_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
    {
      e.Column.Width = kDefaultColumnWidth;
    }

  }
}
