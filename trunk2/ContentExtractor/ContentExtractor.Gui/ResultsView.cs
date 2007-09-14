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
        rowXPathSynchro =
          new SynchronizedObject<string>(delegate { return this.state.Project.Template.RowXPath; },
          delegate(string value)
          {
            if (state.Project.Template.CheckRowXPath(value))
              this.state.Project.Template.RowXPath = value;
          });

        colXPathSynchro = new SynchronizedObject<string>(
            delegate
            {
              if (Utils.IsIndexOk(SelectedCellPoint.X, state.Project.Template.Columns))
                return this.state.Project.Template.Columns[SelectedCellPoint.X];
              return string.Empty;
            },
            delegate(string value)
            {
              if (Utils.IsIndexOk(SelectedCellPoint.X, state.Project.Template.Columns) &&
                state.Project.Template.CheckColumnXPath(value))
                this.state.Project.Template.Columns[SelectedCellPoint.X] = value;
            });

        components.Add(rowXPathSynchro);
        components.Add(colXPathSynchro);
        rowsTextBox.DataBindings.Add("Text", rowXPathSynchro, "Value");
        columnTextBox.DataBindings.Add("Text", colXPathSynchro, "Value");
      }
      else
      {
        //TODO: Should log to warning
        throw new InvalidOperationException("Cannot assign state twice");
      }
    }

    SynchronizedObject<string> rowXPathSynchro;
    SynchronizedObject<string> colXPathSynchro;

    private void timer1_Tick(object sender, EventArgs e)
    {
      RefreshGrid();
    }

    private XmlDocument resultDoc;

    private Point _selectedCellPoint;
    private Point SelectedCellPoint
    {
      get
      {
        _selectedCellPoint.X = Math.Max(0, Math.Min(dataGrid.Columns.Count, _selectedCellPoint.X));
        _selectedCellPoint.Y = Math.Max(0, Math.Min(dataGrid.RowCount, _selectedCellPoint.Y));
        return _selectedCellPoint;
      }
      set { this._selectedCellPoint = value; }
    }

    private void SetupColumn(DataGridViewColumn dgv_column, string column)
    {
      dgv_column.SortMode = DataGridViewColumnSortMode.NotSortable;
      dgv_column.HeaderText = column;
      dgv_column.Tag = column;
      dgv_column.DisplayIndex = dgv_column.Index;
    }

    private bool columnsOrderHasBeenChanged = false;

    private void SetupGridColumns(DataGridViewColumnCollection dgv_columns, List<string> columns)
    {
      int index;
      for (index = 0; index < Math.Min(dgv_columns.Count, columns.Count); index++)
        SetupColumn(dgv_columns[index], columns[index]);
      if (index >= dgv_columns.Count) // Several unadded columns left
      {
        for (; index < columns.Count; index++)
        {
          DataGridViewTextBoxColumn new_column = new DataGridViewTextBoxColumn();
          SetupColumn(new_column, columns[index]);
          dgv_columns.Add(new_column);
        }
      }
      else // Additional columns left in DataGridView
      {
        while (dgv_columns.Count > columns.Count)
          dgv_columns.RemoveAt(dgv_columns.Count - 1);
      }
      columnsOrderHasBeenChanged = false;
    }

    private void RefreshGrid()
    {
      if (!ColumnsAreSame())
        SetupGridColumns(dataGrid.Columns, state.Project.Template.Columns);

      List<XmlDocument> documents = state.Project.SourceUrls.ConvertAll<XmlDocument>(
          delegate(Uri u) { return state.GetXmlAsync(u); });
      resultDoc = state.Project.Template.Transform(documents);
      dataGrid.RowCount = resultDoc.DocumentElement.ChildNodes.Count;
      if (Utils.IsIndexOk(SelectedCellPoint.X, dataGrid.Columns) &&
         Utils.IsIndexOk(SelectedCellPoint.Y, dataGrid.Rows))
      {
        dataGrid.Rows[SelectedCellPoint.Y].Cells[SelectedCellPoint.X].Selected = true;
      }
    }

    private bool ColumnsAreSame()
    {
      Template template = state.Project.Template;
      bool columnsAreSame = dataGrid.ColumnCount == template.Columns.Count;
      if (columnsAreSame)
      {
        for (int i = 0; i < dataGrid.ColumnCount; i++)
        {
          //// If we still have the same order.
          //columnsAreSame &= dataGrid.Columns[i].Index == dataGrid.Columns[i].DisplayIndex;
          // If column values stay the same.
          columnsAreSame &= object.Equals(dataGrid.Columns[i].Tag, template.Columns[i]);
        }
      }
      return columnsAreSame;
    }

    private void clearTemplateToolStripButton_Click(object sender, EventArgs e)
    {
      state.Project.Template = new Template();
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
      if (Utils.IsIndexOk(SelectedCellPoint.X, state.Project.Template.Columns))
      {
        state.Project.Template.Columns.RemoveAt(SelectedCellPoint.X);
      }
    }

    const int kDefaultColumnWidth = 150;

    private void dataGrid_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
    {
      e.Column.Width = kDefaultColumnWidth;
    }

    private void dataGrid_SelectionChanged(object sender, EventArgs e)
    {
      if (dataGrid.SelectedCells.Count > 0)
      {
        SelectedCellPoint = new Point(dataGrid.SelectedCells[0].ColumnIndex,
          dataGrid.SelectedCells[0].RowIndex);
      }
      else
        SelectedCellPoint = new Point(0, 0);
    }

    private void dataGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      XmlNode row = resultDoc.DocumentElement.ChildNodes[e.RowIndex];
      string value = string.Empty;
      if (row != null)
      {
        XmlNode cell = row.ChildNodes[e.ColumnIndex];
        if (cell != null)
          value = cell.InnerXml;
      }
      e.Value = value;
    }

    private void rowsTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        if (state.Project.Template.CheckRowXPath(rowsTextBox.Text))
        {
          rowsTextBox.DataBindings["Text"].WriteValue();
        }
        else
        {
          MessageBox.Show(
            string.Format(Properties.Resources.CantApplyRowXPathWarning, rowsTextBox.Text),
            Properties.Resources.CantApplyRowXPathWarningCaption,
            MessageBoxButtons.OK);
        }
      }
    }

    private void columnTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        if (state.Project.Template.CheckRowXPath(columnTextBox.Text))
        {
          columnTextBox.DataBindings["Text"].WriteValue();
        }
        else
        {
          MessageBox.Show(
            string.Format(Properties.Resources.CantApplyColumnXPathWarning, columnTextBox.Text),
            Properties.Resources.CantApplyColumnXPathWarningCaption,
            MessageBoxButtons.OK);
        }
      }
    }

    private void dataGrid_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
    {
      int left = e.Column.Index;
      int right = e.Column.DisplayIndex;

      if (left != right && !columnsOrderHasBeenChanged)
      {
        string left_col = state.Project.Template.Columns[left];
        state.Project.Template.Columns.RemoveAt(left);
        state.Project.Template.Columns.Insert(right, left_col);

        // SetupGridColumns will be called after this. It'll fix wrong DisplayIndex sequences
        // and make it sorted.
        columnsOrderHasBeenChanged = true;
      }

    }

    /// <summary>
    /// Provides "select column" functionality in case grid doesn't have any row.
    /// </summary>
    private void dataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (Utils.IsIndexOk(e.ColumnIndex, dataGrid.Columns))
      {
        SelectedCellPoint = new Point(e.ColumnIndex, SelectedCellPoint.Y);
      }
    }
  }
}
