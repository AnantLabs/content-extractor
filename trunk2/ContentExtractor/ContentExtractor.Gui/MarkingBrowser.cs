//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 06.07.2007
// Time: 19:22
//

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ContentExtractor.Core;
using System.Collections.Generic;
using log4net;

namespace ContentExtractor.Gui
{
  public partial class MarkingBrowser : UserControl
  {
    public MarkingBrowser()
    {
      // The InitializeComponent() call is required for Windows Forms designer support.
      InitializeComponent();
    }

    public void SetState(State state)
    {
      if (this.state == null)
      {
        this.state = state;

        highlighter = new BrowserHighlighter(Browser);
        highlighter.AddHighlighter(
          delegate
          {
            if (Browser.Document != null && state != null && state.SelectedNodeXPath != null)
              return Utils.SelectHtmlElement(Browser.Document, state.SelectedNodeXPath);
            return null;
          },
          "BACKGROUND-COLOR: #FFAAAA");
        Getter<HtmlElement> currentGetter =
            delegate
            {
              if (Browser.Document != null)
              {
                Point p = Browser.PointToClient(Control.MousePosition);
                if (Browser.ClientRectangle.Contains(p))
                {
                  HtmlElement element = Browser.Document.GetElementFromPoint(p);
                  return element;
                }
              }
              return null;
            };
        highlighter.AddHighlighter(currentGetter, "BACKGROUND-COLOR: #CCFF99");

        this.components.Add(highlighter);
      }
      else
      {
        Logger.Warn("Cannot assign state twice");
      }
    }

    private State state;
    private BrowserHighlighter highlighter;

    public void ForceSynchronize()
    {
      ForceBrowserPosition();
    }

    public ExtendedWebBrowser Browser
    {
      get { return webBrowser1; }
    }

    private void webBrowser1_BeforeNavigate(object sender, ExtendedNavigatingEventArgs e)
    {
      Uri eventUri = new Uri(e.Url);
      e.Cancel = !Uri.Equals(eventUri.AbsoluteUri, state.BrowserPosition.Url.AbsoluteUri) ||
        (Browser.Url != null && Uri.Equals(Browser.Url.AbsoluteUri, eventUri.AbsoluteUri));
      if (e.Cancel)
        Logger.DebugFormat("Browser navigation to '{0}' canceled", e.Url);
      else
        Logger.InfoFormat("Browser is navigating to {0}", e.Url);
      if (!e.Cancel && Browser.Document != null)
        DeInitDocument(Browser.Document);
    }

    private void webBrowser1_BeforeNewWindow(object sender, ExtendedNavigatingEventArgs e)
    {
      e.Cancel = true;
    }

    private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
      Logger.DebugFormat("Navigation to {0} completed", e.Url.AbsoluteUri);
      //InitDocument(Browser.Document);
    }

    private void InitDocument(HtmlDocument doc)
    {
      doc.Click += doc_Click;
      Logger.DebugFormat("Document {0} inited", doc.Url.AbsoluteUri);
    }
    private void DeInitDocument(HtmlDocument doc)
    {
      doc.Click -= doc_Click;
      Logger.DebugFormat("Document {0} deinited", doc.Url.AbsoluteUri);
    }

    void doc_Click(object sender, HtmlElementEventArgs e)
    {
      e.BubbleEvent = false;
      HtmlElement element = Browser.Document.GetElementFromPoint(e.ClientMousePosition);
      state.SelectedNodeXPath = Utils.HtmlElementXPath(element);
      Logger.DebugFormat("Clicked on '{0}'", state.SelectedNodeXPath);
    }

