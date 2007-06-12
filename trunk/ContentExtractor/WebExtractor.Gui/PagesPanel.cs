using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MetaTech.Library;
using ContentExtractor.Core;
using System.Xml;
using System.Xml.XPath;

namespace ContentExtractor.Gui
{
  public partial class PagesPanel : UserControl, IView
  {
    public PagesPanel()
    {
      InitializeComponent();
    }

    private List<WebPosition.PersistStruct> cachedPositions = new List<WebPosition.PersistStruct>();
    private List<int> selectedIndeces = new List<int>();

    Getter<Model> GetModel = delegate { return new Model(); };

    private Model CurModel
    {
      get
      { return GetModel(); }
    }

    #region IView Members

    public void InitModel(Getter<Model> modelGetter)
    {
      this.GetModel = modelGetter;
    }

    public void ForceSynchronize()
    {
      button1.Enabled = WebPosition.Parse(comboBox1.Text) != null;
      List<WebPosition.PersistStruct> currentList = GetModel().PositionsList.ConvertAll<WebPosition.PersistStruct>(WebPosition.GetPersist);

      if (!WebExtractorHlp.CompareList(cachedPositions, currentList))
      {
        listBox1.Items.Clear();
        foreach (WebPosition.PersistStruct pos in currentList)
          listBox1.Items.Add(pos.Url);
        listBox1.SelectedIndices.Clear();
        foreach (int index in selectedIndeces)
        {
          int corrIndex = currentList.FindIndex(delegate(WebPosition.PersistStruct p) { return object.Equals(p, cachedPositions[index]); });
          if (corrIndex >= 0)
            listBox1.SelectedIndices.Add(corrIndex);
        }
        cachedPositions = WebExtractorHlp.CopyList(currentList);
      }
      if (!WebExtractorHlp.CompareList(selectedIndeces, _.From<int>(listBox1.SelectedIndices)))
        selectedIndeces = _.From<int>(listBox1.SelectedIndices);

      toolStrip1.Visible = lastLoadedPosition != null;
      if (lastLoadedPosition != null)
      {
        LoadIteratedPosition();
      }
      SynchronizeToolStrip();
    }
    #endregion

    private bool AddPosition(string adress)
    {
      WebPosition pos = WebPosition.Parse(adress);
      if (pos != null)
      {
        CurModel.PositionsList.Add(pos);
        return true;
      }
      else
        return false;
    }

    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        if (AddPosition(comboBox1.Text))
          comboBox1.Text = "";
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (AddPosition(comboBox1.Text))
        comboBox1.Text = "";
    }

