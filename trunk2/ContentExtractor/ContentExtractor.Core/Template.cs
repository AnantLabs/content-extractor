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

    public const string CEXNamespace = "http://contentextractor.com/documentschema";
    public const string CEXPrefix = "cex";

    public const string DocumentTag = "Document";
    public const string RowTag = "Row";
    public const string CellTag = "Cell";

    [XmlElement("RowXPath")]
    public string _rowXPath = ".";

    [XmlIgnore]
    public string RowXPath
    {
      get { return XPathInfo.GetPathWithoutBracets(_rowXPath); }
      set { _rowXPath = value; }
    }

    [XmlArray("Columns")]
    public string[] XmlColumns
    {
      get { return Columns.ToArray(); }
      set { Columns = new List<string>(value); }
    }

    [XmlIgnore]
    public List<string> Columns = new List<string>();

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
          XmlElement outRow = outDoc.OwnerDocument.CreateElement(CEXPrefix,
                                                                 RowTag,
                                                                 CEXNamespace);
          outDoc.AppendChild(outRow);
          foreach (string cellXPath in Columns)
          {
            XmlElement outCell = outDoc.OwnerDocument.CreateElement(CEXPrefix,
                                                                    CellTag,
                                                                    CEXNamespace);
            outRow.AppendChild(outCell);
            foreach (XmlNode subNode in inRow.SelectNodes(cellXPath))
            {
              if (subNode.NodeType != XmlNodeType.Attribute)
                outCell.AppendChild(outDoc.OwnerDocument.ImportNode(subNode, true));
              else
                outCell.AppendChild(outDoc.OwnerDocument.CreateTextNode(subNode.Value));
            }
          }
        }
      }
    }

    private static XmlDocument GetResultBillet()
    {
      XmlDocument result = new XmlDocument();
      result.AppendChild(result.CreateElement(CEXPrefix, DocumentTag, CEXNamespace));
      return result;
    }

    public void AddColumn(string xpath)
    {
      //List<XPathDataColumn> xColumns = this.XColumns;
      //bool insert = true;
      string rowPath = xpath;
      for (int i = 0; i < Columns.Count; i++)
      {
        string absPath = XPathInfo.CombineXPaths(_rowXPath, Columns[i]);
        //XPathInfo.Combine(RowRule.RowsXPath, xColumns[i].RelativeXPath);
        rowPath = XPathInfo.GetXPathsCommonPart(rowPath, absPath);
      }

      for (int i = 0; i < Columns.Count; i++)
        Columns[i] = XPathInfo.GetRelativeXPath(XPathInfo.CombineXPaths(_rowXPath, Columns[i]), rowPath);
      //XPathInfo.Relative(XPathInfo.Combine(RowRule.RowsXPath, xColumns[i].RelativeXPath), rowPath);

      //if (RowRule == null)
      //  Rules.Add(new XPathDataRowRule());
      //RowRule.RowsXPath = rowPath;
      _rowXPath = rowPath;
      Columns.Add(XPathInfo.GetRelativeXPath(xpath, rowPath));
      //if (insert)
      //{
      //  XPathDataColumn col = new XPathDataColumn();
      //  col.RelativeXPath = XPathInfo.Relative(xpath, rowPath);
      //  this.Columns.Insert(Math.Min(columnIndex, this.Columns.Count), col);
      //}
    }

    public bool CheckRowXPath(string xpath)
    {
      try
      {
        System.Xml.XPath.XPathExpression.Compile(xpath);
        return true;
      }
      catch (System.Xml.XPath.XPathException)
      {
        return false;
      }
    }

    public bool CheckColumnXPath(string xpath)
    {
      return !xpath.Trim().StartsWith("/") && CheckRowXPath(xpath);
    }
  }
}
