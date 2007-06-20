using System;
using System.Collections.Generic;
using System.Text;
using ContentExtractor.Gui;
using NUnit.Framework;
using ContentExtractor.Core;
using System.Windows.Forms;
using System.Reflection;
using MetaTech.Library;
using System.Xml;

namespace WebExtractor_Testing.Core
{
  [TestFixture]
  public class SelectedNodeInTreeView
  {
    [SetUp]
    public void SetUp()
    {
      view = new TreeView();
      map = new TreeNodesHtmlMap(view.Nodes);
      TestUtils.DoSomeEvents();
    }
    TreeNodesHtmlMap map;
    TreeView view;

    private void InitMap(XmlDocument doc)
    {
      map.GetTreeNode(doc, "");
    }

    private void InitMap(string code)
    {
      map.GetTreeNode(XmlHlp.HtmlDocFromNavigable(WebExtractorHlp.LoadHtmlCode(code)), "");
    }

    private void AssertTreeNode(TreeNode node, int expectedChildNum, string expectedText)
    {
      Assert.AreEqual(expectedChildNum, node.Nodes.Count);
      Assert.AreEqual(expectedText, node.Text);
    }


    [Test]
    public void TreeNode2XmlNode()
    {
      XmlDocument doc = XmlHlp2.XmlDocFromString("<html><body><p>Some text #1 </p><p> Some text #2</p></body></html>");
      InitMap(doc);

      string path = map.GetXmlNode(TestUtils.GetTreeNode(view.Nodes, "/0/0/1/0"), doc);
      Assert.AreEqual("/html[1]/body[1]/p[2]/text()[1]", path);
    }


    [Test]
    public void XmlNode2TreeNode()
    {
      XmlDocument doc = XmlHlp2.XmlDocFromString("<html><body><p>Some text #1 </p><p> Some text #2</p></body></html>");
      TreeNode node = map.GetTreeNode(doc, "/html[1]/body[1]/p[2]/text()[1]");
      Assert.AreEqual(TestUtils.GetTreeNode(view.Nodes, "/0/0/1/0"), node);
    }

    [Test]
    public void OneElement()
    {
      InitMap("<html></html>");
      Assert.AreEqual(1, view.Nodes.Count);
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0"), 0, "html");
    }
    [Test]
    public void TextNodes()
    {
      InitMap("<html>SuperText</html>");

      Assert.AreEqual(1, view.Nodes.Count);
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0"), 1, "html");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0"), 0, "text()");
    }
    [Test]
    public void Attributes()
    {
      InitMap("<html><body style=\"color:Red\">Some text</body> </html>");

      Assert.AreEqual(1, view.Nodes.Count);
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0"), 1, "html");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0"), 2, "body");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0/0"), 0, "@style");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0/1"), 0, "text()");
    }
    [Test]
    public void Comment()
    {
      InitMap("<html><body><!-- Comment--></body></html>");
      Assert.AreEqual(1, view.Nodes.Count);
      //Body tag
      TreeNode curNode = TestUtils.GetTreeNode(view.Nodes, "/0/0");
      AssertTreeNode(curNode, 0, "body");
    }

    [Test]
    public void HtmlDocument2TreeView()
    {
      InitMap("<html><body><p>#1<p>#2<p> #3</body></html>");

      Assert.AreEqual(1, view.Nodes.Count);
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0"), 1, "html");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0"), 3, "body");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0/0"), 1, "p");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0/0/0"), 0, "text()");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0/1"), 1, "p");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0/1/0"), 0, "text()");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0/2"), 1, "p");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0/2/0"), 0, "text()");
    }

    [Test]
    public void ChangingPageDocChangesTreeView()
    {
      HtmlDocument2TreeView();
      InitMap("<html><body><p>text</p></body></html>");

      Assert.AreEqual(1, view.Nodes.Count);
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0"), 1, "html");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0"), 1, "body");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0/0"), 1, "p");
      AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0/0/0"), 0, "text()");
    }

    [Ignore]
    [Test]
    public void NodesImages()
    {
      //SetXmlCode(string.Format("<{0}><{1} color='blue'>some text</{1}></{0}>", "html", "body"));
      //Assert.AreEqual(1, view.Nodes.Count);
      //AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0"), 1, "html");
      //AssertTreeNode(TestUtils.GetTreeNode(view.Nodes, "/0/0"), 2, "body");
      //Assert.AreEqual("attribute", TestUtils.GetTreeNode(view.Nodes, "/0/0/0").ImageKey);
      //Assert.AreEqual("text", TestUtils.GetTreeNode(view.Nodes, "/0/0/1").ImageKey);
      //Assert.AreEqual("tag", TestUtils.GetTreeNode(view.Nodes, "/0/0").ImageKey);

      //model.SelectedNode = XmlHlp2.GetPath(model.PageTree.SelectSingleNode("/html/body"));
      ////presenter.ForceSynchronyse();
      //Assert.AreEqual(TestUtils.GetTreeNode(view.Nodes, "/0/0"), view.SelectedNode);
      //Assert.AreEqual("tag", view.SelectedImageKey);
    }


  }

}
