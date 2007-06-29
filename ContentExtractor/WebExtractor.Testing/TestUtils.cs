using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;
using NUnit.Framework;
using ContentExtractor.Core;

namespace WebExtractor_Testing
{
  public static class TestUtils
  {
    public static void DoSomeEvents()
    {
      for (int i = 0; i < 200; i++)
        Application.DoEvents();
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

    public static IntPtr SendMessage(Control control, int msg, IntPtr wparam, IntPtr lparam)
    {
      return SendMessage(new HandleRef(control, control.Handle), msg, wparam, lparam);
    }

    public static void XmlAssertEqual(XmlNode expected, XmlNode actual)
    {
      MetaTech.Library.XmlHlp2.IsXmlEqual(expected, actual, delegate(string message)
      {
        throw new AssertionException(message);
      });
    }

    public static TreeNode GetTreeNode(TreeNodeCollection collection, string path)
    {
      try
      {
        string[] parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        TreeNode result = null;
        foreach (string part in parts)
        {
          int index = int.Parse(part);
          if (result == null)
            result = collection[index];
          else
            result = result.Nodes[index];
        }
        return result;
      }
      catch (Exception exc)
      {
        throw new Exception(string.Format("BrowserTestFixture.GetTreeNode, can't parse '{0}' path", path), exc);
      }
    }

    public static void AssertXmlAreEqual(string expected, XmlNode actualNode)
    {
      AssertXmlAreEqual(XmlHlp.LoadXml(expected), actualNode);
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
          writer.WriteStartElement(el.Name);
          List<XmlAttribute> attrs = ContentExtractor.Core.Utils.CastList<XmlAttribute>(el.Attributes);
          if(attrs.Count > 0)
          {
            attrs.Sort(delegate(XmlAttribute left, XmlAttribute right) {return left.Name.CompareTo(right.Name);} );
            foreach(XmlAttribute at in attrs)
              writer.WriteAttributeString(at.Name, at.Value);
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
