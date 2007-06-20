using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using MetaTech.Library;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace ContentExtractor.Core
{

//  public class TemplateColumn : ICloneable
//  {
//    public TemplateColumn() { }

//    public TemplateColumn(WebTemplate template, string xpath)
//    {
//      this.template = template;
//      this.Points.Add(xpath);
//      //this.AbsXPath = xpath;
//    }
//    private WebTemplate template;

//    public string RelXPath
//    {
//      get
//      {
//        if (template.RowsXPath.Length >= Points[0].Length)
//          return ".";
//        string preResult = Points[0].Substring(template.RowsXPath.Length);
//        if (preResult.Contains("/"))
//          return preResult.Substring(preResult.IndexOf('/') + 1);
//        else return ".";
//      }
//    }
//    //public string AbsXPath;

//    public string NodeXPath
//    {
//      get
//      {
//        if (Points.Count == 1)
//          return Points[0];
//        else
//          return WebTemplate.CollapseRowsXPathes(Points.ToArray());
//      }
//    }

//    public List<string> Points = new List<string>();

//    public string GetValue(XmlNode row)
//    {
//      XmlNode cell = XmlHlp.SelectSingleNode(row, RelXPath);
//      if (cell != null && cell.InnerText != null)
//        return cell.InnerText.Trim();
//      else
//        return string.Empty;
//    }
//    public string GetValue(IXPathNavigable row)
//    {
//      XPathNavigator nav = XmlHlp.SelectSingleNode(row.CreateNavigator(), RelXPath);
//      if (nav != null)
//      {
//        if (nav.NodeType == XPathNodeType.Text || nav.NodeType == XPathNodeType.Attribute)
//          return nav.TypedValue.ToString();
//        else if (nav.InnerXml != null)
//          return nav.InnerXml;
//      }
//      return string.Empty;
//    }

//    public object Clone()
//    {
//      TemplateColumn result = new TemplateColumn();
//      result.template = this.template;
//      result.Points = WebExtractorHlp.CopyList(Points);
//      return result;
//    }

//    public override bool Equals(object obj)
//    {
//      if (obj is TemplateColumn)
//      {
//        TemplateColumn other = (TemplateColumn)obj;
//        return WebExtractorHlp.CompareList(this.Points, other.Points);
//      }
//      return false;
//    }
//    public override int GetHashCode()
//    {
//      return base.GetHashCode();
//    }
//  }

//  public class TemplateFixPoint
//    : ICloneable
//  {
//    public int Column;
//    public int Row;
//    public string XPath;

//    public TemplateFixPoint() { }
//    public TemplateFixPoint(int row, int col, string xpath)
//    {
//      this.Row = row;
//      this.Column = col;
//      this.XPath = xpath;
//    }

//    public object Clone()
//    {
//      return new TemplateFixPoint(Row, Column, XPath);
//    }

//    public override bool Equals(object obj)
//    {
//      if (obj is TemplateFixPoint)
//      {
//        TemplateFixPoint other = (TemplateFixPoint)obj;
//        return other.Row == this.Row && this.Column == other.Column && this.XPath == other.XPath;
//      }
//      return false;
//    }
//    public override int GetHashCode()
//    {
//      return Row.GetHashCode() + Column.GetHashCode() + XPath.GetHashCode();
//    }

//  }

//  [Obsolete("Использовать DataTemplate")]
//  public class WebTemplate : ICloneable
//  {
//    public List<TemplateFixPoint> FixPoints = new List<TemplateFixPoint>();

//    private string cachedRowsPath = "";
//    private List<TemplateColumn> cachedList = new List<TemplateColumn>();
//    [XmlIgnore]
//    public string RowsXPath
//    {
//      get
//      {
//        if (!WebExtractorHlp.CompareList(cachedList, columns))
//        {
//          cachedList = WebExtractorHlp.CopyList(columns);
//          cachedRowsPath = GetRowXPath(columns.ConvertAll<string>(delegate(TemplateColumn c) { return c.NodeXPath; }).ToArray());
//        }
//        return cachedRowsPath;
//      }
//    }

//    private List<TemplateColumn> columns = new List<TemplateColumn>();
//    [XmlIgnore]
//    public List<TemplateColumn> Columns
//    {
//      get
//      {
//        return columns;

//        List<TemplateColumn> result = new List<TemplateColumn>();
//        for (int i = 0; i < ColumnsCount; i++)
//        {
//          foreach (TemplateFixPoint fix in FixPoints)
//          {
//            if (fix.Column == i)
//            {
//              TemplateColumn newColumn = new TemplateColumn(this, fix.XPath);
//              //Если колонки с таким же xpath еще нет, то добавляем ее
//              if (result.TrueForAll(delegate(TemplateColumn c) { return c.RelXPath != newColumn.RelXPath; }))
//                result.Add(newColumn);
//              break;
//            }
//          }
//        }
//        return result;
//      }
//    }

//    public List<XPathNavigator> GetRows(IXPathNavigable source)
//    {
//      if (source != null)
//      {
//        XPathNavigator navigator = source.CreateNavigator();

//        List<XPathNavigator> result = CollectionHlp.From<XPathNavigator>(
//          XmlHlp.Select(navigator, "/Page" + RowsXPath));
//        return result;
//      }
//      else
//        return new List<XPathNavigator>();
//    }

//    public void AddNode(int row, int column, string xpath)
//    {
//      if (column >= columns.Count)
//        columns.Add(new TemplateColumn(this, xpath));
//      else
//      {
//        columns[column].Points.Add(xpath);
//      }
//      //foreach (TemplateFixPoint fix in FixPoints)
//      //{
//      //  if (fix.Row == row && fix.Column == column)
//      //  {
//      //    fix.XPath = xpath;
//      //    return;
//      //  }
//      //}
//      //FixPoints.Add(new TemplateFixPoint(Math.Min(row, RowsCount), Math.Min(column, ColumnsCount), xpath));
//    }

//    private int RowsCount
//    {
//      get
//      {
//        return MathHlp.Max(0, FixPoints.ConvertAll<int>(delegate(TemplateFixPoint fix) { return fix.Row + 1; })); ;
//      }
//    }
//    private int ColumnsCount
//    {
//      get
//      {
//        return MathHlp.Max(0, FixPoints.ConvertAll<int>(delegate(TemplateFixPoint fix) { return fix.Column + 1; })); ;
//      }
//    }

//    public string GetRowXPath(List<TemplateFixPoint> points)
//    {
//      List<string> rowPathes = new List<string>();
//      for (int i = 0; i < RowsCount; i++)
//      {
//        List<string> columns = new List<string>();
//        foreach (TemplateFixPoint fix in FixPoints)
//        {
//          if (fix.Row == i)
//            columns.Add(fix.XPath);
//        }
//        if (columns.Count > 0)
//          rowPathes.Add(GetRowXPath(columns.ToArray()));
//#if DEBUG
//        else
//          throw new Exception("В шаблоне не может быть пустых строк");
//#endif
//      }
//      if (rowPathes.Count > 0)
//      {
//        return CollapseRowsXPathes(rowPathes.ToArray());
//      }
//      else
//        return ".";
//    }

//    public static string CollapseRowsXPathes(params string[] xpathes)
//    {
//      if (xpathes.Length > 0)
//      {
//        string result = xpathes[0];
//        for (int i = 1; i < xpathes.Length; i++)
//          result = CommonPart(result, xpathes[i]);
//        if (result.EndsWith("["))
//          result = result.Remove(result.Length - 1);
//        else if (result.EndsWith("]/"))
//          result = result.Remove(result.LastIndexOf('['));
//        return result;
//      }
//      else
//        return string.Empty;
//    }

//    public static string CommonPart(string s1, string s2)
//    {
//      int length = Math.Min(s1.Length, s2.Length);
//      int index;
//      for (index = 0; index < length; index++)
//      {
//        if (s1[index] != s2[index])
//          break;
//      }
//      if (index < length)
//        return s1.Remove(index);
//      else
//        return s1;
//    }

//    public static string GetRowXPath(params string[] cols)
//    {
//      string[] columns = CollectionHlp.From<string>(CollectionHlp.Distinct(cols)).ToArray();

//      if (columns.Length == 1)
//        return columns[0];
//      if (columns.Length > 1)
//      {
//        string[] xPathParts = SplitPath(columns[0]);
//        //string[] xPathParts = columns[0].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
//        int partsCount = xPathParts.Length;
//        for (int colIndex = 1; colIndex < columns.Length; colIndex++)
//        {
//          string[] currentParts = SplitPath(columns[colIndex]);
//          //string[] currentParts = columns[colIndex].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
//          int partIndex = 0;
//          int minLength = Math.Min(xPathParts.Length, currentParts.Length);
//          while (partIndex < minLength && xPathParts[partIndex] == currentParts[partIndex])
//            partIndex++;


//          if (partIndex < minLength &&
//            (xPathParts[partIndex].StartsWith(currentParts[partIndex]) || currentParts[partIndex].StartsWith(xPathParts[partIndex])))
//          {
//            if (currentParts[partIndex].Length > xPathParts[partIndex].Length)
//              currentParts[partIndex] = xPathParts[partIndex];
//            partIndex++;
//          }

//          partsCount = Math.Min(partsCount, partIndex);
//        }

//        if (partsCount > 0)
//        {
//          int lastPartIndex = partsCount - 1;
//          Regex indexedRegex = new Regex(@"(?<name>({.*?}:)?\w[\w|\d]*)\[\d+\]");
//          Match match = indexedRegex.Match(xPathParts[lastPartIndex]);
//          if (match.Success)
//          {
//            xPathParts[lastPartIndex] = match.Groups["name"].Value;
//          }
//        }

//        StringBuilder result = new StringBuilder();
//        for (int i = 0; i < partsCount; i++)
//        {
//          result.Append('/' + xPathParts[i]);
//        }
//        return result.ToString();
//      }
//      return string.Empty;
//    }

//    private static string[] SplitPath(string col)
//    {
//      List<string> matchList = new List<string>();
//      Match splitMatch = SplitRegex.Match(col);
//      while (splitMatch.Success)
//      {
//        matchList.Add(splitMatch.Groups[0].Value);
//        splitMatch = splitMatch.NextMatch();
//      }

//      string[] xPathParts = matchList.ToArray();
//      return xPathParts;
//    }
//    private static Regex SplitRegex
//    {
//      get
//      {
//        if (_splitRegex == null)
//        {
//          string nsRegex = "({.*?}:)?";
//          string nameRegex = @"\w[\w|\d]*";
//          string countRegex = @"(\[\d+\])?";
//          string tagRegex = nsRegex + nameRegex + countRegex;
//          string attRegex = "@" + nsRegex + nameRegex;
//          string textRegex = @"text\(\)" + countRegex;//(^|\G)
//          _splitRegex = new Regex(string.Format(@"{0}|{1}|{2}",
//            tagRegex, attRegex, textRegex), RegexOptions.Compiled);
//        }
//        return _splitRegex;
//      }
//    }
//    private static Regex _splitRegex = null;

//    public override bool Equals(object obj)
//    {
//      if (obj is WebTemplate)
//      {
//        WebTemplate other = (WebTemplate)obj;
//        if (this.RowsXPath == other.RowsXPath && other.Columns.Count == this.Columns.Count)
//        {
//          for (int i = 0; i < this.Columns.Count; i++)
//            if (other.Columns[i].RelXPath != this.Columns[i].RelXPath)
//              return false;
//          return true;
//        }
//      }
//      return false;
//    }

//    public override int GetHashCode()
//    {
//      return base.GetHashCode();
//    }

//    public object Clone()
//    {
//      WebTemplate result = new WebTemplate();
//      result.FixPoints.AddRange(this.FixPoints);
//      return result;
//    }

//    public static XmlDocument Apply(WebTemplate mainTemplate, WebTemplate freshTemplate, IXPathNavigable source)
//    {
//      StringBuilder resultBuilder = new StringBuilder();
//      using (XmlWriter writer = XmlWriter.Create(resultBuilder))
//      {
//        writer.WriteStartElement("Table");
//        if (freshTemplate == null)
//          foreach (IXPathNavigable row in mainTemplate.GetRows(source))
//          {
//            writer.WriteStartElement("Row");
//            foreach (TemplateColumn column in mainTemplate.Columns)
//              writer.WriteElementString("Cell", column.GetValue(row));
//            writer.WriteEndElement();
//          }
//        else
//        {
//          List<string> mainNodesList = new List<string>();
//          foreach (IXPathNavigable row in mainTemplate.GetRows(source))
//            foreach (TemplateColumn column in mainTemplate.Columns)
//            {
//              XPathNavigator nav = XmlHlp.SelectSingleNode(row.CreateNavigator(), column.RelXPath);
//              if (nav != null)
//                mainNodesList.Add(nav.OuterXml);
//            }

//          foreach (IXPathNavigable row in freshTemplate.GetRows(source))
//          {
//            writer.WriteStartElement("Row");
//            foreach (TemplateColumn column in freshTemplate.Columns)
//            {
//              XPathNavigator nav = XmlHlp.SelectSingleNode(row.CreateNavigator(), column.RelXPath);

//              writer.WriteStartElement("Cell");
//              if (nav != null && !mainNodesList.Contains(nav.OuterXml))
//                writer.WriteAttributeString("new", "true");
//              writer.WriteString(column.GetValue(row));
//              writer.WriteEndElement();
//            }
//            writer.WriteEndElement();
//          }
//        }
//      }

//      return XmlHlp2.XmlDocFromString(resultBuilder.ToString());
//    }
//  }

}
