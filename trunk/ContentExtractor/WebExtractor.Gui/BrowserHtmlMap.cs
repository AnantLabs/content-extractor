using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MetaTech.Library;
using ContentExtractor.Core;

namespace ContentExtractor.Gui
{
  public class BrowserHtmlMap
  {
    public BrowserHtmlMap()
    {

    }

    public HtmlElementWrapper GetHtmlElement(WebBrowser browser, XmlNode node)
    {
      if (browser != null && browser.ReadyState == WebBrowserReadyState.Complete && browser.Document != null && node != null)
      {
        if (CacheNotValid(XmlHlp2.GetDocument(node), browser.Document))
          ClearCache(XmlHlp2.GetDocument(node), browser.Document);

        string requestPath = XmlHlp.GetPath(node);
        return GetHtmlElement_Inner(requestPath);
      }
      else
        return null;
    }

    private XmlDocument cachedXml = new XmlDocument();
    private HtmlDocument cachedHtmlDocument = null;
    private Dictionary<HtmlElementWrapper, string> xmlNodesCache = new Dictionary<HtmlElementWrapper, string>();
    private Dictionary<string, HtmlElementWrapper> htmlNodesCache = new Dictionary<string, HtmlElementWrapper>();

    public string GetXmlNode(XmlDocument document, HtmlElementWrapper elem)
    {
      if (elem.Element != null && elem.Element.Document != null)
      {
        if (CacheNotValid(document, elem.Element.Document))
          ClearCache(document, elem.Element.Document);
        return GetXmlNode_Inner(elem);
      }
      else
        return string.Empty;
    }

    private string GetXmlNode_Inner(HtmlElementWrapper element)
    {
      if (element != null)
      {
        if (!xmlNodesCache.ContainsKey(element))
        {
          xmlNodesCache[element] = XmlHlp2.GetPath(XmlHlp.SelectSingleNode(cachedXml, element.Path));

          if (false)
          {
            if (element.Parent != null)
            {
              string parentPath = GetXmlNode_Inner(element.Parent);
              XmlNode xmlParent = XmlHlp.SelectSingleNode(cachedXml, parentPath);
              if (xmlParent != null)
              {
                string subPath = element.Path.Substring(element.Path.LastIndexOf('/') + 1);
                XmlNode child = xmlParent.SelectSingleNode(subPath);
                if (child != null)
                {
                  xmlNodesCache[element] = XmlHlp.GetPath(child);
                  htmlNodesCache[XmlHlp.GetPath(child)] = element;
                }
                else if (element.Parent.Children.Count == 1)
                  xmlNodesCache[element] = parentPath;
                else
                  xmlNodesCache[element] = null;
              }
              else
                xmlNodesCache[element] = null;
            }
            else if (WebExtractorHlp.TagsAreSame(element.Name, "html") &&
              XmlHlp.SelectSingleNode(cachedXml, "/html[1]") != null)
            {
              xmlNodesCache[element] = "/html[1]";
              htmlNodesCache["/html[1]"] = element;
            }
            else
              xmlNodesCache[element] = null;
          }
        }
        return xmlNodesCache[element];
      }
      return string.Empty;
    }

    private HtmlElementWrapper GetHtmlElement_Inner(string xmlPath)
    {
      if (!htmlNodesCache.ContainsKey(xmlPath))
      {
        htmlNodesCache[xmlPath] = HtmlElementWrapper.SelectHtmlNode(cachedHtmlDocument, xmlPath);
        if (false)
        {
          XmlNode xmlNode = XmlHlp.SelectSingleNode(cachedXml, xmlPath);
          if (xmlNode != null)
          {
            XmlNode parentNode = GetParentNode(xmlNode);
            if (parentNode != null && parentNode.NodeType != XmlNodeType.Document)
            {
              HtmlElementWrapper parentHtml = GetHtmlElement_Inner(XmlHlp.GetPath(parentNode));
              if (parentHtml != null)
              {
                if (xmlNode.NodeType == XmlNodeType.Element)
                {
                  string subPath = xmlPath.Substring(xmlPath.LastIndexOf('/') + 1);
                  HtmlElementWrapper child = parentHtml.SelectSingleNode(subPath);
                  if (child != null)
                  {
                    htmlNodesCache[xmlPath] = child;
                    xmlNodesCache[child] = xmlPath;
                  }
                  else if (parentHtml.Children.Count == 1)
                  {
                    xmlNodesCache[parentHtml.Children[0]] = XmlHlp.GetPath(parentNode);
                    child = parentHtml.Children[0].SelectSingleNode(subPath);
                    if (child != null)
                    {
                      htmlNodesCache[xmlPath] = child;
                      xmlNodesCache[child] = xmlPath;
                    }
                    else
                      htmlNodesCache[xmlPath] = null;
                  }
                  else
                    htmlNodesCache[xmlPath] = null;
                }
                else
                  htmlNodesCache[xmlPath] = parentHtml;
              }
              else
                htmlNodesCache[xmlPath] = null;
            }
            else if (WebExtractorHlp.TagsAreSame(xmlNode.LocalName, "html"))
            {
              htmlNodesCache[xmlPath] = HtmlElementWrapper.SelectHtmlNode(cachedHtmlDocument, "/html[1]");
              xmlNodesCache[htmlNodesCache[xmlPath]] = xmlPath;
            }
            else
              htmlNodesCache[xmlPath] = null;
          }
          else
            htmlNodesCache[xmlPath] = null;
        }
      }
      return htmlNodesCache[xmlPath];
    }

