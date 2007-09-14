using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using ContentExtractor.Core;

namespace ContentExtractor.Gui
{
  public partial class DocTreeView : UserControl
  {
    public DocTreeView()
    {
      InitializeComponent();
    }

    private State state;
    public void SetState(State state)
    {
      if (this.state == null)
      {
        this.state = state;
        state.SelectedNodeChanged += new EventHandler(SelectedNodeChanged);
      }
      else
        // TODO: Should warning to log
        throw new InvalidOperationException("Cannot assign state twice");
    }

    XmlDocument cachedDocument = null;

    private void timer1_Tick(object sender, EventArgs e)
    {
      XmlDocument freshDoc = state.GetXmlAsync(state.BrowserUri);
      if (!IsXmlEqual(cachedDocument, freshDoc))
      {
        Refresh(freshDoc);
      }
    }

    private void Refresh(XmlDocument freshDoc)
    {
      this.cachedDocument = freshDoc;
      Rebuild(treeView1.Nodes, freshDoc);
    }

    private bool IsXmlEqual(XmlDocument left, XmlDocument right)
    {
      return object.ReferenceEquals(left, right) || (left != null &&
        right != null &&
        left.SelectNodes(".//*").Count == right.SelectNodes(".//*").Count &&
        left.OuterXml.Length == right.OuterXml.Length);
    }

    Dictionary<string, TreeNode> nodeIndex = new Dictionary<string, TreeNode>();

    void SelectedNodeChanged(object sender, EventArgs e)
    {
      ApplySelectedNode();
    }

    private void ApplySelectedNode()
    {
      bool isSameNode;
      if (treeView1.SelectedNode == null)
        isSameNode = string.IsNullOrEmpty(state.SelectedNodeXPath);
      else
        isSameNode = object.Equals(state.SelectedNodeXPath, treeView1.SelectedNode.Tag);
      if (!isSameNode && nodeIndex.ContainsKey(state.SelectedNodeXPath))
      {
        treeView1.SelectedNode = nodeIndex[state.SelectedNodeXPath];
        treeView1.SelectedNode.EnsureVisible();
      }
    }

    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
    {
      InvokeSelectNode(e.Node);
    }

    private void InvokeSelectNode(TreeNode node)
    {
      state.SelectedNodeXPath = (string)node.Tag;
    }

    private void Rebuild(TreeNodeCollection collection, XmlDocument doc)
    {
      collection.Clear();
      nodeIndex.Clear();
      if (doc != null)
        LoadXmlNode(collection, doc.ChildNodes);
      ApplySelectedNode();
    }

    private void LoadXmlNode(TreeNodeCollection collection, IEnumerable list)
    {
      foreach (XmlNode child in list)
      {
        TreeNode tNode = CorrespondentNode(collection, child);
        if (tNode != null)
        {
          tNode.ImageKey = NodeType.Attribute;
          tNode.SelectedImageKey = NodeType.Attribute;
          if (child.NodeType == XmlNodeType.Text || child.NodeType == XmlNodeType.CDATA)
          {
            tNode.ImageKey = NodeType.Text;
            tNode.SelectedImageKey = NodeType.Text;
          }
          if (child.NodeType == XmlNodeType.Element)
          {
            tNode.ImageKey = NodeType.Tag;
            tNode.SelectedImageKey = NodeType.Tag;
            LoadXmlNode(tNode.Nodes, child.Attributes);
            LoadXmlNode(tNode.Nodes, child.ChildNodes);
          }
        }
      }
    }

    private static string CorrespondentName(XmlNode xmlNode)
    {
      string name = null;
      switch (xmlNode.NodeType)
      {
        case XmlNodeType.Attribute:
          name = "@" + xmlNode.Name;
          break;
        case XmlNodeType.CDATA:
        case XmlNodeType.Text:
          name = "text()";
          break;
        case XmlNodeType.Element:
          name = xmlNode.Name;
          break;
      }
      return name;
    }

    private TreeNode CorrespondentNode(TreeNodeCollection collection, XmlNode xmlNode)
    {
      string name = CorrespondentName(xmlNode);
      if (!string.IsNullOrEmpty(name))
      {
        TreeNode res = collection.Add(name);
        string xpath = XmlUtils.GetXPath(xmlNode);
        res.Tag = xpath;
        nodeIndex[xpath] = res;
        return res;
      }
      else
        return null;
    }

    public static class NodeType
    {
      public const string Attribute = "attribute";
      public const string Tag = "tag";
      public const string Text = "text";
    }

    private void addColumnMenuItem_Click(object sender, EventArgs e)
    {
      if (treeView1.SelectedNode != null)
        if (state.Project.Template.CanAutoModifyTemplate)
          state.Project.Template.AddColumn((string)treeView1.SelectedNode.Tag);
        else
          MessageBox.Show(Properties.Resources.NotAbleToAddSpecificColumnWarning,
              Properties.Resources.NotAbleToAddSpecificColumnWarningCaption,
              MessageBoxButtons.OK);
    }

    private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
        InvokeSelectNode(e.Node);
    }

  }
}
