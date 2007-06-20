using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;
using NUnit.Framework;

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


  }
}
