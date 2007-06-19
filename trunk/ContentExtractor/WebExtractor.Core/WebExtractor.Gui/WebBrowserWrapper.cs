using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MetaTech.Library;
using System.Runtime.InteropServices;
using ContentExtractor.Core;
using System.IO;
using Sgml;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;
using SoftTech.Gui;

namespace ContentExtractor.Gui
{
  [ComVisible(true)]
  public partial class WebBrowserWrapper : UserControl, IView
  {
    public WebBrowserWrapper()
    {
      InitializeComponent();
      highlighter = new HtmlElementsHighlighter(this.Browser);
      string style = "FILTER: progid:DXImageTransform.Microsoft.Alpha(opacity=30);BACKGROUND-COLOR: #ff0000";
      highlighter.Links[style] = GetSelectedElements;
    }

    public WebBrowser Browser
    {
      get { return webBrowser1; }
    }

    public void InitDocumentForParse()
    {
      if (webBrowser1.Document != null)
      {
        webBrowser1.Document.MouseDown += new HtmlElementEventHandler(Document_MouseDown);
        webBrowser1.Document.MouseMove += new HtmlElementEventHandler(Document_MouseMove);
        //foreach (HtmlElement el in webBrowser1.Document.Body.All)
        //  el.MouseDown += new HtmlElementEventHandler(Document_MouseDown);
      }
    }
    public void DeInitDocumentForParse()
    {
      if (webBrowser1.Document != null)
      {
        webBrowser1.Document.MouseMove -= new HtmlElementEventHandler(Document_MouseMove);
        if (webBrowser1.Document.Body != null && webBrowser1.Document.Body.All != null)
          foreach (HtmlElement el in webBrowser1.Document.Body.All)
            el.MouseDown -= new HtmlElementEventHandler(Document_MouseDown);
      }
    }

    bool dragHasDone = false;
    string dragNode = string.Empty;

    void Document_MouseMove(object sender, HtmlElementEventArgs e)
    {
      e.BubbleEvent = false;
      if (e.MouseButtonsPressed == MouseButtons.Left && !dragHasDone)
      {
        dragHasDone = true;
        DoDragDrop(dragNode, DragDropEffects.Link);
      }
    }

