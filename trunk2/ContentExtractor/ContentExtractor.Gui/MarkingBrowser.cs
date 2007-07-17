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

namespace ContentExtractor.Gui
{
  public partial class MarkingBrowser : UserControl
  {
    // HACK: Need to make Sharp Develop designer work properly
    public MarkingBrowser()
    {
      //
      // The InitializeComponent() call is required for Windows Forms designer support.
      //
      InitializeComponent();
    }

    internal void SetState(State state)
    {
      if (this.state == null)
      {
        this.state = state;
        state.BrowserUriChanged += new EventHandler(BrowserUriChanged);
        this.BrowserUriChanged(this, EventArgs.Empty);
      }
      else
      {
        throw new InvalidOperationException("Cannot assign state twice");
      }
    }

    public MarkingBrowser(State state)
    {
      InitializeComponent();
      SetState(state);
    }

    private State state;

    public ExtendedWebBrowser Browser
    {
      get { return webBrowser1; }
    }

    private void BrowserUriChanged(object sender, EventArgs e)
    {
      Browser.Navigate(state.BrowserUri);
    }

    private void webBrowser1_BeforeNavigate(object sender, ExtendedNavigatingEventArgs e)
    {
      e.Cancel = !Uri.Equals(Utils.ParseUrl(e.Url), state.BrowserUri);
      if (!e.Cancel && Browser.Document != null)
        DeInitDocument(Browser.Document);
    }

    private void webBrowser1_BeforeNewWindow(object sender, ExtendedNavigatingEventArgs e)
    {
      e.Cancel = true;
    }

    private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
      InitDocument(Browser.Document);
    }

    private void InitDocument(HtmlDocument doc)
    {
      doc.Click += doc_Click;
      doc.MouseMove += doc_MouseMove;
      currentHighlighter = new BrowserHighlighter("BACKGROUND-COLOR: #CCFF99");
      selectedHighlighter = new BrowserHighlighter("BACKGROUND-COLOR: #880000");
    }
    private void DeInitDocument(HtmlDocument doc)
    {
      doc.Click -= doc_Click;
      doc.MouseMove -= doc_MouseMove;
      currentHighlighter = null;
      selectedHighlighter = null;
    }

    void doc_Click(object sender, HtmlElementEventArgs e)
    {
      e.BubbleEvent = false;
      HtmlElement element = Browser.Document.GetElementFromPoint(e.ClientMousePosition);
      selectedHighlighter.Highlight(element);
    }

    void doc_MouseMove(object sender, HtmlElementEventArgs e)
    {
      e.BubbleEvent = false;
      if (e.MouseButtonsPressed == MouseButtons.None)
      {
        HtmlElement element = Browser.Document.GetElementFromPoint(e.ClientMousePosition);
        currentHighlighter.Highlight(element);
      }
    }

    private BrowserHighlighter currentHighlighter;
    private BrowserHighlighter selectedHighlighter;

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (selectedHighlighter != null)
        selectedHighlighter.Force();
    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {

    }
  }

  internal class BrowserHighlighter
  {
    public BrowserHighlighter(string style)
    {
      Utils.CheckNotNull(style);
      this.style = style;
    }
    private string style;

    public void Highlight(HtmlElement element)
    {
      if (highlightedElement != element)
      {
        if (highlightedElement != null)
          highlightedElement.Style = highlightedElementStyle;
        if (element != null)
        {
          highlightedElementStyle = element.Style;
          highlightedElement = element;
          highlightedElement.Style += ";" + style;
        }
        else
        {
          highlightedElement = null;
          highlightedElementStyle = null;
        }
      }
    }
    public void Force()
    {
      if (highlightedElement != null &&
        highlightedElement.Style != highlightedElementStyle + ";" + style)
      {
        highlightedElement.Style = highlightedElementStyle + ";" + style;
      }
    }
    private HtmlElement highlightedElement = null;
    private string highlightedElementStyle = null;
  }
}
