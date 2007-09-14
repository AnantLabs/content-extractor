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
              outCell.AppendChild(outDoc.OwnerDocument.CreateWhitespace(" "));
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
        string absPath = XPathInfo.CombineXPaths(_rowXPath, Columns[i]);
        rowPath = XPathInfo.GetXPathsCommonPart(rowPath, absPath);
      }

      for (int i = 0; i < Columns.Count; i++)
        Columns[i] = XPathInfo.GetRelativeXPath(XPathInfo.CombineXPaths(_rowXPath, Columns[i]), rowPath);

      _rowXPath = rowPath;
      Columns.Add(XPathInfo.GetRelativeXPath(xpath, rowPath));
      return true;
    }

    public void AddEmptyColumn()
    {
      Columns.Add(".");
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

    public bool CanAutoModifyTemplate
    {
      get
      {
        bool result = XPathInfo.Parse(_rowXPath).IsParsedRight;
        if (result)
          foreach (string column_xpath in Columns)
            result &= XPathInfo.Parse(column_xpath).IsParsedRight;
        return result;
      }
    }
  }
}
