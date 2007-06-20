using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using MetaTech.Library;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections;

namespace ContentExtractor.Core
{
  public interface IDataRow
  {
    IDataRowRule ParentRule { get;}
    object Value { get;}
  }

  public interface IDataRowRule : ICloneable
  {
    IDataRow[] GetRows(IXPathNavigable document);
  }

  [TypeConverter(typeof(ExpandableObjectConverter))]
  public interface IDataColumn : ICloneable
  {
    object GetValue(IDataRow row);

    string Name
    {
      get;
      set;
    }
  }

  [NineRays.Obfuscator.NotObfuscate]
  public class DataTemplate : ICloneable//, IXmlSerializable
  {
    [XmlIgnore]
    public readonly List<IDataRowRule> Rules = new List<IDataRowRule>();
    [XmlIgnore]
    public readonly List<IDataColumn> Columns = new List<IDataColumn>();

    [XmlArrayItem(Type = typeof(XPathDataRowRule)), XmlArrayItem(Type = typeof(NamesDataRowRule))]
    public object[] XmlRules
    {
      get
      {
        return Rules.ToArray();
      }
      set
      {
        Rules.Clear();
        Rules.AddRange(_.From<IDataRowRule>(value));
      }
    }

    [XmlArrayItem(Type = typeof(XPathDataColumn))]
    public object[] XmlColumns
    {
      get
      {
        return Columns.ToArray();
      }
      set
      {
        Columns.Clear();
        Columns.AddRange(_.From<IDataColumn>(value));
      }
    }


    internal List<IDataRow> GetRows(IXPathNavigable document)
    {
      List<IDataRow> result = new List<IDataRow>();
      foreach (IDataRowRule rule in Rules)
        result.AddRange(rule.GetRows(document));
      return result;
    }

    internal List<XPathDataColumn> XColumns
    {
      get
      {
        List<XPathDataColumn> result = new List<XPathDataColumn>();
        foreach (IDataColumn c in Columns)
          if (c is XPathDataColumn)
            result.Add((XPathDataColumn)c);
        return result;
      }
    }

    private XPathDataRowRule RowRule
    {
      get
      {
        foreach (IDataRowRule rule in Rules)
          if (rule is XPathDataRowRule)
            return (XPathDataRowRule)rule;
        return null;
      }
    }

    private int GetIndex(IDataColumn column)
    {
      return Columns.FindIndex(delegate(IDataColumn c) { return IDataColumn.Equals(c, column); });
    }

    public void AddXPathColumnToPosition(int rowIndex, int columnIndex, string xpath)
    {
      if (columnIndex < 0 || rowIndex < 0)
        return;
      List<XPathDataColumn> xColumns = this.XColumns;
      bool insert = true;
      string rowPath = xpath;
      for (int i = 0; i < xColumns.Count; i++)
      {
        string absPath = XPathInfo.Combine(RowRule.RowsXPath, xColumns[i].RelativeXPath);
        if (columnIndex == GetIndex(xColumns[i]))
        {
          insert = false;
          rowPath = XPathInfo.CommonPart(rowPath, XPathInfo.RowCommonPart(absPath, xpath));
        }
        else
          rowPath = XPathInfo.CommonPart(rowPath, absPath);
      }

      for (int i = 0; i < xColumns.Count; i++)
        xColumns[i].RelativeXPath = XPathInfo.Relative(XPathInfo.Combine(RowRule.RowsXPath, xColumns[i].RelativeXPath), rowPath);

      if (RowRule == null)
        Rules.Add(new XPathDataRowRule());
      RowRule.RowsXPath = rowPath;
      if (insert)
      {
        XPathDataColumn col = new XPathDataColumn();
        col.RelativeXPath = XPathInfo.Relative(xpath, rowPath);
        this.Columns.Insert(Math.Min(columnIndex, this.Columns.Count), col);
      }
    }

    internal XmlDocument Apply(IXPathNavigable document)
    {
      StringBuilder resultBuilder = new StringBuilder();
      XmlDocument result = new XmlDocument();
      using (XmlWriter writer = XmlWriter.Create(resultBuilder))
      {
        writer.WriteStartElement("Table");
        foreach (IDataRow row in GetRows(document))
        {
          writer.WriteStartElement("Row");
          foreach (IDataColumn column in Columns)
          {
            writer.WriteStartElement("Cell");
            object value = column.GetValue(row);
            WriteCellValue(writer, value);
            writer.WriteEndElement();
          }
          writer.WriteEndElement();
        }
      }
      return XmlHlp2.XmlDocFromString(resultBuilder.ToString());
    }

