﻿//
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
    
    public string RowXPath = ".";
    
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
      foreach(XmlNode inNode in input)
        TransformNode(result.DocumentElement, inNode);
      return result;
    }
    
    private void TransformNode(XmlNode outDoc, XmlNode input)
    {
      foreach(XmlNode inRow in input.SelectNodes(RowXPath))
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
            outCell.AppendChild(outDoc.OwnerDocument.ImportNode(subNode, true));
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
  }
}