    void Document_MouseDown(object sender, HtmlElementEventArgs e)
    {
      using (new SoftTech.Gui.WaitCursorShower(this))
      {
        e.BubbleEvent = false;
        if (e.MouseButtonsPressed == MouseButtons.Left || e.MouseButtonsPressed == MouseButtons.None)
        {
          dragHasDone = false;
          HtmlElement element = null;
          if (e.MouseButtonsPressed == MouseButtons.Left)
            element = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);
          else
          {
            element = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);
            //element = (HtmlElement)sender;
          }
          dragNode = map.GetXmlNode(GetModel().ActivePosition.XmlDocument, new HtmlElementWrapper(element));
          GetModel().SelectedNodes[GetModel().ActivePosition.Persist] = dragNode;
        }
      }
    }

    private BrowserHtmlMap map = new BrowserHtmlMap();
    private HtmlElementsHighlighter highlighter;
    private string cachedSelectedNode;
    private HtmlElementWrapper cachedSelectedElementWrapper = null;

    private List<HtmlElementWrapper> GetSelectedElements()
    {
      if (GetModel().Mode == Model.WorkMode.Browse)
      {
        cachedSelectedNode = null;
        cachedSelectedElementWrapper = null;
      }
      else if (Browser.ReadyState == WebBrowserReadyState.Complete)
      //if (GetModel().SelectedNode != cachedSelectedNode)
      {
        cachedSelectedNode = GetModel().GetSelectedNode(GetModel().ActivePosition);
        XmlNode node = XmlHlp.SelectSingleNode(GetModel().ActivePosition.XmlDocument, cachedSelectedNode);
        cachedSelectedElementWrapper = map.GetHtmlElement(Browser, node);
      }

      List<HtmlElementWrapper> result = new List<HtmlElementWrapper>();
      if (cachedSelectedElementWrapper != null)
        result.Add(cachedSelectedElementWrapper);
      return result;
    }

    private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
    {
      //if (GetModel().Mode == Model.WorkMode.Parse)
      //  e.Cancel = true;
    }

    private void Navigate(string adress)
    {
      //Browser.Navigate(Uri.EscapeUriString(adress));
      Browser.Navigate(adress);
    }

    #region Interface Helpers
    private void urlComboBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter && GetModel().Mode == Model.WorkMode.Browse)
      {
        Navigate(urlComboBox.Text);
      }
    }
    private void urlComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (GetModel().Mode == Model.WorkMode.Browse)
      {
        Navigate(urlComboBox.Text);
      }
    }
    private void toolStripButton1_Click(object sender, EventArgs e)
    {
      if (GetModel().Mode == Model.WorkMode.Browse)
      {
        Browser.GoBack();
      }
    }
    private void toolStripButton2_Click(object sender, EventArgs e)
    {
      if (GetModel().Mode == Model.WorkMode.Browse)
      {
        Browser.GoForward();
      }
    }
    private void goButton_Click(object sender, EventArgs e)
    {
      if (GetModel().Mode == Model.WorkMode.Browse)
      {
        Navigate(urlComboBox.Text);
      }
    }
    private void toolStripButton1_Click_1(object sender, EventArgs e)
    {
      WebPosition pos = WebPosition.Parse(Browser.Url.AbsoluteUri);
      GetModel().PositionsList.Add(pos);
    }

    private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
    {
      //if (GetModel().Mode == Model.WorkMode.Parse)
      //  InitDocumentForParse();
      //if (GetModel().Mode == Model.WorkMode.Browse)
      //  urlComboBox.Text = e.Url.AbsoluteUri;
    }
    #endregion

    Getter<Model> GetModel = delegate { return new Model(); };

    #region IView Members

    public void InitModel(Getter<Model> modelGetter)
    {
      GetModel = modelGetter;
      cachedWorkMode = GetModel().Mode;
    }
    public void ForceSynchronize()
    {
      if (GetModel().Mode == Model.WorkMode.Parse)
      {
        if (cachedWorkMode != GetModel().Mode)
        {
          //Значит только переключили режим
          SwitchToParseMode();
        }
        SynchronizeCurrentPosition();
        ParseModeSynchronize();
      }
      else if (GetModel().Mode == Model.WorkMode.Browse)
      {
        if (cachedWorkMode != GetModel().Mode)
          //Значит только переключили режим
          SwitchToBrowseMode();
        SynchronizeCurrentPosition();
        if (Browser != null && Browser.Url != null && Browser.ReadyState == WebBrowserReadyState.Complete)
        {
          if (!Uri.Equals(cachedBrowseUrl, Browser.Url))
          {
            cachedBrowseUrl = Browser.Url;
            urlComboBox.Text = Browser.Url.AbsoluteUri;
          }
          GetModel().ActivePosition = WebPosition.Parse(Browser.Url.AbsoluteUri);
        }
      }
      cachedWorkMode = GetModel().Mode;
    }
    Uri cachedBrowseUrl = null;

    private void SwitchToParseMode()
    {
      Browser.AllowWebBrowserDrop = false;
      toolStrip1.Enabled = false;
      cachedCurrentPosition = null;
    }

    private void SwitchToBrowseMode()
    {
      Browser.Stop();
      Browser.Visible = true;
      Browser.AllowWebBrowserDrop = true;
      Browser.AllowNavigation = true;
      progressBar1.Visible = false;
      DeInitDocumentForParse();
      toolStrip1.Enabled = true;
      cachedCurrentPosition = null;

      //Hack - сбрасываем кэш отображения
      map.GetHtmlElement(Browser, GetModel().ActivePosition.XmlDocument.DocumentElement);
      Navigate(GetModel().ActivePosition.Url.AbsoluteUri);
    }

    private Model.WorkMode? cachedWorkMode = null;
    private WebPosition cachedCurrentPosition = null;

    private void ParseModeSynchronize()
    {
      if (Browser.ReadyState == WebBrowserReadyState.Complete)
      {
        if (Browser.AllowNavigation)
        {
          Browser.AllowNavigation = false;
          InitDocumentForParse();
        }
      }
      if (Browser.ReadyState == WebBrowserReadyState.Complete && (progressBar1.Visible || !Browser.Visible))
      {
        Browser.Visible = true;
        progressBar1.Visible = false;
        progressBar1.Value = 0;
      }
      else if (Browser.ReadyState != WebBrowserReadyState.Complete && !progressBar1.Visible)
      {
        Browser.Visible = false;
        progressBar1.Visible = true;
      }

      if (Browser.Focused)
      {
        if (Browser.Document != null && Browser.Document.DomDocument != null)
        {
          mshtml.HTMLDocumentClass nativeDoc = webBrowser1.Document.DomDocument as mshtml.HTMLDocumentClass;
          if (nativeDoc != null && nativeDoc.selection != null && nativeDoc.selection.type != "None")
          {
            //Console.WriteLine(nativeDoc.selection.type);
            nativeDoc.selection.empty();
            //nativeDoc.selection.clear();
          }
        }
      }
    }

    private void SynchronizeCurrentPosition()
    {
      if (!WebPosition.Equals(cachedCurrentPosition, GetModel().ActivePosition))
      {
        cachedCurrentPosition = GetModel().ActivePosition;
        Browser.AllowNavigation = true;
        Browser.Navigate(cachedCurrentPosition.Url.AbsoluteUri);
        urlComboBox.Text = cachedCurrentPosition.Url.ToString();
      }
    }

    #endregion

    private void WebBrowserWrapper_DragDrop(object sender, DragEventArgs e)
    {
      MessageBox.Show("Drop");
    }

    private void WebBrowserWrapper_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = e.AllowedEffect;
    }

    private void webBrowser1_BeforeNewWindow(object sender, ExtendedNavigatingEventArgs e)
    {
      if (GetModel().Mode == Model.WorkMode.Parse)
        e.Cancel = true;
      Console.WriteLine(e.Url);
    }

  }
}
