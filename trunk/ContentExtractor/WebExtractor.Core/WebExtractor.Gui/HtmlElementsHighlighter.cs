using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MetaTech.Library;
using System.Drawing;

namespace ContentExtractor.Gui
{
  using ElementsDictonary = Dictionary<string, Getter<List<HtmlElementWrapper>>>;
  using HighlightDictionary = Dictionary<HtmlElementWrapper, HtmlElementWrapper>;

  public class HtmlElementsHighlighter : SoftTech.Gui.ControlSynchronizer
  {
    public HtmlElementsHighlighter(WebBrowser browser)
    {
      this.browser = browser;
      base.UpdateTimer(true);
      //this.browser.Navigating += new WebBrowserNavigatingEventHandler(browser_Navigating);
    }

    private WebBrowser browser;

    public ElementsDictonary Links = new ElementsDictonary();

    private HighlightDictionary divDict = new HighlightDictionary();
    private Dictionary<HtmlElementWrapper, string> styles = new Dictionary<HtmlElementWrapper, string>();

    Uri cachedBrowserUri = null;

    public void ForceSyncronize()
    {
      if (!browser.IsBusy)
      {
        if (browser.Url != cachedBrowserUri)
        {
          Clear();
          cachedBrowserUri = browser.Url;
        }

        List<HtmlElementWrapper> allElements = new List<HtmlElementWrapper>();
        foreach (string style in Links.Keys)
        {
          List<HtmlElementWrapper> elements = Links[style]();
          allElements.AddRange(elements);
          if (elements != null)
          {
            foreach (HtmlElementWrapper element in elements)
            {
              //if (!styles.ContainsKey(element))
              //{
              //  string myStyle = ";FILTER: progid:DXImageTransform.Microsoft.Alpha(opacity=90);BACKGROUND-COLOR: #ff0000";
              //  //string myStyle = "border-color:red;border-style:dashed;border-width:5;margin:-5;";//margin:-5;
              //  styles[element] = element.Element.Style;
              //  element.Element.Style = (styles[element] ?? "") + myStyle;
              //  //style + "z-index:1000";
              //}
              if (element.Element.Document != null && element.Element.Document.Body != null)
              {
                if (!divDict.ContainsKey(element))
                {
                  FlowHlp.SafeBlock("Создание подсвечивающих div-ов на события", delegate
                  {
                    HtmlElementWrapper newDiv = new HtmlElementWrapper(element.Element.Document.Body).CreateChild("div");
                    newDiv.Element.MouseDown += new HtmlElementEventHandler(Element_MouseDown);
                    divDict[element] = newDiv;
                  });
                }
                HtmlElementWrapper div = divDict[element];
                string divStyle = style;
                divStyle += ";position:absolute;left:{0}px;top:{1}px;width:{2}px;height:{3}px;";// z-index:-1000;";
                Rectangle rect = element.AbsoluteRectangle;
                divStyle = string.Format(divStyle, rect.Left, rect.Top, rect.Width, rect.Height);
                div.Element.Style = divStyle;
              }
            }
          }
        }

        Getter<HtmlElementWrapper, HtmlElementWrapper> one = delegate(HtmlElementWrapper w) { return w; };
        CollectionSynchronizer.CompareResults<HtmlElementWrapper, HtmlElementWrapper> compareResults =
        CollectionSynchronizer.Compare<HtmlElementWrapper, HtmlElementWrapper, HtmlElementWrapper>(allElements, one, divDict.Keys, one);
        //CollectionSynchronizer.Compare<HtmlElementWrapper, HtmlElementWrapper, HtmlElementWrapper>(allElements, one, styles.Keys, one);
        foreach (HtmlElementWrapper elementToHide in compareResults.OnlyRight)
        {
          //elementToHide.Element.Style = styles[elementToHide];
          //styles.Remove(elementToHide);

          //divDict[elementToHide].Element.MouseDown -= Element_MouseDown;
          divDict[elementToHide].DeleteSelf();
          divDict.Remove(elementToHide);
        }
      }
    }

    void Element_MouseDown(object sender, HtmlElementEventArgs e)
    {
      DateTime clickTime = DateTime.UtcNow;
      //TraceHlp2.AddMessage("div mouseDown @ {0}.{1}", clickTime, clickTime.Millisecond);
      e.BubbleEvent = false;
      HtmlElement div = (HtmlElement)sender;
      div.Style += "visibility:hidden;";
      HtmlElement to = browser.Document.GetElementFromPoint(e.ClientMousePosition);
      div.Style += "visibility:visible;";
      try
      {
        //Hack - не знаю как напрямую передать в обработчик события
        ButtonPressed = e.MouseButtonsPressed;
        to.RaiseEvent("onMouseDown");
      }
      finally
      {
        ButtonPressed = null;
      }
    }

    public static MouseButtons? ButtonPressed = null;
    protected override Control Control_ForSync
    {
      get { return browser; }
    }

    protected override TimeSpan UpdateInterval_ForSync
    {
      get { return TimeSpan.FromSeconds(0.3); }
    }

    protected override void timer_Tick(object sender, EventArgs e)
    {
      ForceSyncronize();
    }

    private void Clear()
    {
      foreach (HtmlElementWrapper element in divDict.Values)
        element.DeleteSelf();
      divDict.Clear();
      //Links.Clear();
    }

  }
}
