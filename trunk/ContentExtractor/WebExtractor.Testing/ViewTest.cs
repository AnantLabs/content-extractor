using System;
using System.Collections.Generic;
using System.Text;
using ContentExtractor.Core;
using NUnit.Framework;
using System.Windows.Forms;
using System.Xml;
using ContentExtractor.Gui;

namespace WebExtractor_Testing
{
  public class BrowserTestBase
  {
    protected WebBrowserWrapper wrapper;
    protected XmlDocument PageDoc;
    protected string SelectedNode;

    [SetUp]
    public void SetUp()
    {
      wrapper = new WebBrowserWrapper();
      //wrapper.GetAccessor(Presenter.PageTree).Changed += new Action<object>(wrapper_XmlTreeChanged);
      //wrapper.GetAccessor(Presenter.SelectedNode).Changed += new Action<object>(wrapper_SelectedNodeChanged);
    }

    void wrapper_SelectedNodeChanged(object obj)
    {
      SelectedNode = (string)obj;
    }

    protected WebBrowser Browser
    {
      get { return wrapper.Browser; }
    }

    void wrapper_XmlTreeChanged(object obj)
    {
      PageDoc = (XmlDocument)obj;
    }

    protected XmlNode GetXmlNode(string xpath)
    {
      return PageDoc.SelectSingleNode(xpath);
    }

    [TearDown]
    public void TearDown()
    {
      if (wrapper != null && !wrapper.IsDisposed)
        wrapper.Dispose();
    }

    protected void Init(string code)
    {
      Browser.DocumentText = code;
      do
      {
        TestUtils.DoSomeEvents();
      }
      while (Browser.IsBusy);
    }

    protected HtmlElementWrapper GetElementById(string id)
    {
      return new HtmlElementWrapper(Browser.Document.GetElementById(id));
    }
  }
}
