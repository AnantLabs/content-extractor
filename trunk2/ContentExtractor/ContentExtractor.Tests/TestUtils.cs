using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml;
using NUnit.Framework;
using ContentExtractor.Core;

namespace ContentExtractorTests
{
  public static class TestUtils
  {
    public static void AssertXmlAreEqual(string expected, XmlNode actualNode)
    {
      AssertXmlAreEqual(XmlUtils.LoadXml(expected), actualNode);
    }
    
    public static void AssertXmlAreEqual(XmlNode expectedNode, XmlNode actualNode)
    {
      StringBuilder expected = new StringBuilder();
      StringBuilder actual = new StringBuilder();
      
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.Indent = false;
      settings.NewLineOnAttributes = false;
      settings.OmitXmlDeclaration = false;
      
      using( XmlWriter writer = XmlWriter.Create(expected, settings))
        WriteXmlNode(expectedNode, writer);
      using( XmlWriter writer = XmlWriter.Create(actual, settings))
        WriteXmlNode(actualNode, writer);
      
      Assert.AreEqual(expected.ToString(), actual.ToString());
    }
    
    private static void WriteXmlNode(XmlNode node, XmlWriter writer)
    {
      switch(node.NodeType)
      {
        case XmlNodeType.Element:
          XmlElement el = (XmlElement) node;
          writer.WriteStartElement(el.Prefix, el.LocalName, el.NamespaceURI);
          List<XmlAttribute> attrs = Utils.CastList<XmlAttribute>(el.Attributes);
          if(attrs.Count > 0)
          {
            attrs.Sort(delegate(XmlAttribute left, XmlAttribute right) {return left.Name.CompareTo(right.Name);} );
            foreach(XmlAttribute at in attrs)
              writer.WriteAttributeString(at.Prefix, 
                                          at.LocalName, 
                                          at.NamespaceURI, 
                                          at.Value);
          }
          foreach(XmlNode child in el.ChildNodes)
            WriteXmlNode(child, writer);
          writer.WriteEndElement();
          break;
        case XmlNodeType.Document:
          foreach(XmlNode child in node.ChildNodes)
            WriteXmlNode(child, writer);
          break;
        case XmlNodeType.CDATA:
        case XmlNodeType.Text:
          writer.WriteString(node.Value.Trim());
          break;
      }
    }
    

  }
}
