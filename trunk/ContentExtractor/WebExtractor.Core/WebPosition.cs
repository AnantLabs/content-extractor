using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using MetaTech.Library;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Text.RegularExpressions;

namespace ContentExtractor.Core
{
  public class WebPosition
  {
    public struct PersistStruct : ICloneable
    {
      public PersistStruct(string Url)
      {
        this.Url = Url;
      }
      public string Url;

      public object Clone()
      {
        return new PersistStruct(Url);
      }

      public override string ToString()
      {
        return Url;
      }
    }

    public WebPosition() { }

    public WebPosition(Uri url)
    {
      this.Url = url;
    }

    public static WebPosition Parse(string address)
    {
      Uri uri = null;
      if (!Uri.TryCreate(address, UriKind.Absolute, out uri))
        Uri.TryCreate(Uri.UriSchemeHttp + "://" + address, UriKind.Absolute, out uri);

      if (uri != null)
        return new WebPosition(uri);
      else
        return null;
    }

    public static readonly Uri EmptyUri = new Uri(@"about:blank");
    public static readonly WebPosition.PersistStruct EmptyPersist = new PersistStruct(EmptyUri.AbsoluteUri);
    public static WebPosition EmptyPosition
    {
      get
      {
        WebPosition result = new WebPosition();
        result.Persist = Model.Clone(EmptyPersist);
        return result;
      }
    }

    [XmlIgnore]
    public Uri Url = EmptyUri;
    [XmlIgnore]
    public string htmlCode = string.Empty;

    public PersistStruct Persist
    {
      get
      {
        return new PersistStruct(this.Url.AbsoluteUri);
      }
      set
      {
        this.Url = new Uri(value.Url);
      }
    }

    private string cachedText = string.Empty;
    
    public IXPathNavigable XPathNavigable
    {
      get
      {
        bool rebuildCache = false;
        string text = DocumentText;
        lock (xPathNavigableCache)
          rebuildCache = !xPathNavigableCache.ContainsKey(this.Persist) || xPathNavigableCache[this.Persist].Key != text;
        if (rebuildCache)
        {
          lock (xPathNavigableCache)
          {
            //Hack - delete xmlns attribute
            xPathNavigableCache[this.Persist] =
              new KeyValuePair<string, IXPathNavigable>(text, WebExtractorHlp.LoadHtmlCode(XmlnsRemover.Replace(text, "")));
          }
        }
        lock (xPathNavigableCache)
          return xPathNavigableCache[this.Persist].Value;
      }
    }
    
    public XmlDocument XmlDocument
    {
      get
      {
        if (this.XPathNavigable is XmlDocument)
          return (XmlDocument)XPathNavigable;
        return XmlHlp.HtmlDocFromNavigable(this.XPathNavigable);
      }
    }

    public override string ToString()
    {
      return string.Format("> {0}", Url.AbsoluteUri);
    }

    public void Set(WebPosition source)
    {
      this.Persist = source.Persist;
    }

    public static PersistStruct GetPersist(WebPosition pos)
    {
      return pos.Persist;
    }

    public WebPosition GetLinkedPosition(string linkXPath)
    {
      XPathNavigator node = XmlHlp.SelectSingleNode(XPathNavigable, linkXPath);
      if (node != null)
      {
        string[] pathes = new string[] { "@href", "*/@href", "../@href" };
        foreach (string path in pathes)
        {
          XPathNavigator attr = node.SelectSingleNode(path);
          if (attr != null && attr.NodeType == XPathNodeType.Attribute)
          {
            Uri nextLink;
            if (Uri.TryCreate(Url, attr.Value, out nextLink))
            {
              return new WebPosition(nextLink);
            }
          }
        }
      }
      return null;
    }

  }
}
