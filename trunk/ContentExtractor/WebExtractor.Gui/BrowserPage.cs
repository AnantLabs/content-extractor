using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ContentExtractor.Core;
using MetaTech.Library;
using System.Xml;
using System.Xml.XPath;

namespace ContentExtractor.Gui
{
  public partial class BrowserPage : UserControl, IView
  {
    public BrowserPage(WebPosition position)
    {
      this.innerPosition = position;
      InitializeComponent();
      //this.TabsContextMenu.Tag = this;

      highlighter = new HtmlElementsHighlighter(this.Browser);
      string style = "FILTER: progid:DXImageTransform.Microsoft.Alpha(opacity=30);BACKGROUND-COLOR: #ff0000";
      highlighter.Links[style] = GetSelectedElements;
    }

    WebPosition innerPosition;

    public WebPosition Position
    {
      get { return innerPosition; }
    }

    private string title = string.Empty;
    public string Title
    {
      get
      {
        if (IsBusy)
          title = "Loading ...";
        else if (Browser.Document != null)
          title = Browser.Document.Title;
        else
          title = "Blank page";
        return title;
      }
    }

    public bool IsBusy
    {
      get
      {
        return !(Browser.ReadyState == WebBrowserReadyState.Complete || Browser.ReadyState == WebBrowserReadyState.Uninitialized);
      }
    }

    private WebPosition.PersistStruct cachedPosition = WebPosition.EmptyPosition.Persist;

    KeyValuePair<WebPosition.PersistStruct, HtmlDocument> lastParsedDocument = new KeyValuePair<WebPosition.PersistStruct, HtmlDocument>();
    private HtmlElementsHighlighter highlighter;

    public void ForceSynchronize()
    {
      Browser.Visible = CurrentModel.Mode == Model.WorkMode.Browse || !IsBusy;
      Browser.IsWebBrowserContextMenuEnabled = CurrentModel.Mode == Model.WorkMode.Browse;
      progressBar.Visible = IsBusy;
      Browser.AllowNavigation = CurrentModel.Mode == Model.WorkMode.Browse || IsBusy;
      toolStrip1.Enabled = CurrentModel.Mode == Model.WorkMode.Browse;
      Browser.AllowWebBrowserDrop = CurrentModel.Mode == Model.WorkMode.Browse;

      if (/*!IsBusy && */!WebPosition.Equals(cachedPosition, Position))
      {
        if (!Uri.Equals(cachedPosition, Position.Persist))
        {
          Browser.AllowNavigation = true;
          //Browser.Navigate(Position.Url.AbsoluteUri);
          Browser.Navigate(Position.Url);
          Console.WriteLine("Navigate {0}", Position.Url);
          urlComboBox.Text = Position.Url.AbsoluteUri;
        }
        cachedPosition = Position.Persist;
      }
      if (CurrentModel.Mode == Model.WorkMode.Browse)
      {
        backButton.Enabled = Browser.CanGoBack;
        forwardButton.Enabled = Browser.CanGoForward;
        if (!IsBusy && Browser.Url != null && !Uri.Equals(Position.Url, Browser.Url))
        {
          Position.Set(WebPosition.Parse(Browser.Url.AbsoluteUri));
          cachedPosition = Position.Persist;
          urlComboBox.Text = cachedPosition.Url;
        }
      }
      if (CurrentModel.Mode == Model.WorkMode.Parse)
      {
        if (Browser.Focused && Browser.Document != null && Browser.Document.DomDocument != null)
        {
          mshtml.HTMLDocumentClass nativeDoc = Browser.Document.DomDocument as mshtml.HTMLDocumentClass;
          if (nativeDoc != null && nativeDoc.selection != null && nativeDoc.selection.type != "None")
            nativeDoc.selection.empty();
        }
        if (!IsBusy && Browser.Document != null)
        {
          if (!WebPosition.Equals(lastParsedDocument.Key, cachedPosition))
          {
            if (lastParsedDocument.Value != null)
              DeinitDocument(lastParsedDocument.Value);
            lastParsedDocument = new KeyValuePair<WebPosition.PersistStruct, HtmlDocument>(cachedPosition, Browser.Document);
            InitDocument(Browser.Document);
          }
        }
        if (!contextMenuStrip1.Visible) //Browser.Focused
          SetLastPointedElement(CurrentPointedElement);
      }
    }

    private List<HtmlElementWrapper> GetSelectedElements()
    {
      List<HtmlElementWrapper> result = new List<HtmlElementWrapper>();

      if (CurrentModel.Mode == Model.WorkMode.Parse && !IsBusy && CurrentModel.SelectedNodes.ContainsKey(Position.Persist))
      {
        XmlNode node = XmlHlp.SelectSingleNode(Position.XmlDocument, CurrentModel.GetSelectedNode(Position));
        HtmlElementWrapper element = map.GetHtmlElement(Browser, node);
        if (element != null)
          result.Add(element);
      }
      return result;
    }

    private void InitDocument(HtmlDocument htmlDocument)
    {
      if (htmlDocument != null)
      {
        htmlDocument.MouseDown += htmlDocument_MouseDown;
        htmlDocument.MouseMove += htmlDocument_MouseMove;
      }
    }

