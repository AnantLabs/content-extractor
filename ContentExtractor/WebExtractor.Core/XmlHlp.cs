using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using MetaTech.Library;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace ContentExtractor.Core
{
  public class XmlHlp
  {
    public static string GetPath(XmlNode node)
    {
      string result = string.Empty;
      string prevName = string.Empty;
      string prevNamespace = null;

      XmlNode currentNode = node;
      while (currentNode != null)
      {
        if (prevNamespace != null)
        {
          if (prevNamespace == currentNode.NamespaceURI || prevName.StartsWith("text()"))
            result = "/" + prevName + result;
          else
            result = string.Format("/{{{0}}}:{1}{2}", prevNamespace, prevName, result);
        }
        switch (currentNode.NodeType)
        {
          case XmlNodeType.Element:
            prevName = currentNode.LocalName + GetIndex(currentNode);
            break;
          case XmlNodeType.Attribute:
            prevName = "@" + currentNode.LocalName;
            break;
          case XmlNodeType.CDATA:
          case XmlNodeType.Text:
            prevName = "text()" + GetIndex(currentNode);
            break;
        }
        prevNamespace = currentNode.NamespaceURI;

        if (currentNode.NodeType == XmlNodeType.Attribute)
          currentNode = ((XmlAttribute)currentNode).OwnerElement;
        else
          currentNode = currentNode.ParentNode;
      }
      return result;


      //string result = "";
      //XmlNode currentNode = node;
      //string namespaceUri = string.Empty;
      //while (currentNode != null)
      //{
      //  if (currentNode.NodeType != XmlNodeType.Document)
      //  {
      //    string nodeName = string.Empty;
      //    switch (currentNode.NodeType)
      //    {
      //      case XmlNodeType.Element:
      //        nodeName = NodeFullName(currentNode) + GetIndex(currentNode);
      //        break;
      //      case XmlNodeType.Attribute:
      //        nodeName = "@" + NodeFullName(currentNode);
      //        break;
      //      case XmlNodeType.CDATA:
      //      case XmlNodeType.Text:
      //        nodeName = "text()" + GetIndex(currentNode);
      //        break;
      //    }
      //    if (StringHlp2.IsEmpty(result))
      //      result = nodeName;
      //    else
      //      result = nodeName + "/" + result;
      //  }
      //  if (currentNode.NodeType == XmlNodeType.Attribute)
      //    currentNode = ((XmlAttribute)currentNode).OwnerElement;
      //  else
      //    currentNode = currentNode.ParentNode;
      //}
      //result = "/" + result;
      //return result;
    }

    static string NodeFullName(XmlNode node)
    {
      string result = node.LocalName;
      if (!string.IsNullOrEmpty(node.NamespaceURI))
        return string.Format("{{{0}}}:{1}", node.NamespaceURI, node.LocalName);
      else
        return node.LocalName;
    }

    static string GetIndex(XmlNode node)
    {
      if (node == null)
        return "";
      XmlNode parent = node.ParentNode;
      if (parent == null)
        return "";
      int index = 0;
      foreach (XmlNode childNode in parent.ChildNodes)
      {
        if ((childNode.NodeType & (XmlNodeType.Element | XmlNodeType.Text | XmlNodeType.CDATA)) == 0)
          continue;
        if (childNode == node)
          break;
        if (childNode.Name == node.Name && childNode.NamespaceURI == node.NamespaceURI)
          ++index;
      }
      return string.Format("[{0}]", index + 1);
    }

    public static XmlNode SelectSingleNode(XmlNode parent, string path)
    {
      try
      {
        if (parent != null && !string.IsNullOrEmpty(path))
        {
          XmlNameTable table;
          if (parent is XmlDocument)
            table = ((XmlDocument)parent).NameTable;
          else if (parent.OwnerDocument != null)
            table = parent.OwnerDocument.NameTable;
          else
            return null;
          XmlNamespaceManager manager;
          string xpath = ParsePath(path, table, out manager);
          return parent.SelectSingleNode(xpath, manager);
        }
        else
          return null;
      }
      catch (XPathException)
      {
        return null;
      }
    }

    public static XmlNodeList SelectNodes(XmlNode parent, string path)
    {
      try
      {
        if (parent != null)
        {
          XmlNameTable table;
          if (parent is XmlDocument)
            table = ((XmlDocument)parent).NameTable;
          else if (parent.OwnerDocument != null)
            table = parent.OwnerDocument.NameTable;
          else
            return null;
          XmlNamespaceManager manager;
          string xpath = ParsePath(path, table, out manager);
          return parent.SelectNodes(xpath, manager);
        }
        else
          return null;
      }
      catch (XPathException)
      {
        return null;
      }
    }

    public static XPathNodeIterator Select(XPathNavigator navigator, string path)
    {
      try
      {
        if (navigator != null)
        {
          XmlNamespaceManager manager;
          string xpath = ParsePath(path, navigator.NameTable, out manager);
          return navigator.Select(xpath, manager);
        }
        else
          return null;
      }
      catch (XPathException)
      {
        return null;
      }
    }

    public static XPathNavigator SelectSingleNode(IXPathNavigable navigator, string path)
    {
      try
      {
        if (navigator != null)
        {
          XPathNavigator node = navigator.CreateNavigator();

          XmlNamespaceManager manager;
          string xpath = ParsePath(path, node.NameTable, out manager);
          return node.SelectSingleNode(xpath, manager);
        }
        else
          return null;
      }
      catch (XPathException)
      {
        return null;
      }
    }


    static string ParsePath(string path, XmlNameTable table, out XmlNamespaceManager manager)
    {
      manager = new XmlNamespaceManager(table);
      if (path == ".")
        return ".";
      string prefix = "A";
      Dictionary<string, string> name2prefix = new Dictionary<string, string>();

      Match match = Regex.Match(path, "{(.*?)}", RegexOptions.Compiled);
      while (match.Success)
      {
        string uri = match.Groups[1].Value;
        if (!name2prefix.ContainsKey(uri))
        {
          name2prefix[uri] = prefix;
          manager.AddNamespace(prefix, uri);
          prefix = WebExtractorHlp.NextExcelName(prefix);
        }
        match = match.NextMatch();
      }
      string lastNamespace = string.Empty;
      List<string> resultParts = new List<string>();
      foreach (XPathPart part in XPathInfo.Parse(path).XParts)
      {
        if (part == null)
          resultParts.Add(string.Empty);
        else
        {
          if ((part.Namespace != null || part.Type == XPathPartType.Text) && part.Namespace != lastNamespace)
            lastNamespace = part.Namespace;
          if (lastNamespace == null)
            lastNamespace = string.Empty;

          if (part.Type == XPathPartType.Attribute)
            resultParts.Add(string.Format("@{{{0}}}:{1}", lastNamespace, part.LocalName));
          else if (part.Index.HasValue)
            resultParts.Add(string.Format("{{{0}}}:{1}[{2}]", lastNamespace, part.LocalName, part.Index.Value));
          else
            resultParts.Add(string.Format("{{{0}}}:{1}", lastNamespace, part.LocalName));
        }
      }
      StringBuilder result = new StringBuilder(StringHlp2.Join("/", "{0}", resultParts));
      foreach (string uri in name2prefix.Keys)
      {
        manager.AddNamespace(name2prefix[uri], uri);
        result.Replace("{" + uri + "}:", name2prefix[uri] + ":");
      }
      result.Replace("{}:", "");
      return result.ToString();
    }

    public static string InnerText(XPathNavigator nav)
    {
      return nav.InnerXml;
    }

    public static XPathNavigator SelectChildren(IXPathNavigable node, string localName, XPathNodeType type)
    {
      foreach (XPathNavigator child in node.CreateNavigator().SelectChildren(type))
      {
        if (WebExtractorHlp.TagsAreSame(child.LocalName, localName))
          return child;
      }
      return null;
    }

    public static XPathNavigator SelectElement(IXPathNavigable node, string localName)
    {
      return SelectChildren(node, localName, XPathNodeType.Element);
    }

    public static XmlDocument HtmlDocFromNavigable(IXPathNavigable navigable)
    {
      XPathNavigator nav = navigable.CreateNavigator();
      try
      {
        if (WebExtractorHlp.TagsAreSame(nav.LocalName, "html"))
          return XmlHlp2.XmlDocFromString(nav.OuterXml);
        XPathNavigator html = XmlHlp.SelectElement(nav, "html");
        if (html != null)
          return XmlHlp2.XmlDocFromString(html.OuterXml);
      }
      catch (Exception exc)
      {
        TraceHlp2.WriteException(exc);
      }
      return new XmlDocument();
    }

    public static XmlDocument LoadXml(string content)
    {
      XmlDocument result = new XmlDocument();
      result.LoadXml(content);
      return result;
    }

  }
}
