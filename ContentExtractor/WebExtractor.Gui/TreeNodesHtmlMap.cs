using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MetaTech.Library;
using System.Collections;
using ContentExtractor.Core;

namespace ContentExtractor.Gui
{
  public class TreeNodesHtmlMap
  {
    public TreeNodesHtmlMap(TreeNodeCollection treeCollection)
    {
      this.treeCollection = treeCollection;
    }
    private readonly TreeNodeCollection treeCollection;

    public TreeNode GetTreeNode(XmlDocument doc, string node)
    {
      if (!WebExtractorHlp.IsCachedXmlActual(cachedDoc, doc))
      {
        Rebuild(doc);
      }
      if (node != null)
        return DictionaryHlp.GetValueOrDefault(xml2treeMap, node, null);
      else
        return null;
    }
    public string GetXmlNode(TreeNode node, XmlDocument doc)
    {
      if (!WebExtractorHlp.IsCachedXmlActual(cachedDoc, doc))
      {
        Rebuild(doc);
      }
      if (node != null)
        return (string)node.Tag;
      else
        return null;
    }

    XmlDocument cachedDoc = new XmlDocument();

    private Dictionary<string, TreeNode> xml2treeMap = new Dictionary<string, TreeNode>();
    private void Rebuild(XmlDocument doc)
    {
      cachedDoc = WebExtractorHlp.CopyXmlDocument(doc);
      treeCollection.Clear();
      xml2treeMap.Clear();
      if (doc != null)
        LoadXmlNode(treeCollection, doc.ChildNodes);
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
        xml2treeMap[XmlHlp.GetPath(xmlNode)] = res;
        res.Tag = XmlHlp.GetPath(xmlNode);
        return res;
      }
      else
        return null;
    }


  }
}
