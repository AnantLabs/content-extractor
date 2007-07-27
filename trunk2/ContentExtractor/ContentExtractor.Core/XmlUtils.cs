//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 29.06.2007
// Time: 19:34
//

using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ContentExtractor.Core
{
  public static class XmlUtils
  {
    public static XmlDocument LoadXml(string content)
    {
      XmlDocument result = new XmlDocument();
      result.LoadXml(content);
      return result;
    }

    public static T Deserialize<T>(Stream stream)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(T));
      return (T)serializer.Deserialize(stream);
    }

    public static T Deserialize<T>(string filename)
    {
      using (Stream stream = File.OpenRead(filename))
        return Deserialize<T>(stream);
    }

    public static void Serialize<T>(string filename, T obj)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(T));
      using (Stream stream = File.Create(filename))
        serializer.Serialize(stream, obj);
    }

    public static string GetXPath(XmlNode node)
    {
      string result = "";
      XmlNode currentNode = node;
      while (currentNode != null)
      {
        if (currentNode.NodeType != XmlNodeType.Document)
        {
          string nodeName = string.Empty;
          switch (currentNode.NodeType)
          {
            case XmlNodeType.Element:
              nodeName = currentNode.Name + GetIndex(currentNode);
              break;
            case XmlNodeType.Attribute:
              nodeName = "@" + currentNode.Name;
              break;
            case XmlNodeType.CDATA:
            case XmlNodeType.Text:
              nodeName = "text()" + GetIndex(currentNode);
              break;
          }
          //if (string.IsNullOrEmpty(result))
          //  result = nodeName;
          //else
          result = "/" + nodeName + result;
        }
        if (currentNode.NodeType == XmlNodeType.Attribute)
          currentNode = ((XmlAttribute)currentNode).OwnerElement;
        else
          currentNode = currentNode.ParentNode;
      }
      //result = "/" + result;
      return result;
    }

    static string GetIndex(XmlNode node)
    {
      if (node != null)
      {
        XmlNode parent = node.ParentNode;
        if (parent != null)
        {
          int index = 0;
          foreach (XmlNode childNode in parent.ChildNodes)
            if ((childNode.NodeType & (XmlNodeType.Element | XmlNodeType.Text | XmlNodeType.CDATA)) != 0)
            {
              if (childNode == node)
                break;
              if (childNode.Name == node.Name && childNode.NamespaceURI == node.NamespaceURI)
                ++index;
            }
          return string.Format("[{0}]", index + 1);
        }
      }
      return string.Empty;
    }

  }


  public class XPathInfo
  {
    public static XPathInfo Parse(string xpath)
    {
      return new XPathInfo(xpath);
    }

    private XPathInfo(string xpath)
    {
      this.XPath = xpath;
    }

    public readonly string XPath;

    public string[] Parts
    {
      get
      {
        List<string> result = new List<string>();
        Match m = partRegex.Match(XPath);
        if (m.Success && m.Groups["part"].Success)
          for (int i = 0; i < m.Groups["part"].Captures.Count; i++)
            result.Add(m.Groups["part"].Captures[i].Value);
        return result.ToArray();
      }
    }

    private const string XPathHeadPattern = @"^((?<part>(" + XPathPart.Pattern + ")?)/)?";
    private const string XPathTailPattern = @"(?<part>" + XPathPart.Pattern + ")$";

    private const string PartsPatternt = XPathHeadPattern +
                                         "((?<part>" + XPathPart.Pattern + ")/)*" +
                                         XPathTailPattern;
    private Regex partRegex = new Regex(PartsPatternt, RegexOptions.Compiled);

    public XPathPart[] XParts
    {
      get
      {
        return Array.ConvertAll<string, XPathPart>(Parts,
          delegate(string part)
          {
            if (!string.IsNullOrEmpty(part))
              return new XPathPart(part);
            else
              return null;
          });
      }
    }

    public static string GetXPathsCommonPart(string leftPath, string rightPath)
    {
      XPathInfo left = new XPathInfo(leftPath);
      XPathInfo right = new XPathInfo(rightPath);
      int partIndex;
      List<string> result = new List<string>();
      for (partIndex = 0; partIndex < Math.Min(left.Parts.Length, right.Parts.Length); partIndex++)
      {
        if (left.Parts[partIndex] == right.Parts[partIndex]) // html[1] vs html[1]
          result.Add(left.Parts[partIndex]);
        else // html[1] vs html
        {
          string lShort = GetPathWithoutBracets(left.Parts[partIndex]);
          string rShort = GetPathWithoutBracets(right.Parts[partIndex]);
          if (lShort == right.Parts[partIndex])
            result.Add(lShort);
          else if (rShort == left.Parts[partIndex])
            result.Add(rShort);
          break;
        }
      }
      if (partIndex > 0)
        return string.Join("/", result.ToArray());
      else
        return ".";
    }

    public static string GetRelativeXPath(string nodePath, string rootPath)
    {
      XPathInfo node = XPathInfo.Parse(nodePath);
      XPathInfo root = XPathInfo.Parse(rootPath);
      if (node.Parts.Length >= root.Parts.Length)
      {
        int partIndex;
        // Check if nodePath is a descendant for rootPath
        for (partIndex = 0; partIndex < root.Parts.Length - 1; partIndex++)
          if (node.Parts[partIndex] != root.Parts[partIndex])
            return null;
        if (node.Parts[partIndex].StartsWith(root.Parts[partIndex]))
          partIndex++;
        else
          return null;

        string result = string.Join("/", node.Parts, partIndex, node.Parts.Length - partIndex);
        if (!string.IsNullOrEmpty(result))
          return result;
        else
          return ".";
      }
      else // root cannot be parent of nodePath
        return null;
    }

    public static string CombineXPaths(string node, string relative)
    {
      if (node == ".")
        return relative;
      if (relative == ".")
        return node;
      return node + "/" + relative;
    }

    //public static string RowCommonPart(string leftPath, string rightPath)
    //{
    //  string root = GetXPathsCommonPart(leftPath, rightPath);
    //  XPathInfo left = new XPathInfo(GetRelativeXPath(leftPath, root));
    //  XPathInfo right = new XPathInfo(GetRelativeXPath(rightPath, root));

    //  if (left.Parts.Length > 0 && right.Parts.Length > 0)
    //  {
    //    string leftPart = PathWithoutBracets(left.Parts[0]);
    //    if (!string.IsNullOrEmpty(leftPart) && leftPart == PathWithoutBracets(right.Parts[0]))
    //      return root + "/" + leftPart;
    //  }
    //  return root;
    //}

    public static string GetPathWithoutBracets(string path)
    {
      Match m = Regex.Match(path, @"(?<res>.*?)(\[\d+\])?$", RegexOptions.Compiled);
      if (m.Success)
        return m.Groups["res"].Value;
      else
        return path;
    }

  }

  public enum XPathPartType
  {
    Element,
    Text,
    Attribute
  }

  public class XPathPart
  {
    public readonly XPathPartType Type;
    /// <summary>
    /// Full name of the part including index
    /// F.e. html[1]
    /// </summary>
    public readonly string FullName;
    /// <summary>
    /// Name including only tag or attribute name
    /// F.e. html, @href, text()
    /// </summary>
    public readonly string Name;
    /// <summary>
    /// Indexing part of the XPathPart
    /// F.e. for html[1] tag Index equals 1.
    /// Null if no index has been given
    /// </summary>
    public readonly int? Index;

    public const string TextPattern = @"(?<text>text\(\))";
    public const string ElementPattern = @"(?<elemName>\w[\w\d]*)";
    public const string AttrubutePattern = @"(@(?<attrName>\w[\w\d]*))";
    public const string IndexPattern = @"(\[(?<index>\d+)\])?";
    public const string Pattern = "(" + AttrubutePattern + "|" + TextPattern + "|" +
      ElementPattern + ")" + IndexPattern;

    private static Regex regex = new Regex("^" + Pattern + "$", RegexOptions.Compiled | RegexOptions.Singleline);

    public XPathPart(string fullName)
    {
      this.FullName = fullName;
      Match m = regex.Match(fullName);
      if (m.Success)
      {
        if (m.Groups["elemName"].Success)
        {
          Type = XPathPartType.Element;
          Name = m.Groups["elemName"].Value;
        }
        if (m.Groups["attrName"].Success)
        {
          Type = XPathPartType.Attribute;
          Name = m.Groups["attrName"].Value;
        }
        if (m.Groups["text"].Success)
        {
          Type = XPathPartType.Text;
          Name = m.Groups["text"].Value;
        }
        if (m.Groups["index"].Success)
          Index = int.Parse(m.Groups["index"].Value);
      }
      else
        throw new ArgumentException(fullName);
    }
  }

}
