using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ContentExtractor.Core
{
  public class DataExport
  {
    public static string ExportToExcelML(XmlDocument templateResult)
    {
      StringBuilder table = new StringBuilder();
      table.AppendLine("<Table>");
      foreach (XmlElement row in templateResult.SelectNodes("/Table/Row"))
      {
        table.AppendLine("  <Row>");
        foreach (XmlElement cell in row.SelectNodes("Cell"))
        {
          table.AppendFormat(@"    <Cell>
      <Data ss:Type=""String"">
        <![CDATA[{0}]]>
      </Data>
    </Cell>\r\n", cell.InnerText);
        }
        table.AppendLine("  </Row>");
      }
      table.AppendLine("</Table>");

      return string.Format(Properties.Resources.ExcelMlTemplate,
        Environment.UserName, DateTime.Now, table);
    }

    public static string ExportToHtml(XmlDocument document)
    {
      StringBuilder result = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;
      settings.OmitXmlDeclaration = false;
      settings.Encoding = Encoding.UTF8;
      using (XmlWriter writer = XmlWriter.Create(result, settings))
      {
        writer.WriteStartElement("html");
        writer.WriteStartElement("head");
        writer.WriteElementString("title", "Template result");
        writer.WriteStartElement("meta");
        writer.WriteAttributeString("http-equiv", "Content-Type");
        writer.WriteAttributeString("content", "text/html; charset=UTF-8");
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteStartElement("body");
        writer.WriteStartElement("table");
        writer.WriteAttributeString("style", "border-collapse:collapse");
        writer.WriteAttributeString("bordercolor", "#aaaaaa");
        writer.WriteAttributeString("border", "true");
        foreach (XmlElement row in document.SelectNodes("/Table/Row"))
        {
          writer.WriteStartElement("tr");
          foreach (XmlElement cell in row.SelectNodes("Cell"))
          {
            writer.WriteStartElement("td");
            writer.WriteStartElement("div");
            cell.WriteContentTo(writer);
            writer.WriteEndElement();
            writer.WriteEndElement();
          }
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
      return result.ToString();
    }
  }
}
