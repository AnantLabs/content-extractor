using System;
using System.Collections.Generic;
using System.Text;
using SoftTech.Gui;
using System.Xml;
using MetaTech.Library;
using ContentExtractor.Core;
using System.Drawing;

namespace ContentExtractor.Gui.Templates
{
  //public class Cortage
  //{
  //  public Cortage(WebTemplate template, WebTemplate preFixTemplate, XmlDocument doc)
  //  {
  //    this.Template = template;
  //    this.PreFixTemplate = preFixTemplate;
  //    this.Document = doc;
  //  }
  //  public readonly WebTemplate Template;
  //  public readonly WebTemplate PreFixTemplate;
  //  public readonly XmlDocument Document;
  //}

  internal class Browser : IGridBrowser
  {
    public IList<IGridColumn> GetColumns(object collection)
    {
      string columnName = "A";
      List<IGridColumn> result = new List<IGridColumn>();
      for (int i = 0; i < 30; i++)
      {
        IGridColumn column = GetColumn(columnName, i);
        result.Add(column);
        columnName = WebExtractorHlp.NextExcelName(columnName);
      }
      return result;
    }

    private IGridColumn GetColumn(string columnName, int i)
    {
      string xpath = string.Format("Cell[{0}]", i + 1);
      IGridColumn column = new SimpleColumn<XmlNode>(
        columnName,
        delegate(XmlNode node)
        {
          //":" +
          return ColumnGetValue(xpath, node).ToString();
        }, 80
        , new ColumnExtensionAttribute("CellsBackColor", Colorer(i + 1))
        );
      return column;
    }

    private static object ColumnGetValue(string xpath, XmlNode node)
    {
      XmlNode cell = XmlHlp.SelectSingleNode(node, xpath);
      if (cell != null)
      {
        StringBuilder result = new StringBuilder(cell.InnerXml);
        result.Replace('\r', ' ');
        result.Replace('\n', ' ');
        result.Replace('\t', ' ');
        return result.ToString().Trim();
      }
      else
        return string.Empty;
    }

    private Getter<Color, object> Colorer(int colIndex)
    {
      return delegate(object o)
      {
        string xpath = string.Format("Cell[{0}][@new = 'true']", colIndex);
        if (o != null && ((XmlNode)o).SelectSingleNode(xpath) != null)
          return Color.LightBlue;
        else
          return Color.FromKnownColor(KnownColor.Window);
      };
    }

    private const int minRowsNumber = 100;

    public System.Collections.IList GetRows(object collection)
    {
      XmlDocument doc = (XmlDocument)collection;
      List<XmlNode> result = CollectionHlp.From<XmlNode>(doc.SelectNodes("/Table/Row"));

      while (result.Count < minRowsNumber)
        result.Add(null);
      return result;
    }
  }
}
