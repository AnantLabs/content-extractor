using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using MetaTech.Library;
using System.Text.RegularExpressions;
using ContentExtractor.Core;

namespace ContentExtractor.Gui
{
  public class HtmlElementWrapper
  {
    public HtmlElementWrapper(HtmlElement element)
    {
      this.element = element;
    }

    private HtmlElement element;
    public HtmlElement Element
    {
      get { return element; }
    }


    private HtmlElementWrapper parent = null;
    private bool parentInited = false;
    public HtmlElementWrapper Parent
    {
      get
      {
        if (!parentInited)
        {
          parent = GetParent();
          parentInited = true;
        }
        return parent;
        //throw new Exception("HtmlElementWrapper.Parent - can't find real parent for HtmlElement " + this.ToString());
      }
    }

    private HtmlElementWrapper GetParent()
    {
      HtmlElement element = Element;
      if (element.Parent != null)
      {
        HtmlElementWrapper parent = new HtmlElementWrapper(element.Parent);
        int guard = 0;
        foreach (HtmlElementWrapper wrapper in TreeCollectionHlp.WideVisit(parent, delegate(HtmlElementWrapper w) { return w.Children; }))
        {
          guard++;
          if (guard > 15)
            break;
          if (wrapper.Children.Contains(this))
            return wrapper;
        }
        return new HtmlElementWrapper(element.Parent);
      }
      return null;
    }

    public string Name
    {
      get
      {
        HtmlElement element = Element;
        return element == null ? null : element.TagName;
      }
    }

    private string _path = null;

    public string Path
    {
      get
      {
        HtmlElement element = Element;
        if (_path == null)
        {
          if (element == null)
            _path = "";
          else if (element.Parent != null)
          {
            _path = string.Format("{0}/{1}[{2}]", Parent.Path,
              element.TagName, this.IndexToParent);
          }
          else
            _path = "/" + element.TagName + "[1]";
        }
        return _path;

      }
    }

    int IndexToParent
    {
      get
      {
        int index = 0;
        if (Parent != null)
        {
          foreach (HtmlElementWrapper brother in Parent.Children)
            if (brother.Name == this.Name)
            {
              index++;
              if (brother.Equals(this))
                return index;
            }
        }
        return index;
      }
    }

    private List<HtmlElementWrapper> children = null;

    public List<HtmlElementWrapper> Children
    {
      get
      {
        if (children == null)
        {
          List<HtmlElement> result = this.PseudoChildren;
          if (result == null || result.Count == 0)
          {
            result = new List<HtmlElement>();
            HtmlElement element = Element;
            if (element != null)
            {
              List<HtmlElement> pseudoGrandChilren = new List<HtmlElement>();
              foreach (HtmlElement child in element.Children)
              {
                if (child.TagName != "")
                {
                  result.Add(child);
                }
                pseudoGrandChilren.AddRange(new HtmlElementWrapper(child).PseudoChildren);
              }
              foreach (HtmlElement grandChild in pseudoGrandChilren)
                result.Remove(grandChild);
            }
          }
          children = result.ConvertAll<HtmlElementWrapper>(delegate(HtmlElement elem) { return new HtmlElementWrapper(elem); }); ;
        }
        return children;
      }
    }
    #region Children helpers
    private List<HtmlElement> PseudoChildren
    {
      get
      {
        HtmlElement element = Element;
        if (element != null && element.Children.Count == 0 && element.Parent != null)
        {
          HtmlElementWrapper parent = new HtmlElementWrapper(element.Parent);
          int parentIndex = parent.FindChildNativeIndex(delegate(HtmlElement elem) { return elem == element; });
          if (parentIndex >= 0)
          {
            List<HtmlElement> result = new List<HtmlElement>();
            for (int i = parentIndex + 1; i < element.Parent.Children.Count; i++)
            {
              if (element.Parent.Children[i].TagName == "/" + element.TagName)
                return result;
              result.Add(element.Parent.Children[i]);
            }
          }
        }
        return new List<HtmlElement>();
      }
    }
    private int FindChildNativeIndex(Predicate<HtmlElement> match)
    {
      HtmlElement element = Element;
      if (element != null)
      {
        for (int i = 0; i < element.Children.Count; i++)
          if (match(element.Children[i]))
            return i;
      }
      return -1;
    }
    #endregion

    public void Click()
    {
      try
      {
        HtmlElement element = Element;
        element.RaiseEvent("onClick");
      }
      catch (Exception exc)
      {
        throw new Exception("HtmlElementWrapper.Click() Can't raise onClick event", exc);
      }
    }

