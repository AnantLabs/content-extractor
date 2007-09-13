//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 29.06.2007
// Time: 14:36
//

using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ContentExtractor.Core
{
  /// <summary>
  /// Different useful functions
  /// </summary>
  public static class Utils
  {
    public static List<T> CastList<T>(IEnumerable collection)
    {
      List<T> result = new List<T>();
      foreach (object item in collection)
        result.Add((T)item);
      return result;
    }

    public static Uri ParseUrl(string address)
    {
      Uri uri = null;
      if (!Uri.TryCreate(address, UriKind.Absolute, out uri))
        Uri.TryCreate(Uri.UriSchemeHttp + "://" + address,
                      UriKind.Absolute, out uri);

      return uri;
    }

    public static XmlDocument HtmlParse(string content)
    {
      using (ExtendedWebBrowser browser = new ExtendedWebBrowser())
      {
        browser.DocumentText = content;
        browser.IsWebBrowserContextMenuEnabled = false;
        // WARNING!! this can cause problems when certificate approval needed!
        browser.ScriptErrorsSuppressed = true;
        System.Windows.Forms.Application.DoEvents();
        while (browser.IsBusy)
          System.Windows.Forms.Application.DoEvents();
        return DOMTreeToXml(browser.Document);
      }
    }

    private static XmlDocument DOMTreeToXml(HtmlDocument htmlDoc)
    {
      XmlDocument result = new XmlDocument();
      if (htmlDoc != null &&
         htmlDoc.Body != null &&
         htmlDoc.Body.Parent != null)
      {
        HtmlElement html = htmlDoc.GetElementsByTagName("HTML")[0];
        XmlElement htmlXml = result.CreateElement("HTML");
        result.AppendChild(htmlXml);
        ImportHtml2Xml(html.DomElement, htmlXml);

        //HtmlElement topHtml = htmlDoc.Body.Parent;
        //using (StringReader sReader = new StringReader(topHtml.OuterHtml))
        //{
        //  using (StringWriter errorLog = new StringWriter())
        //  {
        //    Sgml.SgmlReader reader = new Sgml.SgmlReader();
        //    reader.ErrorLog = errorLog;
        //    reader.InputStream = sReader;
        //    reader.DocType = "HTML";

        //    using (StringReader dtdReader = new StringReader(Encoding.UTF8.GetString(Resources.weak)))
        //      //Resources.WeakDtd
        //      reader.Dtd = Sgml.SgmlDtd.Parse(null, "HTML", null, dtdReader, null, null, reader.NameTable);

        //    result.Load(reader);
        //    // TODO: log to INFO
        //    //errorLog.Flush();
        //    //Console.WriteLine(errorLog.ToString());
        //  }
        //}
      }
      return result;
    }

    private static void ImportHtml2Xml(object html, XmlElement xml)
    {
      mshtml.IHTMLDOMNode node = html as mshtml.IHTMLDOMNode;
      if (node != null)
      {
        mshtml.IHTMLAttributeCollection attCol = node.attributes as mshtml.IHTMLAttributeCollection;
        if (attCol != null)
        {
          foreach (mshtml.IHTMLDOMAttribute attr in attCol)
            if (attr.specified && attr.nodeValue != null)
            {
              CheckNotNull(attr.nodeName);
              xml.SetAttribute(attr.nodeName, attr.nodeValue.ToString());
            }
        }
        IEnumerable childsEnum = node.childNodes as IEnumerable;
        foreach (object objChild in childsEnum)
        {
          mshtml.IHTMLElement element = objChild as mshtml.IHTMLElement;
          if (element != null)
          {
            if (!element.tagName.StartsWith("!"))
            {
              if (element.tagName.StartsWith("/"))
              {
                Console.WriteLine(element.tagName);
              }
              else
              {
                XmlElement xmlChild = xml.OwnerDocument.CreateElement(element.tagName);
                xml.AppendChild(xmlChild);
                ImportHtml2Xml(element, xmlChild);
              }
            }
          }
          else
          {
            mshtml.IHTMLDOMTextNode text = objChild as mshtml.IHTMLDOMTextNode;
            if (text != null)
            {
              XmlText xmlText = xml.OwnerDocument.CreateTextNode(text.data);
              xml.AppendChild(xmlText);
            }
            else
              Console.WriteLine(objChild.ToString());
          }
        }
      }
    }

    public static void CheckNotNull(object value)
    {
      if (value == null)
        throw new ArgumentException("Value mustn't be null");
    }

    public static string ApplicationMapPath(string localFileName)
    {
      return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), localFileName);
    }

    public static string HtmlElementXPath(HtmlElement element)
    {
      if (element != null)
      {
        if (element.Parent != null)
        {
          return string.Format("{0}/{1}[{2}]",
            HtmlElementXPath(element.Parent),
            element.TagName,
            HtmlElementIndexToParent(element));
        }
        else
          return "/" + element.TagName + "[1]";
      }
      return null;
    }

    private static int HtmlElementIndexToParent(HtmlElement element)
    {
      int index = 0;
      if (element.Parent != null)
      {
        foreach (HtmlElement brother in element.Parent.Children)
          if (brother.TagName == element.TagName)
          {
            index++;
            if (brother.Equals(element))
              return index;
          }
      }
      return index;
    }

    // WARNING: Can fail with "bad" tags like <noindex>
    public static HtmlElement SelectHtmlElement(HtmlDocument doc, string xpath)
    {
      if (xpath.StartsWith("/HTML[1]/"))
      {
        string[] parts = xpath.Split('/');
        HtmlElementCollection htmlTagCollection = doc.GetElementsByTagName("HTML");
        if (htmlTagCollection.Count == 1)
        {
          HtmlElement current = htmlTagCollection[0];
          // i = 0 - "", i = 1 - "HTML[1]"
          for (int i = 2; i < parts.Length && current != null; i++)
            current = GetHtmlChild(current, parts[i]);
          //:TODO if current == null Log.Warning
          return current;
        }
      }
      return null;
    }

    private static HtmlElement GetHtmlChild(HtmlElement parent, string xPathPart)
    {
      Match m = Regex.Match(xPathPart, @"(?<name>\w+)\[(?<index>\d+)\]", RegexOptions.Compiled);
      if (m.Success)
      {
        int index;
        if (int.TryParse(m.Groups["index"].Value, out index))
          return GetHtmlChild(parent, m.Groups["name"].Value, index);
      }
      return null;
    }
    private static HtmlElement GetHtmlChild(HtmlElement parent, string tagName, int index)
    {
      foreach (HtmlElement child in parent.Children)
        if (child.TagName == tagName)
        {
          index--;
          if (index == 0)
            return child;
        }
      return null;
   }

    public static bool IsIndexOk(int index, ICollection collection)
    {
      return (0 <= index) && (index < collection.Count);
    }

  }

  public delegate void Callback<T>(T value);

}