    private static ILog Logger = LogManager.GetLogger(typeof(MarkingBrowser)); //Logger.Instance; 

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
      if (state.Project.Template.CanAutoModifyTemplate)
        state.Project.Template.AddColumn(state.SelectedNodeXPath);
      else
        MessageBox.Show(Properties.Resources.NotAbleToAddSpecificColumnWarning,
            Properties.Resources.NotAbleToAddSpecificColumnWarningCaption,
            MessageBoxButtons.OK);
    }

    public bool IsBusy
    {
      get
      {
        return Browser.ReadyState == WebBrowserReadyState.Loading;
        //!(Browser.ReadyState == WebBrowserReadyState.Complete || 
        //Browser.ReadyState == WebBrowserReadyState.Uninitialized);
      }
    }

    private Uri lastLoadedUri = null;

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (!IsBusy && lastLoadedUri != Browser.Url)
      {
        lastLoadedUri = Browser.Url;
        if (Browser.Document != null)
        {
          InitDocument(Browser.Document);
        }
      }

      toolStripButton1.Enabled = !string.IsNullOrEmpty(state.SelectedNodeXPath);

      //bool isBusy = IsBusy;
      //if (isBusy != _IsBusyCached_timer1_Tick_)
      //{
      //  if (!isBusy && Browser.Document != null)
      //    InitDocument(Browser.Document);
      //  _IsBusyCached_timer1_Tick_ = isBusy;
      //}
      ForceBrowserPosition();
    }

    private void ForceBrowserPosition()
    {
      if (Browser != null && Browser.ReadyState != WebBrowserReadyState.Loading &&
          (Browser.Url == null || 
            Browser.Url.AbsoluteUri != state.BrowserPosition.Url.AbsoluteUri))
      {
        Browser.Navigate(state.BrowserPosition.Url.AbsoluteUri);
      }
    }

    private void toolStripButton2_Click(object sender, EventArgs e)
    {
      if (Browser != null)
      {
        Browser.Refresh();
        lastLoadedUri = null; //HACK - to refresh highlighter.
      }
    }
  }

  internal class BrowserHighlighter : Component
  {
    public BrowserHighlighter(WebBrowser browser)
    {
      Utils.CheckNotNull(browser);
      this.browser = browser;

      this.timer = new Timer();
      this.timer.Interval = 250;
      this.timer.Tick += new EventHandler(timer_Tick);
      this.timer.Enabled = true;
    }

    /// <summary>
    /// The first highlighter pair is considered as the most strong, so 
    /// if first highlighter says to highlight element other highlighters
    /// won't highlight it.
    /// </summary>
    public void AddHighlighter(Getter<HtmlElement> selector, string style)
    {
      Utils.CheckNotNull(selector);
      Utils.CheckNotNull(style);
      highlighters.Add(new HighlightData(selector, style));
    }

    private class HighlightData
    {
      public HighlightData(Getter<HtmlElement> selector, string style)
      {
        this.selector = selector;
        this.style = style;
        this.lastElement = null;
        this.lastElementStyle = null;
      }

      public Getter<HtmlElement> selector;
      public string style;
      public HtmlElement lastElement;
      public string lastElementStyle;
    }

    private WebBrowser browser;
    private List<HighlightData> highlighters = new List<HighlightData>();
    private Timer timer;
    private Dictionary<HtmlElement, string> original_styles = new Dictionary<HtmlElement, string>();

    private static bool ElementShouldBeCleaned(HtmlElement elem, List<HtmlElement> fresh_elems, int index)
    {
      Predicate<HtmlElement> is_overriden = delegate(HtmlElement e) { return elem == e; };
      return elem != null &&
        (elem != fresh_elems[index] || (index > 0 && fresh_elems.FindIndex(0, index - 1, is_overriden) >= 0));
    }

    void timer_Tick(object sender, EventArgs e)
    {
      if (browser.Document != null && browser.ReadyState != WebBrowserReadyState.Loading)
      {
        List<HtmlElement> fresh_elements = new List<HtmlElement>();
        List<HtmlElement> old_elements = new List<HtmlElement>();
        foreach (HighlightData data in highlighters)
        {
          HtmlElement fresh = data.selector();
          if (!fresh_elements.Contains(fresh))
            fresh_elements.Add(data.selector());
          else
            fresh_elements.Add(null);
          old_elements.Add(data.lastElement);
        }
        for (int i = 0; i < highlighters.Count; i++)
        {
          if (ElementShouldBeCleaned(old_elements[i], fresh_elements, i))
            old_elements[i].Style = highlighters[i].lastElementStyle;
        }
        for (int i = 0; i < highlighters.Count; i++)
        {
          if (fresh_elements[i] != old_elements[i])
          {
            if (fresh_elements[i] != null)
            {
              highlighters[i].lastElementStyle = fresh_elements[i].Style;
              fresh_elements[i].Style = fresh_elements[i].Style + ";" + highlighters[i].style;
            }
            highlighters[i].lastElement = fresh_elements[i];
          }
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        timer.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