    private static void WriteCellValue(XmlWriter writer, object value)
    {
      if (value is IXPathNavigable)
      {
        XPathNavigator nav = ((IXPathNavigable)value).CreateNavigator();
        if (nav.NodeType == XPathNodeType.Attribute)
          writer.WriteString(nav.Value);
        else
          nav.WriteSubtree(writer);
      }
      else if (value is XPathNodeIterator)
      {
        XPathNodeIterator iterator = (XPathNodeIterator)value;
        foreach (XPathNavigator child in iterator)
          child.WriteSubtree(writer);
      }
      else
        writer.WriteString(ObjectHlp2.ToString(value, ""));
    }

    public static XmlDocument Apply(DataTemplate mainTemplate, DataTemplate freshTemplate, IXPathNavigable source)
    {
      if (freshTemplate == null)
        return mainTemplate.Apply(source);

      StringBuilder resultBuilder = new StringBuilder();
      using (XmlWriter writer = XmlWriter.Create(resultBuilder))
      {
        writer.WriteStartElement("Table");
        List<object> mainNodesList = new List<object>();
        foreach (IDataRow row in mainTemplate.GetRows(source))
          foreach (IDataColumn column in mainTemplate.Columns)
          {
            mainNodesList.Add(column.GetValue(row));
          }

        foreach (IDataRow row in freshTemplate.GetRows(source))
        {
          writer.WriteStartElement("Row");
          foreach (IDataColumn column in freshTemplate.Columns)
          {
            object value = column.GetValue(row);

            writer.WriteStartElement("Cell");
            if (value != null && !mainNodesList.Contains(value))
              writer.WriteAttributeString("new", "true");
            WriteCellValue(writer, value);
            writer.WriteEndElement();
          }
          writer.WriteEndElement();
        }
      }

      return XmlHlp2.XmlDocFromString(resultBuilder.ToString());
    }


    #region overridables
    public object Clone()
    {
      DataTemplate result = new DataTemplate();
      result.Rules.AddRange(WebExtractorHlp.CopyList(this.Rules));
      result.Columns.AddRange(WebExtractorHlp.CopyList(this.Columns));
      return result;
    }
    public override bool Equals(object obj)
    {
      if (obj is DataTemplate)
      {
        DataTemplate other = (DataTemplate)obj;
        return WebExtractorHlp.CompareList(this.Rules, other.Rules) && WebExtractorHlp.CompareList(this.Columns, other.Columns);
      }
      return false;
    }
    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
    #endregion
  }

  public class XPathInfo
  {
    public static XPathInfo Parse(string xpath)
    {
      lock (cache)
      {
        if (!cache.ContainsKey(xpath))
          cache[xpath] = new XPathInfo(xpath);
        return cache[xpath];
      }
    }
    private static Dictionary<string, XPathInfo> cache = new Dictionary<string, XPathInfo>();

    private XPathInfo(string xpath)
    {
      this.XPath = xpath;
    }

    public readonly string XPath;

    private string[] _parts = null;
    public string[] Parts
    {
      get
      {
        if (_parts == null)
        {
          List<string> result = new List<string>();
          Match m = partRegex.Match(XPath);
          if (m.Success && m.Groups["part"].Success)
            for (int i = 0; i < m.Groups["part"].Captures.Count; i++)
              result.Add(m.Groups["part"].Captures[i].Value);
          _parts = result.ToArray();
        }
        return _parts;
      }
    }

    private const string PartsPatternt = @"^((?<part>(" + XPathPart.Pattern + ")?)/)?((?<part>" + XPathPart.Pattern + ")/)*(?<part>" + XPathPart.Pattern + ")$";
    private Regex partRegex = new Regex(PartsPatternt, RegexOptions.Compiled);
    private XPathPart[] _xparts = null;
    public XPathPart[] XParts
    {
      get
      {
        if (_xparts == null)
        {
          _xparts = Array.ConvertAll<string, XPathPart>(Parts,
            delegate(string part)
            {
              if (!string.IsNullOrEmpty(part))
                return new XPathPart(part);
              else
                return null;
            });
        }
        return _xparts;
      }
    }