    private void listBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete)
      {
        DeleteSelected();
      }
    }

    private void DeleteSelected()
    {
      List<int> selected = CollectionHlp.From<int>(listBox1.SelectedIndices);
      for (int index = CurModel.PositionsList.Count - 1; index >= 0; index--)
      {
        if (listBox1.SelectedIndices.Contains(index))
          CurModel.PositionsList.RemoveAt(index);
      }
      listBox1.SelectedIndices.Clear();
    }

    private void listBox1_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = e.AllowedEffect;
    }

    private void listBox1_DragDrop(object sender, DragEventArgs e)
    {
      string[] Formats = e.Data.GetFormats();
      List<string> links = new List<string>();
      object obData = e.Data.GetData(DataFormats.Text);
      if (obData != null && obData is string)
        links.Add((string)obData);
      else
      {
        obData = e.Data.GetData(DataFormats.FileDrop);
        if (obData is string[])
          links.AddRange((string[])obData);
      }

      foreach (string link in links)
      {
        WebPosition position = WebPosition.Parse(link);
        if (position != null)
          GetModel().PositionsList.Add(position);
        else
        {
          WebPosition refPosition = GetModel().ActivePosition.GetLinkedPosition(link);
          if (refPosition != null)
            GetModel().PositionsList.Add(refPosition);
        }
      }
    }

    private WebPosition lastLoadedPosition = null;
    private string loadPath = null;

    private void ReceiveXmlNode(string path)
    {
      lastLoadedPosition = new WebPosition();
      lastLoadedPosition.Persist = GetModel().ActivePosition.Persist;
      loadPath = path;
    }

    #region ToolStrip Buttons
    private void LoadIteratedPosition()
    {
      if (!string.IsNullOrEmpty(lastLoadedPosition.DocumentText))
      {
        XPathNavigator node = XmlHlp.SelectSingleNode(lastLoadedPosition.XPathNavigable, loadPath);

        if (node != null)
        {
          string[] pathes = new string[] { "@href", "*/@href", "../@href", "*/*/@href", "../../@href" };
          foreach (string path in pathes)
          {
            XPathNavigator attr = node.SelectSingleNode(path);
            if (attr.NodeType == XPathNodeType.Attribute)
            {
              lastLoadedPosition = new WebPosition(new Uri(lastLoadedPosition.Url, attr.Value));
              if (!GetModel().PositionsList.Contains(lastLoadedPosition))
              {
                GetModel().PositionsList.Add(lastLoadedPosition);
                if (linkedPageMode == AddLinkedPageMode.Default)
                  lastLoadedPosition = null;
              }
              else
                lastLoadedPosition = null;
              return;
            }
          }
        }
        lastLoadedPosition = null;
      }
    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
      lastLoadedPosition = null;
      loadPath = null;
    }

    private void toolStripButton2_Click(object sender, EventArgs e)
    {
      GetModel().PositionsList.Add(GetModel().ActivePosition);
    }

    private enum AddLinkedPageMode
    {
      Default,
      Recursive
    }

    private void SynchronizeToolStrip()
    {
      this.toolStripSplitButton1.Enabled = GetModel().Mode != Model.WorkMode.Browse;
      bool hasSelected = listBox1.SelectedItems.Count > 0;
      
      moveUpButton.Enabled = hasSelected && !listBox1.SelectedIndices.Contains(0);
      moveUpToolStripMenuItem.Enabled = moveUpButton.Enabled;
      
      moveDownButton.Enabled = hasSelected && !listBox1.SelectedIndices.Contains(listBox1.Items.Count - 1);
      moveDownToolStripMenuItem.Enabled = moveDownButton.Enabled;
      
      deleteButton.Enabled = hasSelected;
      deleteFromListToolStripMenuItem.Enabled = hasSelected;
      viewUrlButton.Enabled = hasSelected;
      viewInBrowserToolStripMenuItem.Enabled = hasSelected;

      switch (linkedPageMode)
      {
        case AddLinkedPageMode.Default:
          this.toolStripSplitButton1.Text = addLinkedPageToolStripMenuItem.Text;
          this.toolStripSplitButton1.Image = addLinkedPageToolStripMenuItem.Image;
          break;
        case AddLinkedPageMode.Recursive:
          this.toolStripSplitButton1.Text = addLinkedPageRecursivlyToolStripMenuItem.Text;
          this.toolStripSplitButton1.Image = addLinkedPageRecursivlyToolStripMenuItem.Image;
          break;
      }
    }

    private AddLinkedPageMode linkedPageMode = AddLinkedPageMode.Default;

    private void addLinkedPageToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (lastLoadedPosition == null)
      {
        linkedPageMode = AddLinkedPageMode.Default;
        ReceiveXmlNode(GetModel().GetSelectedNode(GetModel().ActivePosition));
      }
    }

    private void addLinkedPageRecursivlyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (lastLoadedPosition == null)
      {
        linkedPageMode = AddLinkedPageMode.Recursive;
        ReceiveXmlNode(GetModel().GetSelectedNode(GetModel().ActivePosition));
      }
    }

    private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
    {
      if (lastLoadedPosition == null)
        ReceiveXmlNode(GetModel().GetSelectedNode(GetModel().ActivePosition));
    }
    #endregion

    private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      for (int i = 0; i < listBox1.Items.Count; i++)
      {
        if (listBox1.GetItemRectangle(i).Contains(e.X, e.Y))
        {
          Navigate(i);
          return;
        }
      }
    }

    private void Navigate(int index)
    {
      if (0 <= index && index < GetModel().PositionsList.Count)
        GetModel().ActivePosition = GetModel().PositionsList[index];
    }

    private void moveUpButton_Click(object sender, EventArgs e)
    {
      List<int> indeces = _.From<int>(listBox1.SelectedIndices);
      listBox1.SelectedIndices.Clear();
      indeces.Sort();
      foreach (int index in indeces)
      {
        WebPosition position = CurModel.PositionsList[index];
        CurModel.PositionsList.RemoveAt(index);
        CurModel.PositionsList.Insert(index - 1, position);
      }
    }

    private void deleteButton_Click(object sender, EventArgs e)
    {
      DeleteSelected();
    }

    private void moveDownButton_Click(object sender, EventArgs e)
    {
      List<int> indeces = _.From<int>(listBox1.SelectedIndices);
      listBox1.SelectedIndices.Clear();
      indeces.Sort(delegate(int left, int right) { return -left + right; });
      foreach (int index in indeces)
      {
        WebPosition position = CurModel.PositionsList[index];
        CurModel.PositionsList.RemoveAt(index);
        CurModel.PositionsList.Insert(index + 1, position);
      }
    }

    private void addFilesToList_Click(object sender, EventArgs e)
    {
      if (openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        foreach (string name in openFileDialog1.FileNames)
        {
          WebPosition position = WebPosition.Parse(name);
          if (position != null)
            CurModel.PositionsList.Add(position);
        }
      }
    }

    private void viewUrlButton_Click(object sender, EventArgs e)
    {
      if (listBox1.SelectedIndex >= 0)
        Navigate(listBox1.SelectedIndex);
    }

  }
}
