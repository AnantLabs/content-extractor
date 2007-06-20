using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using MetaTech.Library;

namespace ContentExtractor.Core
{
  public delegate object FunctionDelegate(object argument);

  public static class Functions
  {
    public const string FToString = "";
    public const string FBody = "Body";
    public const string FContent = "Content";
    public const string FInnerText = "Text only";

    static Functions()
    {
      lock (functions)
      {
        functions[FToString] = Cast<XPathValue>(ToStringNavigator);
        functions[FBody] = Cast<XPathValue>(Body);
        functions[FContent] = Cast<XPathValue>(Content);
        functions[FInnerText] = Cast<XPathValue>(InnerText);
      }
    }
    private static Dictionary<string, FunctionDelegate> functions = new Dictionary<string, FunctionDelegate>();

    public static object Evaluate(string funcName, object argument)
    {
      if (FunctionExist(funcName))
      {
        FunctionDelegate func;
        lock (functions)
          func = functions[funcName];
        return func(argument);
      }
      else
        throw new ArgumentException(string.Format("Unknown function {0}", funcName));
    }

    public static string[] AllFunctions
    {
      get
      {
        return new List<string>(functions.Keys).ToArray();
      }
    }

    public static bool FunctionExist(string funcName)
    {
      lock (functions)
        return functions.ContainsKey(funcName);
    }

    private static FunctionDelegate Cast<T>(Converter<T, object> func)
    {
      return delegate(object obj) { return func((T)obj); };
    }

    private static object ToStringNavigator(XPathValue value)
    {
      //return value.Navigator;
      if (value != null && value.Navigator != null)
        return value.Navigator;
      //return InnerXml(value.Navigator);
      return string.Empty;
    }

    private static Dictionary<WebPosition, KeyValuePair<string, object>> contentCache =
      new Dictionary<WebPosition, KeyValuePair<string, object>>();

    private static object Content(XPathValue value)
    {
      Uri link = Link(value);
      if (link != null)
      {
        WebPosition position = new WebPosition(link);
        string text = position.DocumentText;
        bool rebuild = false;
        lock (contentCache)
          rebuild = !contentCache.ContainsKey(position) || contentCache[position].Key != text;
        if (rebuild)
        {
          XmlDocument document = FilterDocument(position.XPathNavigable);
          if (document != null && document.DocumentElement != null)
          {
            XmlNode best = BestContentNode(document);
            if (best != null)
            {
              lock (contentCache)
                contentCache[position] =
                  new KeyValuePair<string, object>(text, best.CreateNavigator().SelectChildren(XPathNodeType.All));

              //XmlDocument result = new XmlDocument();
              //result.AppendChild(result.CreateElement("Root"));
              //foreach (XmlNode child in best.ChildNodes)
              //{
              //  if (child.NodeType == XmlNodeType.Element || child.NodeType == XmlNodeType.Text || child.NodeType == XmlNodeType.CDATA)
              //  {
              //    if (!notContentTags.Contains(child.Name.ToLower()))
              //      result.DocumentElement.AppendChild(result.ImportNode(child, true));
              //  }
              //}
              //return result.DocumentElement.CreateNavigator().SelectChildren(XPathNodeType.All);
            }
          }
        }
        lock (contentCache)
          if (contentCache.ContainsKey(position))
            return contentCache[position].Value;
          else
            return null;
      }
      return null;
    }

    private static XmlDocument FilterDocument(IXPathNavigable document)
    {
      XmlDocument result = new XmlDocument();
      if (document != null)
      {
        foreach (XPathNavigator child in document.CreateNavigator().SelectChildren(XPathNodeType.All))
        {
          if (WebExtractorHlp.TagsAreSame(child.LocalName, "html"))
          {
            AppendFiltered(result, child);
          }
        }

        //result.AppendChild(result.ImportNode(document.DocumentElement, false));
        //ImportDeep(result.DocumentElement, document.DocumentElement);
      }
      return result;
    }

    private static void AppendFiltered(XmlNode result, XPathNavigator child)
    {
      XmlDocument document = result as XmlDocument;
      if (document == null)
        document = result.OwnerDocument;
      switch (child.NodeType)
      {
        case XPathNodeType.Attribute:
          if (result is XmlElement)
          {
            XmlAttribute attr = document.CreateAttribute(child.Name);
            attr.Value = child.Value;
            ((XmlElement)result).SetAttributeNode(attr);
          }
          break;
        case XPathNodeType.Comment:
          result.AppendChild(document.CreateComment(child.Value));
          break;
        case XPathNodeType.Element:
          if (GoodName(child.Name))
          {
            XmlElement subResult = document.CreateElement(child.Name);
            result.AppendChild(subResult);
            foreach (XPathNavigator subChild in child.Select("@*"))
              AppendFiltered(subResult, subChild);
            foreach (XPathNavigator subChild in child.SelectChildren(XPathNodeType.All))
              AppendFiltered(subResult, subChild);
          }
          break;
        case XPathNodeType.Text:
          XmlText text = document.CreateTextNode(child.Value);
          result.AppendChild(text);
          break;
        case XPathNodeType.Root:
        case XPathNodeType.Whitespace:
        case XPathNodeType.ProcessingInstruction:
        case XPathNodeType.SignificantWhitespace:
        case XPathNodeType.Namespace:
        case XPathNodeType.All:
        default:
          TraceHlp2.AddMessage("Не знаю, что делать с вершиной типа {0}", child.NodeType);
          break;
      }
    }