    private void DeinitDocument(HtmlDocument htmlDocument)
    {
      htmlDocument.MouseDown -= htmlDocument_MouseDown;
      htmlDocument.MouseMove -= htmlDocument_MouseMove;
    }

    bool dragHasDone = false;
    string dragNode = string.Empty;
    private BrowserHtmlMap map = new BrowserHtmlMap();
    private DateTime lastMouseDown = DateTime.MinValue;

    private HtmlElement lastPointedElement = null;
    private string lastPointedElementStyle = string.Empty;

    private HtmlElement CurrentPointedElement
    {
      get
      {
        if (Browser.Document != null)
        {
          Point p = Browser.PointToClient(Control.MousePosition);
          if (Browser.ClientRectangle.Contains(p))
          {
            HtmlElement element = Browser.Document.GetElementFromPoint(p);
            if (element != null && element.TagName.ToLower() == "body")
              Console.WriteLine(element);
            return element;
          }
        }
        return null;
      }
    }

    private void SetLastPointedElement(HtmlElement element)
    {
      if (lastPointedElement != element)
      {
        if (lastPointedElement != null)
          lastPointedElement.Style = lastPointedElementStyle;
        if (element != null)
        {
          lastPointedElementStyle = element.Style;
          element.Style += ";BACKGROUND-COLOR: #CCFF99";
          lastPointedElement = element;
          TraceHlp2.AddMessage("focus {0}, Focused {1}, status text {2}", Browser.ContainsFocus, Browser.Focused, Browser.StatusText);
        }
      }
    }

    void htmlDocument_MouseDown(object sender, HtmlElementEventArgs e)
    {
      DateTime clickTime = DateTime.UtcNow;
      e.BubbleEvent = false;
      MouseButtons buttons = e.MouseButtonsPressed;
      if (buttons == MouseButtons.None && HtmlElementsHighlighter.ButtonPressed.HasValue)
        buttons = HtmlElementsHighlighter.ButtonPressed.Value;

      dragHasDone = false;
      HtmlElement element = Browser.Document.GetElementFromPoint(e.ClientMousePosition);
      if (element != null && !WebExtractorHlp.TagsAreSame(element.TagName, "html"))
      {
        string curNode = map.GetXmlNode(Position.XmlDocument, new HtmlElementWrapper(element));
        if (CurrentModel.Mode == Model.WorkMode.Parse &&
          buttons == MouseButtons.Left && curNode == dragNode &&
          clickTime - lastMouseDown < TimeSpan.FromSeconds(1))
        {
          HtmlDocDoubleClick(element);
        }
        dragNode = curNode;
        CurrentModel.SelectedNodes[Position.Persist] = dragNode;
        lastMouseDown = clickTime;
      }
      if (buttons == MouseButtons.Right)
      {
        contextMenuStrip1.Tag = dragNode;
        contextMenuStrip1.Show(Browser, e.MousePosition);
      }
    }

    private void HtmlDocDoubleClick(HtmlElement element)
    {
      if (CurrentModel.Mode == Model.WorkMode.Parse)
      {
        WebPosition pos = Position.GetLinkedPosition(dragNode);
        if (pos != null)
          CurrentModel.PositionsList.Add(pos);
      }
    }

    void htmlDocument_MouseMove(object sender, HtmlElementEventArgs e)
    {
      e.BubbleEvent = false;
      if (e.MouseButtonsPressed == MouseButtons.Left && !dragHasDone)
      {
        dragHasDone = true;
        WebExtractorHlp.DoDragHtmlNode(this, dragNode);
      }
    }

    private void Browser_BeforeNewWindow(object sender, ExtendedNavigatingEventArgs e)
    {
      if (CurrentModel.Mode == Model.WorkMode.Browse)
      {
        WebPosition position = WebPosition.Parse(e.Url);
        if (position != null)
          CurrentModel.CurrentPositions.Add(position);
      }
      e.Cancel = true;
    }

    private void urlComboBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        SetPosition(urlComboBox.Text);
      }
    }

    private void SetPosition(string adress)
    {
      cachedPosition = WebPosition.EmptyPosition.Persist;
      WebPosition pos = WebPosition.Parse(adress);
      if (pos != null)
      {
        Position.Set(pos);
      }
    }

    private void urlComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      Console.WriteLine(urlComboBox.SelectedItem);
    }

    private void goButton_Click(object sender, EventArgs e)
    {
      SetPosition(urlComboBox.Text);
    }

    private void backButton_Click(object sender, EventArgs e)
    {
      Browser.GoBack();
    }

    private void forwardButton_Click(object sender, EventArgs e)
    {
      Browser.GoForward();
    }


    #region IView Members

    private Model CurrentModel
    { get { return modelGetter(); } }
    private Getter<Model> modelGetter = delegate { return new Model(); };


    public void InitModel(Getter<Model> modelGetter)
    {
      this.modelGetter = modelGetter;
    }

    #endregion

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CurrentModel.CurrentPositions.Remove(Position);
    }

    private void toolStripButton2_Click(object sender, EventArgs e)
    {
      CurrentModel.PositionsList.Add(Position);
    }

    private void createColumnFromNodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string node = contextMenuStrip1.Tag as string;
      if (!string.IsNullOrEmpty(node))
        CurrentModel.Template.AddXPathColumnToPosition(0, CurrentModel.Template.Columns.Count, node);
    }
  }
}
