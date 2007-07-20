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
      using(Stream stream = File.OpenRead(filename))
        return Deserialize<T>(stream);
    }
    
    public static void Serialize<T>(string filename, T obj)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(T));
      using(Stream stream = File.Create(filename))
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
}