    private static void ImportDeep(XmlNode to, XmlNode from)
    {
      foreach (XmlNode child in from.ChildNodes)
      {
        if (child.NodeType == XmlNodeType.Element && GoodName(child.Name) ||
          child.NodeType == XmlNodeType.Text || child.NodeType == XmlNodeType.CDATA ||
          child.NodeType == XmlNodeType.Comment || child.NodeType == XmlNodeType.Whitespace ||
          child.NodeType == XmlNodeType.Attribute)
        {
          XmlNode toChild = to.AppendChild(to.OwnerDocument.ImportNode(child, false));
          ImportDeep(toChild, child);
        }
      }
    }

    private static bool GoodName(string name)
    {
      return !notContentTags.Contains(name.ToLower());
    }
    private static List<string> notContentTags = new List<string>(new string[] { "script", "style", "noindex", "form" });

    public static XmlNode BestContentNode(XmlNode node)
    {
      Dictionary<XmlNode, double> weight = new Dictionary<XmlNode, double>();
      FillWeight(weight, node);
      //PrintWeights(weight, node, 0);
      return BestNode(weight, node);
    }

    private static XmlNode BestNode(Dictionary<XmlNode, double> weight, XmlNode node)
    {
      List<XmlNode> nodes = new List<XmlNode>();
      foreach (XmlNode key in weight.Keys)
        if (key.NodeType == XmlNodeType.Element)
          nodes.Add(key);

      nodes.Sort(delegate(XmlNode left, XmlNode right) { return -Math.Sign(weight[left] - weight[right]); });
      if (nodes.Count > 0)
      {
        //Console.WriteLine(weight[nodes[0]]);
        return nodes[0];
      }
      else
        return null;
    }

    private static void PrintWeights(Dictionary<XmlNode, double> weight, XmlNode node, int p)
    {
      if (weight.ContainsKey(node))
      {
        Console.Write(new string(' ', p * 2));
        Console.WriteLine("{0} = {1}", node.Name, weight[node]);
        foreach (XmlNode child in node.ChildNodes)
          PrintWeights(weight, child, p + 1);
      }
    }

    private static void FillWeight(Dictionary<XmlNode, double> weight, XmlNode node)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Document:
        case XmlNodeType.Element:
          double value = 0;
          foreach (XmlNode child in node.ChildNodes)
          {
            FillWeight(weight, child);
            if (weight.ContainsKey(child))
              value += weight[child];
          }
          weight[node] = value * 0.7;
          break;
        case XmlNodeType.CDATA:
        case XmlNodeType.Text:
          weight[node] = node.Value.Trim().Length;
          break;
        default:
          break;
      }
    }

    private static object InnerText(XPathValue value)
    {
      //Uri link = Link(value);
      //if (link != null)
      //{
      //  WebPosition position = new WebPosition(link);
      //  return InnerText(position.XPathNavigable);
      //}
      //else
      return InnerText(value.Navigator);
    }

    private static object Body(XPathValue value)
    {
      Uri link = Link(value);
      if (link != null)
      {
        IXPathNavigable doc = new WebPosition(link).XPathNavigable;
        if (doc != null)
        {
          XPathNavigator html = XmlHlp.SelectElement(doc, "html");
          if (html != null)
          {
            XPathNavigator body = XmlHlp.SelectElement(html, "body");
            if (body != null)
              return body.SelectChildren(XPathNodeType.All ^ XPathNodeType.Attribute);
          }
        }
      }
      return null;
    }

    private static string InnerXml(IXPathNavigable node)
    {
      if (node != null)
      {
        XPathNavigator navigator = node.CreateNavigator();
        if (navigator.NodeType == XPathNodeType.Text || navigator.NodeType == XPathNodeType.Attribute)
          return navigator.TypedValue.ToString();
        else if (navigator.InnerXml != null)
          return navigator.InnerXml;
      }
      return null;
    }

    public static string InnerText(IXPathNavigable node)
    {
      if (node != null)
      {
        XPathNavigator navigator = node.CreateNavigator();
        StringBuilder result = new StringBuilder();
        AppendNode(result, navigator);
        return result.ToString();
      }
      return string.Empty;
    }

    private static void AppendNode(StringBuilder sb, XPathNavigator nav)
    {
      switch (nav.NodeType)
      {
        case XPathNodeType.Text:
          if (sb.Length > 0 && !char.IsSeparator(sb[sb.Length - 1]))
            sb.Append(' ');
          sb.Append(nav.Value.Trim());
          break;
        case XPathNodeType.Element:
        case XPathNodeType.Root:
          foreach (XPathNavigator child in nav.SelectChildren(XPathNodeType.All))
            AppendNode(sb, child);
          break;
      }
    }

    private static Uri Link(XPathValue value)
    {
      Uri page = null;
      if (value.Navigator != null && (value.Navigator.NodeType == XPathNodeType.Text ||
        value.Navigator.NodeType == XPathNodeType.Attribute))
      {
        page = new Uri(value.PageUri, value.Navigator.Value);
      }
      return page;
    }
  }
}
