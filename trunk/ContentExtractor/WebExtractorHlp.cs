using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MetaTech.Library;
using System.Drawing;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using Sgml;
using System.Xml.XPath;

namespace ContentExtractor.Core
{
  public static class WebExtractorHlp
  {
    public static int XmlNodeIndexToParent(XmlNode node)
    {
      if (node.NodeType == XmlNodeType.Element && node.ParentNode != null)
      {
        int index = 0;
        foreach (XmlNode child in node.ParentNode.SelectNodes("*"))
        {
          if (child.Name == node.Name)
          {
            index++;
            if (child == node)
              return index;
          }
        }
      }
      return -1;
    }

    public static bool NextIndexes(ref int[] indexes, int topValue)
    {
      int topBorder = topValue;
      for (int i = indexes.Length - 1; i >= 0; i--)
      {
        if (indexes[i] + 1 < topBorder)
        {
          indexes[i]++;
          for (int j = i + 1; j < indexes.Length; j++)
            indexes[j] = indexes[j - 1] + 1;
          return true;
        }
        topBorder = indexes[i];
      }
      return false;
    }
    public static string FilteredXPath(string[] strings, int[] indexes)
    {
      StringBuilder result = new StringBuilder();
      for (int i = 0; i < strings.Length; i++)
      {
        bool needInsert = true;
        foreach (int index in indexes)
          if (index == i)
          {
            needInsert = false;
            break;
          }
        if (needInsert)
          result.Append("/" + strings[i]);
      }
      return result.ToString();
    }

    //public static XmlNode HtmlElement2XmlNode(HtmlElementWrapper testElement, XmlDocument xmlDocument)
    //{
    //  string[] xPath = testElement.Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

    //  DateTime startTime = DateTime.Now;
    //  TimeSpan guardSpan = TimeSpan.FromSeconds(60);
    //  for (int rank = 0; rank < xPath.Length; rank++)
    //  {
    //    int[] indexes = CreateIndexes(rank);
    //    do
    //    {
    //      if (xmlDocument.SelectNodes(FilteredXPath(xPath, indexes)).Count == 1)
    //        return xmlDocument.SelectSingleNode(FilteredXPath(xPath, indexes));
    //      if (DateTime.Now - startTime > guardSpan)
    //        return null;
    //    }
    //    while (NextIndexes(ref indexes, xPath.Length));
    //  }
    //  return null;
    //}

    private static int[] CreateIndexes(int rank)
    {
      int[] indexes = new int[rank];
      for (int j = 0; j < rank; j++)
        indexes[j] = j;
      return indexes;
    }