    public static string CommonPart(string leftPath, string rightPath)
    {
      XPathInfo left = new XPathInfo(leftPath);
      XPathInfo right = new XPathInfo(rightPath);
      //StringBuilder result = new StringBuilder();
      int i;
      List<string> result = new List<string>();
      for (i = 0; i < Math.Min(left.Parts.Length, right.Parts.Length); i++)
      {
        if (left.Parts[i] == right.Parts[i])
          result.Add(left.Parts[i]);
        else
        {
          string lShort = PathWithoutBracets(left.Parts[i]);
          string rShort = PathWithoutBracets(right.Parts[i]);
          //if (lShort == rShort)
          //  result.Add(lShort);
          if (lShort == right.Parts[i])
            result.Add(lShort);
          else if (rShort == left.Parts[i])
            result.Add(rShort);
          break;
        }
      }
      if (i > 0)
      {
        return string.Join("/", result.ToArray());
      }
      else
        return ".";
    }

    public static string Relative(string nodePath, string rootPath)
    {
      XPathInfo node = new XPathInfo(nodePath);
      XPathInfo root = new XPathInfo(rootPath);
      if (node.Parts.Length >= root.Parts.Length)
      {
        int i;
        for (i = 0; i < root.Parts.Length - 1; i++)
          if (node.Parts[i] != root.Parts[i])
            return null;
        if (node.Parts[i].StartsWith(root.Parts[i]))
          i++;
        else
          return null;

        string result = string.Join("/", node.Parts, i, node.Parts.Length - i);
        if (!string.IsNullOrEmpty(result))
          return result;
        else
          return ".";
      }
      else
        return null;
    }

    public static string Combine(string node, string relative)
    {
      if (node == ".")
        return relative;
      if (relative == ".")
        return node;
      return node + "/" + relative;
    }

    public static string RowCommonPart(string leftPath, string rightPath)
    {
      string root = CommonPart(leftPath, rightPath);
      XPathInfo left = new XPathInfo(Relative(leftPath, root));
      XPathInfo right = new XPathInfo(Relative(rightPath, root));

      if (left.Parts.Length > 0 && right.Parts.Length > 0)
      {
        string leftPart = PathWithoutBracets(left.Parts[0]);
        if (!string.IsNullOrEmpty(leftPart) && leftPart == PathWithoutBracets(right.Parts[0]))
          return root + "/" + leftPart;
      }
      return root;
    }

    public static string PathWithoutBracets(string path)
    {
      Match m = Regex.Match(path, @"(?<res>.*?)(\[\d+\])?$", RegexOptions.Compiled);
      if (m.Success)
        return m.Groups["res"].Value;
      else
        return path;
    }

  }

  public enum XPathPartType
  {
    Element,
    Text,
    Attribute
  }

  public class XPathPart
  {
    public readonly XPathPartType Type;
    public readonly string FullName;
    public readonly string Namespace = null;
    public readonly string LocalName;
    public readonly int? Index;

    public const string NamespacePattern = @"({(?<space>.*?)}:)?";
    public const string TextPattern = @"(?<text>text\(\))";
    public const string ElementPattern = @"(?<elemName>\w[\w\d]*)";
    public const string AttrubutePattern = @"(@(?<attrName>\w[\w\d]*))";
    public const string IndexPattern = @"(\[(?<index>\d+)\])?";
    public const string Pattern = NamespacePattern + "(" + AttrubutePattern + "|" + TextPattern + "|" +
      ElementPattern + ")" + IndexPattern;

    private static Regex regex = new Regex("^" + Pattern + "$", RegexOptions.Compiled | RegexOptions.Singleline);

    public XPathPart(string fullName)
    {
      this.FullName = fullName;
      Match m = regex.Match(fullName);
      if (m.Success)
      {
        if (m.Groups["elemName"].Success)
        {
          Type = XPathPartType.Element;
          LocalName = m.Groups["elemName"].Value;
        }
        if (m.Groups["attrName"].Success)
        {
          Type = XPathPartType.Attribute;
          LocalName = m.Groups["attrName"].Value;
        }
        if (m.Groups["text"].Success)
        {
          Type = XPathPartType.Text;
          LocalName = m.Groups["text"].Value;
        }
        if (m.Groups["space"].Success)
          Namespace = m.Groups["space"].Value;
        if (m.Groups["index"].Success)
          Index = int.Parse(m.Groups["index"].Value);
      }
      else
        throw new ArgumentException(fullName);
    }
  }

}
