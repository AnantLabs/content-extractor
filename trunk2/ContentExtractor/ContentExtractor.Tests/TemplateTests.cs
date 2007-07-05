//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 05.07.2007
// Time: 13:01
//

using NUnit.Framework;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

using ContentExtractor.Core;

namespace ContentExtractorTests
{
  [TestFixture]
  public class TemplateTests
  {
    Template template;
    [SetUp]
    public void SetUp()
    {
      template = new Template();
    }
    
    private string Result(params string[] rows)
    {
      StringBuilder result = new StringBuilder();
      result.AppendFormat("<{0}:{1} xmlns:{0}='{2}'>",
                          Template.CEXPrefix,
                          Template.DocumentTag,
                          Template.CEXNamespace);
      foreach(string row in rows)
        result.Append(row);
      result.AppendFormat("</{0}:{1}>",
                          Template.CEXPrefix,
                          Template.DocumentTag);
      return result.ToString();
    }
    
    private string Row(params string[] cells)
    {
      StringBuilder result = new StringBuilder();
      result.AppendFormat("<{0}:{1}>", Template.CEXPrefix, Template.RowTag);
      foreach(string cell in cells)
      {
        result.AppendFormat("<{0}:{1}>", Template.CEXPrefix, Template.CellTag);
        result.Append(cell);
        result.AppendFormat("</{0}:{1}>", Template.CEXPrefix, Template.CellTag);
      }
      result.AppendFormat("</{0}:{1}>", Template.CEXPrefix, Template.RowTag);
      return result.ToString();
    }
    
    [Test]
    public void SimplestTemplate()
    {
      template.RowXPath = "/html/body/p";
      template.Columns.Add("text()");

      XmlDocument input =
        XmlUtils.LoadXml("<html><body><p> #1 </p><p>#2</p></body></html>");
      
      XmlDocument result = template.Transform(input);
      TestUtils.AssertXmlAreEqual(Result(Row("#1"),Row("#2")),
                                  result);
    }
    
    [Test]
    public void MultiplyDocumentsTransform()
    {
      List<XmlDocument> input = new List<XmlDocument>();
      input.Add(
        XmlUtils.LoadXml("<A>" +
                         "<B><Col1>value</Col1> <Col2>1024</Col2></B>" +
                         "<B><Col1>text </Col1> <Col2>0811</Col2></B>" +
                         "</A>"));
      input.Add(
        XmlUtils.LoadXml("<A>" +
                         "<B><Col1>string</Col1> <Col2>-519.12</Col2></B>" +
                         "<B><Col1>number</Col1> <Col2>+1114.0</Col2></B>" +
                         "</A>"));
      
      template.RowXPath = "/A/B";
      template.Columns.Add("Col1/text()");
      template.Columns.Add("Col2/text()");
      
		  XmlDocument result = template.Transform(input);
      TestUtils.AssertXmlAreEqual(
		    Result(
		      Row("value", "1024"),
		      Row("text", "0811"),
		      Row("string", "-519.12"),
		      Row("number", "+1114.0")),
		    result);
    }
  }
}