    public static bool TagsAreSame(string tag1, string tag2)
    {
      return string.Equals(tag1, tag2, StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool IsCachedXmlActual(XmlDocument cached, XmlDocument actual)
    {
      bool cacheIsNull = cached != null;
      bool actualIsNull = actual != null;

      if (cacheIsNull && actualIsNull)
      {
        return cached.SelectNodes("//*").Count == actual.SelectNodes("//*").Count;
      }
      else
        return cacheIsNull == actualIsNull;
    }

    public static XmlNode ParentXmlNode(XmlNode xmlNode)
    {
      if (xmlNode is XmlAttribute)
        return ((XmlAttribute)xmlNode).OwnerElement;
      else
        return xmlNode.ParentNode;
    }

    public static XmlDocument CopyXmlDocument(XmlDocument doc)
    {
      if (doc != null)
      {
        XmlDocument result = new XmlDocument();
        if (doc.DocumentElement != null)
          result.AppendChild(result.ImportNode(doc.DocumentElement, true));
        return result;
      }
      else
        return null;
    }

    //public static IXPathNavigable LoadHtmlCode1(string documentCode)
    //{
    //  HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
    //  doc.OptionFixNestedTags = false;
    //  doc.OptionAutoCloseOnEnd = false;
    //  doc.OptionCheckSyntax = false;
    //  doc.OptionAutoCloseOnEnd = false;
    //  doc.OptionOutputAsXml = true;
    //  try
    //  {
    //    doc.LoadHtml(documentCode);
    //  }
    //  catch (Exception exc)
    //  {
    //    TraceHlp2.WriteException(exc);
    //  }
    //  return doc;
    //  //XmlDocument result = new XmlDocument();
    //  //result.LoadXml(doc.CreateNavigator().OuterXml);
    //  //return result;
    //}

    private static SgmlDtd _dtd = null;
    private static SgmlDtd DTD
    {
      get
      {
        if (_dtd == null)
        {
          using (StringReader dtdReader = new StringReader(Properties.Resources.WeakHtml))
            //using (StringReader dtdReader = new StringReader(SoftTech.Html.HtmlDtd.Loose))
            _dtd = SgmlDtd.Parse(null, "HTML", null, dtdReader, null, null, new SgmlReader().NameTable);
        }
        return _dtd;
      }
    }

    public static IXPathNavigable LoadHtmlCode(string documentCode)
    {
      //XmlDocument resultXml = new XmlDocument();
      if (documentCode != null)
      {
        try
        {
          //Создаем документ обрамленный тегами, на случай, если на верхнем уровне больше чем один тэг
          XmlDocument tempDocument = new XmlDocument();
          using (StringReader sReader = new StringReader(PrepareHtmlCode(documentCode)))
          //using (StringReader sReader = new StringReader(documentCode))
          {
            using (StringWriter errorLog = new StringWriter())
            {
              SgmlReader reader = new SgmlReader();
              reader.ErrorLog = errorLog;
              reader.InputStream = sReader;
              //reader.Dtd = DTD;
              using (StringReader dtdReader = new StringReader(Properties.Resources.WeakHtml))
                //using (StringReader dtdReader = new StringReader(SoftTech.Html.HtmlDtd.Loose))
                reader.Dtd = SgmlDtd.Parse(null, "HTML", null, dtdReader, null, null, reader.NameTable);

              //reader.DocType = "HTML";
              tempDocument.Load(reader);
              //errorLog.Flush();
              //Console.WriteLine(errorLog.ToString());
            }
          }
          if (tempDocument != null && tempDocument.DocumentElement != null)
          {
            //XmlNode doc = tempDocument.DocumentElement;
            //XmlNode html = _.From<XmlNode>(doc.ChildNodes).Find(
            //  delegate(XmlNode node) { return node.LocalName == "html"; }) ?? doc;
            foreach (XmlNode html in tempDocument.DocumentElement.ChildNodes)
            {
              if (html.NodeType == XmlNodeType.Element && html.Name.ToLower() == "html")
              {
                XmlDocument result = new XmlDocument();
                result.AppendChild(result.ImportNode(html, false));
                SafeDeepCopy(result.DocumentElement, html);
                return result;
              }
            }
          }

        }
        catch (Exception exc)
        {
          TraceHlp2.WriteException(exc);
        }
      }
      return new XmlDocument();
    }

    private static void SafeDeepCopy(XmlNode to, XmlNode from)
    {
      for (int i = 0; i < from.ChildNodes.Count; i++)
      {
        try
        {
          XmlNode child = from.ChildNodes[i];
          XmlNode toChild = to.AppendChild(to.OwnerDocument.ImportNode(child, false));
          SafeDeepCopy(toChild, child);
        }
        catch (Exception exc)
        {
          TraceHlp2.WriteException(exc);
          //Console.WriteLine(exc);
        }
      }
    }

    private static string PrepareHtmlCode(string code)
    {
      string result = code;
      result = Regex.Replace(code, @"<\?xml\s+version=.+?\?>", "", RegexOptions.Compiled);
      return string.Format("<Root>{0}</Root>", result);
    }

    public static string DocumentText(WebBrowser browser)
    {
      string documentCode = null;
      if (browser != null && browser.Document != null)
      {
        using (StreamReader tReader = new StreamReader(browser.DocumentStream, Encoding.GetEncoding(browser.Document.Encoding)))
        {
          documentCode = tReader.ReadToEnd();
        }
      }
      return documentCode;
    }

    public static bool CompareList<T>(object ob1, object ob2)
    {
      List<T> list1 = (List<T>)ob1;
      List<T> list2 = (List<T>)ob1;
      return CompareList(list1, list2);
    }

    public static bool CompareList<T>(IEnumerable<T> col1, IEnumerable<T> col2)
    {
      List<T> list1 = new List<T>(col1);
      List<T> list2 = new List<T>(col2);

      if (list1.Count == list2.Count)
      {
        for (int i = 0; i < list1.Count; i++)
        {
          if (!object.Equals(list1[i], list2[i]))
            return false;
        }
        return true;
      }
      return false;
    }


    public static List<T> CopyList<T>(List<T> list)
      where T : ICloneable
    {
      List<T> result = new List<T>();
      foreach (T val in list)
        result.Add((T)val.Clone());
      return result;
    }

    public static string NextExcelName(string columnName)
    {
      StringBuilder result = new StringBuilder(columnName);
      for (int i = result.Length - 1; i >= 0; i--)
      {
        if (result[i] < 'Z')
        {
          result[i] = Convert.ToChar(Convert.ToInt32(columnName[i]) + 1);
          return result.ToString();
        }
        else
          result[i] = 'A';
      }
      result.Insert(0, 'A');
      return result.ToString();
    }

    public static void DoDragHtmlNode(Control control, object htmlNode)
    {
      DataObject dataObj = new DataObject("Html node", htmlNode);
      control.DoDragDrop(dataObj, DragDropEffects.Link);
    }

    public static string ExtractDragData(IDataObject data)
    {
      if (data.GetDataPresent("Html node"))
        return (string)data.GetData("Html node", true);
      return null;
    }

  }

  public static class NodeType
  {
    public const string Attribute = "attribute";
    public const string Tag = "tag";
    public const string Text = "text";
  }

}
