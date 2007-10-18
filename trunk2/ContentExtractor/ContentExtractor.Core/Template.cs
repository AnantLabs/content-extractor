//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 05.07.2007
// Time: 13:02
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace ContentExtractor.Core
{
  /// <summary>
  /// Describes transformation template from html pages into a table
  /// </summary>
  public class Template
  {
    public Template()
    {
    }

    public const string CexNamespace = "http://contentextractor.com/documentschema";
    public const string CexPrefix = "cex";

    public const string DocumentTag = "Document";
    public const string RowTag = "Row";
    public const string CellTag = "Cell";

    [XmlElement("RowXPath")]
    public string rowXPath_ = ".";

    [XmlIgnore]
    public string RowXPath
    {
      get { return XPathInfo.GetPathWithoutBracets(rowXPath_); }
      set { rowXPath_ = value; }
    }

    [XmlArray("Columns")]
    public Column[] XmlColumns
    {
      get { return Columns.ToArray(); }
      set { Columns = new List<Column>(value); }
    }

    [XmlIgnore]
    public List<Column> Columns = new List<Column>();

    public XmlDocument Transform(XmlNode input)
    {
      XmlDocument result = GetResultBillet();
      TransformNode(result.DocumentElement, input);

      Console.WriteLine(result.OuterXml);
      return result;
    }

    public XmlDocument Transform(IEnumerable input)
    {
      XmlDocument result = GetResultBillet();
      foreach (XmlNode inNode in input)
        TransformNode(result.DocumentElement, inNode);
      return result;
    }

    private void TransformNode(XmlNode outDoc, XmlNode input)
    {
      if (Columns.Count > 0)
      {
        foreach (XmlNode inRow in input.SelectNodes(RowXPath))
        {
          XmlElement outRow = outDoc.OwnerDocument.CreateElement(CexPrefix,
                                                                 RowTag,
                                                                 CexNamespace);
          outDoc.AppendChild(outRow);
          foreach (Column column in Columns)
          {
            XmlElement outCell = outDoc.OwnerDocument.CreateElement(CexPrefix,
                                                                    CellTag,
                                                                    CexNamespace);
            outRow.AppendChild(outCell);
            foreach (XmlNode subNode in inRow.SelectNodes(column.XPath))
            {
              if (subNode.NodeType != XmlNodeType.Attribute)
                outCell.AppendChild(outDoc.OwnerDocument.ImportNode(subNode, true));
              else
                outCell.AppendChild(outDoc.OwnerDocument.CreateTextNode(subNode.Value));
              outCell.AppendChild(outDoc.OwnerDocument.CreateWhitespace(" "));
            }
          }
        }
      }
    }

    private static XmlDocument GetResultBillet()
    {
      XmlDocument result = new XmlDocument();
      result.AppendChild(result.CreateElement(CexPrefix, DocumentTag, CexNamespace));
      return result;
    }

    /// <summary>
    /// Adds new column to the template. The template row xpath and column xpaths
    /// are changing like all added columns and the new one was in one row.
    /// </summary>
    /// <param name="xpath">XPath for adding cell</param>
    /// <returns>true if column was successfully added.</returns>
    public bool AddColumn(string xpath)
    {
      string rowPath = xpath;
      for (int i = 0; i < Columns.Count; i++)
      {
        string absPath = XPathInfo.CombineXPaths(rowXPath_, Columns[i].XPath);
        rowPath = XPathInfo.GetXPathsCommonPart(rowPath, absPath);
      }

      for (int i = 0; i < Columns.Count; i++)
      {
        Columns[i].XPath = 
            XPathInfo.GetRelativeXPath(XPathInfo.CombineXPaths(rowXPath_, Columns[i].XPath), rowPath);
      }

      rowXPath_ = rowPath;
      Columns.Add(new Column(XPathInfo.GetRelativeXPath(xpath, rowPath)));
      return true;
    }

    public void AddEmptyColumn()
    {
      Columns.Add( new Column());
    }

    public bool CheckRowXPath(string xpath)
    {
      try
      {
        XmlDocument doc = XmlUtils.LoadXml("<doc><node>text</node></doc>");
        doc.SelectNodes(xpath);
        //System.Xml.XPath.XPathExpression.Compile(xpath);
        return true;
      }
      catch (System.Xml.XPath.XPathException)
      {
        return false;
      }
    }

    public bool CheckColumnXPath(string xpath)
    {
      return xpath != null && !xpath.Trim().StartsWith("/") && CheckRowXPath(xpath);
    }

    public bool CanAutoModifyTemplate
    {
      get
      {
        bool result = XPathInfo.Parse(rowXPath_).IsParsedRight;
        if (result)
          foreach (Column column in Columns)
            result &= XPathInfo.Parse(column.XPath).IsParsedRight;
        return result;
      }
    }
  }

  public class Column
  {
    public string XPath;

    public Column(string xpath)
    {
      this.XPath = xpath;
    }

    public Column()
      : this(".")
    { }
  }
}