    private static HtmlElementWrapper GoDeepThrowAlienTags(HtmlElementWrapper parent, string nodeName)
    {
      List<HtmlElementWrapper> children = parent.Children;

      while (children.Count == 1 && !WebExtractorHlp.TagsAreSame(children[0].Name, nodeName))
      {
        parent = children[0];
        children = parent.Children;
      }
      return parent;
    }

    public static HtmlElementWrapper XmlNode2HtmlElement(XmlNode xmlNode, WebBrowser browser)
    {
      if (xmlNode == null || browser == null)
        return null;

      if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name.ToLower() == "body")
      {
        if (browser.Document != null)
          return new HtmlElementWrapper(browser.Document.Body);
        else
          return null;
      }
      else
      {
        HtmlElementWrapper parent = XmlNode2HtmlElement(WebExtractorHlp.ParentXmlNode(xmlNode), browser);

        if (parent != null)
        {
          parent = GoDeepThrowAlienTags(parent, xmlNode.Name);

          int neededElementIndex = WebExtractorHlp.XmlNodeIndexToParent(xmlNode);
          if (neededElementIndex >= 0)
          {
            foreach (HtmlElementWrapper child in parent.Children)
            {
              if (WebExtractorHlp.TagsAreSame(child.Name, xmlNode.Name))
              {
                neededElementIndex--;
                if (neededElementIndex == 0)
                  return child;
              }
            }
          }
          else
            return parent;
        }
      }
      return null;
    }
    #region Ovveridables
    public override bool Equals(object obj)
    {
      if (obj is HtmlElementWrapper)
        return object.Equals(this.element, ((HtmlElementWrapper)obj).element);
      return false;
    }
    public override int GetHashCode()
    {
      if (this.element != null)
        return this.element.GetHashCode();
      else return 0;
    }
    public override string ToString()
    {
      return this.Path ?? string.Empty;
    }
    #endregion

    public HtmlElementWrapper SelectSingleNode(string path)
    {
      string[] parts = path.Split('/');
      HtmlElementWrapper current = this;
      foreach (string part in parts)
      {
        Match m = Regex.Match(part, @"(?<name>\w+)\[(?<index>\d+)\]", RegexOptions.Compiled);
        HtmlElementWrapper nextCurrent = null;
        if (m.Success)
        {
          int index = -1;
          if (!int.TryParse(m.Groups["index"].Value, out index))
            return null;
          foreach (HtmlElementWrapper child in current.Children)
          {
            if (WebExtractorHlp.TagsAreSame(child.Name, m.Groups["name"].Value))
            {
              index--;
              if (index == 0)
              {
                nextCurrent = child;
                break;
              }
            }
          }
        }
        if (nextCurrent != null)
          current = nextCurrent;
        else
          return null;
      }
      return current;
    }

    public HtmlElementWrapper CreateChild(string tagName)
    {
      HtmlElement element = Element;
      HtmlDocument doc = element.Document;
      if (doc != null)
      {
        HtmlElement childElem = doc.CreateElement(tagName);
        element.AppendChild(childElem);
        return new HtmlElementWrapper(childElem);
      }
      return null;
    }

    public Rectangle AbsoluteRectangle
    {
      get
      {
        HtmlElement element = Element;
        Rectangle res = element.OffsetRectangle;
        HtmlElement curElem = null;
        FlowHlp.SafeBlock("Переход к OffsetParent", delegate { curElem = element.OffsetParent; });
        while (curElem != null && curElem != curElem.Document.Body)
        {
          res.X += curElem.OffsetRectangle.X;
          res.Y += curElem.OffsetRectangle.Y;
          FlowHlp.SafeBlock("Переход к OffsetParent", delegate { curElem = curElem.OffsetParent; });
        }
        return res;
      }
    }

    public void DeleteSelf()
    {
      Element.OuterHtml = "";
    }


    public static HtmlElementWrapper SelectHtmlNode(HtmlDocument htmlDocument, string resultPath)
    {
      if (htmlDocument != null && resultPath != null && htmlDocument.Body != null && htmlDocument.Body.Parent != null)
      {
        string[] parts = resultPath.Split('/');
        int currentIndex = 1;
        if (currentIndex < parts.Length && parts[currentIndex] == "HTML[1]")
        {
          currentIndex++;
          HtmlElement html = htmlDocument.Body.Parent;
          while (html.Parent != null)
            html = html.Parent;
          HtmlElementWrapper current = new HtmlElementWrapper(html);

          while (currentIndex < parts.Length)
          {
            bool found = false;
            foreach (HtmlElementWrapper child in current.Children)
            {
              if (string.Format("{0}[{1}]", child.Name, child.IndexToParent) == parts[currentIndex])
              {
                current = child;
                found = true;
                break;
              }
            }
            if (!found)
              return null;
            currentIndex++;
          }
          return current;
        }
        else
          return null;

      }
      else
        return null;
    }


  }
}