    private XmlNode GetParentNode(XmlNode node)
    {
      if (node.NodeType == XmlNodeType.Attribute)
        return ((XmlAttribute)node).OwnerElement;
      else
        return node.ParentNode;
    }

    private void ClearCache(XmlDocument document, HtmlDocument htmlDoc)
    {
      cachedXml = WebExtractorHlp.CopyXmlDocument(document);
      cachedHtmlDocument = htmlDoc;
      xmlNodesCache = new Dictionary<HtmlElementWrapper, string>();
      htmlNodesCache = new Dictionary<string, HtmlElementWrapper>();
    }

    //private void RebuildCache(XmlDocument document, HtmlDocument htmlDoc)
    //{
    //  cachedXml = WebExtractorHlp.CopyXmlDocument(document);
    //  cachedHtmlDocument = htmlDoc;
    //  xmlNodesCache = new Dictionary<string, string>();
    //  htmlNodesCache = new Dictionary<string, string>();

    //  HtmlElement htmlBody = htmlDoc.Body;
    //  if (htmlBody != null && htmlBody.Parent != null)
    //  {
    //    string htmlTagXPath = XmlHlp.GetPath(document.DocumentElement);
    //    xmlNodesCache[new HtmlElementWrapper(htmlBody.Parent).Path] = htmlTagXPath;
    //    htmlNodesCache[htmlTagXPath] = new HtmlElementWrapper(htmlBody.Parent).Path;
    //  }

    //  if (document.DocumentElement != null)
    //  {
    //    //  //ToDo здесь падает null reference exception
    //    XmlNode bodyElem = CollectionHlp.Find<XmlNode>(
    //      CollectionHlp.From<XmlNode>(document.DocumentElement.SelectNodes("*")),
    //      delegate(XmlNode node) { return node.LocalName == "body"; });
    //    if (bodyElem != null)
    //      CacheXmlSubTree(htmlBody, bodyElem);
    //  }
    //}

    //private void CacheXmlSubTree(HtmlElement element, XmlNode xmlNode)
    //{
    //  if (xmlNode != null && element != null)
    //  {
    //    HtmlElementWrapper wrapper = new HtmlElementWrapper(element);
    //    string xpath = XmlHlp.GetPath(xmlNode);
    //    if (!xmlNodesCache.ContainsKey(wrapper.Path))
    //      xmlNodesCache[wrapper.Path] = xpath;
    //    if (!htmlNodesCache.ContainsKey(xpath))
    //      htmlNodesCache[xpath] = wrapper.Path;

    //    List<XmlNode> xmlChildren = XmlHlp2.GetChildren(xmlNode);
    //    xmlChildren.Remove(xmlNode);

    //    List<HtmlElementWrapper> elementChildren = wrapper.Children;

    //    foreach (HtmlElementWrapper child in elementChildren)
    //    {
    //      if (child.Name == "!" || child.Name.StartsWith("/"))
    //        continue;
    //      XmlNode friend = xmlNode;
    //      for (int i = 0; i < xmlChildren.Count; i++)
    //        if (WebExtractorHlp.TagsAreSame(child.Name, xmlChildren[i].Name))
    //        {
    //          friend = xmlChildren[i];
    //          xmlChildren.RemoveAt(i);
    //          break;
    //        }
    //      CacheXmlSubTree(child.Element, friend);
    //    }
    //    foreach (XmlNode aloneXmlChild in xmlChildren)
    //      if (!htmlNodesCache.ContainsKey(XmlHlp.GetPath(aloneXmlChild)))
    //        htmlNodesCache[XmlHlp.GetPath(aloneXmlChild)] = wrapper.Path;
    //  }
    //}

    private bool CacheNotValid(XmlDocument document, HtmlDocument htmlDoc)
    {
      return !WebExtractorHlp.IsCachedXmlActual(cachedXml, document) ||
        !HtmlDocEquals(htmlDoc, cachedHtmlDocument);
      //(htmlDoc == null || cachedHtmlDocument == null) ||
      //htmlDoc.Url != cachedHtmlDocument.Url;
      //!object.Equals(htmlDoc, cachedHtmlDocument);
    }

    private static bool HtmlDocEquals(HtmlDocument doc1, HtmlDocument doc2)
    {
      //return HtmlDocument.Equals(doc1, doc2);
      if (doc1 != null)
      {
        if (doc2 != null)
        {
          //Hack
          return doc1.Body == doc2.Body;
          //return doc1.All.Count == doc2.All.Count;

          //return doc1.Body.OuterHtml == doc2.Body.OuterHtml;
        }
        else
          return false;
      }
      else
        return doc2 == null;
    }

  }
}
