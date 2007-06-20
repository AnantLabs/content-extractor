using System;
using System.Collections.Generic;
using System.Text;
using ContentExtractor.Core;
using System.Xml;
using NUnit.Framework;
using System.Windows.Forms;
using ContentExtractor.Gui;

namespace WebExtractor_Testing
{
  public class BrowserTestFixture
  {
    protected BrowserForm form;
    //protected Presenter treePresenter;
    //protected Presenter webPresenter;
    //protected Presenter gridPresenter;
    protected Model model;

    [SetUp]
    public virtual void SetUp()
    {
      form = new BrowserForm();
      model = new Model();
      //model.NavigationAllowed = false;
      //page.Init(form.Browser);
      //treePresenter = new Presenter(delegate { return model; }, form.TreeViewWrapper);
      //webPresenter = new Presenter(delegate { return model; }, form.BrowserWrapper);
      //gridPresenter = new Presenter(delegate { return model; }, form.GridWrapper);
      ForceSynchronyse();
    }
    [TearDown]
    public virtual void TearDown()
    {
      if (!form.IsDisposed)
        form.Dispose();
    }

    protected void ForceSynchronyse()
    {
      //webPresenter.ForceSynchronyse();
      //treePresenter.ForceSynchronyse();
      //gridPresenter.ForceSynchronyse();
      form.GridWrapper.ForceGridSynchronizer();
    }

    protected void Init(string htmlCode)
    {
      //model.Position = new WebPosition();
      //model.Position.url = WebPosition.EmptyUri;
      //model.Position.htmlCode = htmlCode;
      //ForceSynchronyse();
      form.Browser.DocumentText = htmlCode;
      do
      {
        TestUtils.DoSomeEvents();
      }
      while (form.Browser.IsBusy);
      ForceSynchronyse();
    }

    protected HtmlElementWrapper GetElementById(string id)
    {
      return new HtmlElementWrapper(form.Browser.Document.GetElementById(id));
    }

    protected XmlNode GetXmlNode(string xpath)
    {
      return model.ActivePosition.XmlDocument.SelectSingleNode(xpath);
    }

    protected TreeNode GetTreeNode(string path)
    {
      try
      {
        string[] parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        TreeNode result = null;
        foreach (string part in parts)
        {
          int index = int.Parse(part);
          if (result == null)
            result = form.TreeViewWrapper.TreeView.Nodes[index];
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

    protected void AssertTreeNode(TreeNode node, int expectedChildNum, string expectedText)
    {
      Assert.AreEqual(expectedChildNum, node.Nodes.Count);
      Assert.AreEqual(expectedText, node.Text);
    }


  }
}
