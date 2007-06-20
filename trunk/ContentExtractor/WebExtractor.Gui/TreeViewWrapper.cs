using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ContentExtractor.Core;
using System.Collections;
using MetaTech.Library;

namespace ContentExtractor.Gui
{
  public partial class TreeViewWrapper : UserControl, IView
  {
    public TreeViewWrapper()
    {
      InitializeComponent();

      map = new TreeNodesHtmlMap(treeView.Nodes);
    }
    private string cachedSelectedNode = string.Empty;

    private Model CurModel
    {
      get
      {
        return GetModel();
      }
    }

    public TreeView TreeView
    {
      get { return treeView; }
    }

    private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (e.Action != TreeViewAction.Unknown)
      {
        GetModel().SelectedNodes[GetModel().ActivePosition.Persist] =
          map.GetXmlNode(treeView.SelectedNode, GetModel().ActivePosition.XmlDocument);
      }
    }

    private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
    {
      TreeNode node = (TreeNode)e.Item;
      //Отправляем Xml-вершину
      WebExtractorHlp.DoDragHtmlNode(this, map.GetXmlNode(node, GetModel().ActivePosition.XmlDocument));
    }

    private TreeNodesHtmlMap map;

    public void SetSelectedNode()
    {
      cachedSelectedNode = GetModel().GetSelectedNode(GetModel().ActivePosition);
      XmlNode localNode = XmlHlp.SelectSingleNode(GetModel().ActivePosition.XmlDocument, cachedSelectedNode);

      treeView.SelectedNode = map.GetTreeNode(GetModel().ActivePosition.XmlDocument, cachedSelectedNode);
      if (treeView.SelectedNode != null)
        treeView.SelectedNode.EnsureVisible();

      this.selectedNodeLabel.Text = string.Format("Selected node: {0}", GetModel().GetSelectedNode(GetModel().ActivePosition));
    }

    Getter<Model> GetModel = delegate { return new Model(); };

    #region IView Members

    public void InitModel(Getter<Model> modelGetter)
    {
      this.GetModel = modelGetter;
    }

    int cachedDocumentCodeHash = string.Empty.GetHashCode();

    public void ForceSynchronize()
    {
      this.Enabled = GetModel().Mode == Model.WorkMode.Parse;
      treeView.Visible = this.Enabled;
      if (!this.Enabled)
      {
        return;
      }

      int docHash = GetModel().ActivePosition.DocumentText.GetHashCode();

      if (docHash != cachedDocumentCodeHash ||
        cachedSelectedNode != GetModel().GetSelectedNode(GetModel().ActivePosition))
      {
        cachedDocumentCodeHash = docHash;
        SetSelectedNode();
      }

      TreeNode curNode = TreeView.SelectedNode;
      toParentButton.Enabled = curNode != null && curNode.Parent != null;
      toChildButton.Enabled = curNode != null && curNode.FirstNode != null;
      toPrevButton.Enabled = curNode != null && curNode.PrevNode != null;
      toNextButton.Enabled = curNode != null && curNode.NextNode != null;
    }

    #endregion

    private void toParentButton_Click(object sender, EventArgs e)
    {
      TreeNode curNode = TreeView.SelectedNode;
      if (curNode != null && curNode.Parent != null)
        GetModel().SelectedNodes[GetModel().ActivePosition.Persist] = map.GetXmlNode(curNode.Parent, GetModel().ActivePosition.XmlDocument);
    }

    private void toPrevButton_Click(object sender, EventArgs e)
    {
      TreeNode curNode = TreeView.SelectedNode;
      if (curNode != null && curNode.PrevNode != null)
        GetModel().SelectedNodes[GetModel().ActivePosition.Persist] = map.GetXmlNode(curNode.PrevNode, GetModel().ActivePosition.XmlDocument);
    }

    private void toNextButton_Click(object sender, EventArgs e)
    {
      TreeNode curNode = TreeView.SelectedNode;
      if (curNode != null && curNode.NextNode != null)
        GetModel().SelectedNodes[GetModel().ActivePosition.Persist] = map.GetXmlNode(curNode.NextNode, GetModel().ActivePosition.XmlDocument);
    }

    private void toChildButton_Click(object sender, EventArgs e)
    {
      TreeNode curNode = TreeView.SelectedNode;
      if (curNode != null && curNode.FirstNode != null)
        GetModel().SelectedNodes[GetModel().ActivePosition.Persist] = map.GetXmlNode(curNode.FirstNode, GetModel().ActivePosition.XmlDocument);
    }

    private void statusStrip1_Resize(object sender, EventArgs e)
    {
      this.selectedNodeLabel.Width = this.statusStrip1.Width - 18;
    }

    private void markNodeAsNewColumnToolStripMenuItem_Click(object sender, EventArgs e)
    {
      TreeNode node = (TreeNode)contextMenuStrip1.Tag as TreeNode;
      CurModel.Template.AddXPathColumnToPosition(0, CurModel.Template.Columns.Count, map.GetXmlNode(node, CurModel.ActivePosition.XmlDocument));
    }

    private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        CurModel.SelectedNodes[CurModel.ActivePosition.Persist] = map.GetXmlNode(e.Node, CurModel.ActivePosition.XmlDocument);
        treeView.SelectedNode = e.Node;
        contextMenuStrip1.Tag = e.Node;
        contextMenuStrip1.Show(treeView, e.Location);
      }
    }

  }
}
