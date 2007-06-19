using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Core;
using System.Xml;
using System.Collections;
using SoftTech.Gui;
using MetaTech.Library;
using ContentExtractor.Gui;
using ContentExtractor.Gui.Templates;
using System.Text.RegularExpressions;

namespace WebExtractor_Testing.Templates
{
  [TestFixture]
  public class TemplateTests
  {
    DataTemplate template;
    XmlDocument source = new XmlDocument();
    [SetUp]
    public void SetUp()
    {
      template = new DataTemplate();
    }
    private void LoadSource(string xmlCode)
    {
      source = XmlHlp2.XmlDocFromString(string.Format("<Root><Page>{0}</Page></Root>", xmlCode));
    }
    private void AddNode(int row, int column, string xpath)
    {
      template.AddXPathColumnToPosition(row, column, xpath);
    }
    private string RowsXPath
    {
      get
      {
        XPathDataRowRule rule = (XPathDataRowRule)template.Rules[0];

        //Убираем последний квадратный скобк, потому что так делает правило
        return XPathInfo.PathWithoutBracets(rule.RowsXPath);
      }
    }
    private XPathDataColumn[] Columns
    {
      get
      {
        return template.Columns.ConvertAll<XPathDataColumn>(delegate(IDataColumn c) { return (XPathDataColumn)c; }).ToArray();
      }
    }

    [Test]
    public void _001_AddCell()
    {
      AddNode(0, 0, "/Doc[1]/text()[1]");

      Assert.AreEqual("/Doc[1]/text()", RowsXPath);
      Assert.AreEqual(".", Columns[0].RelativeXPath);
    }

    [Test]
    public void _002_TwoColumnsTemplate()
    {
      AddNode(0, 0, "/Doc[1]/Line[1]/Item[1]/text()");
      AddNode(0, 1, "/Doc[1]/Line[1]/Item[2]/text()");

      Assert.AreEqual("/Doc[1]/Line", RowsXPath);
      Assert.AreEqual("Item[1]/text()", Columns[0].RelativeXPath);
      Assert.AreEqual("Item[2]/text()", Columns[1].RelativeXPath);
    }

    [Test]
    public void _003_YandexQueryResult()
    {
      AddNode(0, 0, "/html[1]/body[1]/ol[1]/li[1]/div[1]");
      AddNode(0, 1, "/html[1]/body[1]/ol[1]/li[1]/div[1]/a[1]/@href");
      AddNode(0, 2, "/html[1]/body[1]/ol[1]/li[1]/div[1]/a[2]/@href");
      AddNode(0, 3, "/html[1]/body[1]/ol[1]/li[1]/div[2]");

      Assert.AreEqual("/html[1]/body[1]/ol[1]/li", RowsXPath);
      Assert.AreEqual("div[1]", Columns[0].RelativeXPath);
      Assert.AreEqual("div[1]/a[1]/@href", Columns[1].RelativeXPath);
      Assert.AreEqual("div[1]/a[2]/@href", Columns[2].RelativeXPath);
      Assert.AreEqual("div[2]", Columns[3].RelativeXPath);
    }

    [Test]
    public void _004_TwoCellsInSameColumn()
    {
      AddNode(0, 0, "/Doc[1]/Line[1]/text()");
      AddNode(1, 0, "/Doc[1]/Line[2]/text()");
      Assert.AreEqual("/Doc[1]/Line", RowsXPath);
      Assert.AreEqual(1, Columns.Length);
      Assert.AreEqual("text()", Columns[0].RelativeXPath);
    }

    [Test]
    public void _005_FirstPlusRowPlusColumn()
    {
      AddNode(0, 0, "/Doc[1]/Line[1]/text()[1]");
      AddNode(1, 0, "/Doc[1]/Line[2]/text()[1]");
      AddNode(0, 1, "/Doc[1]/Line[1]/a[1]");
      Assert.AreEqual("/Doc[1]/Line", RowsXPath);
      Assert.AreEqual("text()[1]", Columns[0].RelativeXPath);
      Assert.AreEqual("a[1]", Columns[1].RelativeXPath);
    }
    [Ignore]
    [Test]
    public void _006_DiagonaledCells()
    {
      AddNode(0, 0, "/Doc[1]/Line[1]/text()[1]");
      AddNode(1, 1, "/Doc[1]/Line[2]/a[1]");
      Assert.AreEqual("/Doc[1]/Line", RowsXPath);
      Assert.AreEqual("text()[1]", Columns[0].RelativeXPath);
      Assert.AreEqual("a[1]", Columns[1].RelativeXPath);
    }
    [Test]
    public void _007_TwoColumnsAddedByDiagonal()
    {
      AddNode(0, 0, "/Doc[1]/Line[1]/text()[1]");
      AddNode(1, 1, "/Doc[1]/Line[1]/a[1]");
      Assert.AreEqual("/Doc[1]/Line", RowsXPath);
      Assert.AreEqual("text()[1]", Columns[0].RelativeXPath);
      Assert.AreEqual("a[1]", Columns[1].RelativeXPath);
    }
    [Ignore]
    [Test]
    public void _008_RowsAddedDiagnoledMakeOneColumn()
    {
      AddNode(0, 0, "/Doc[1]/Line[1]/text()[1]");
      AddNode(1, 1, "/Doc[1]/Line[2]/text()[1]");
      Assert.AreEqual("/Doc[1]/Line", RowsXPath);
      Assert.AreEqual(1, Columns.Length);
      Assert.AreEqual("text()[1]", Columns[0].RelativeXPath);
    }

    [Test]
    public void _009_NameSpacedCells()
    {
      AddNode(0, 0, @"/{http://www.w3.org/1999/xhtml}:html[1]/body[1]/div[2]/div[1]/div[6]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]/table[1]/tr[2]/td[2]/text()[1]");
      AddNode(0, 1, @"/{http://www.w3.org/1999/xhtml}:html[1]/body[1]/div[2]/div[1]/div[6]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]/table[1]/tr[2]/td[3]/@href");
      Assert.AreEqual(@"/{http://www.w3.org/1999/xhtml}:html[1]/body[1]/div[2]/div[1]/div[6]/div[1]/div[1]/div[1]/div[2]/div[1]/div[2]/table[1]/tr",
        RowsXPath);
      Assert.AreEqual(2, Columns.Length);
      Assert.AreEqual(@"td[2]/text()[1]", Columns[0].RelativeXPath);
      Assert.AreEqual(@"td[3]/@href",
       Columns[1].RelativeXPath);
    }

  }

  //[TestFixture]
  //public class RowXPathTests
  //{
  //  [Test]
  //  public void RowXPathFromCell()
  //  {
  //    Assert.AreEqual("/Root", WebTemplate.GetRowXPath("/Root"));
  //    Assert.AreEqual("/Root[1]/sub[2]", WebTemplate.GetRowXPath("/Root[1]/sub[2]"));
  //  }

  //  [Test]
  //  public void RowXPathFromTwoCells()
  //  {
  //    Assert.AreEqual("/Root", WebTemplate.GetRowXPath("/Root[1]/first[1]", "/Root[1]/second[1]"));
  //  }
  //  [Test]
  //  public void RowXPathFromTwoSameCells()
  //  {
  //    Assert.AreEqual("/Root", WebTemplate.GetRowXPath("/Root[1]/item[1]", "/Root[1]/item[2]"));
  //  }
  //  [Test]
  //  public void RowXPathFromRealData()
  //  {
  //    Assert.AreEqual("/Root[1]/Sub[2]/item", WebTemplate.GetRowXPath("/Root[1]/Sub[2]/item[4]/value[1]/text()[1]",
  //                                                                    "/Root[1]/Sub[2]/item[4]/value[1]/image[2]",
  //                                                                    "/Root[1]/Sub[2]/item[4]/variable[3]"));
  //  }

  //  [Test]
  //  public void TwoSameCells()
  //  {
  //    Assert.AreEqual("/Root[1]/sub[2]", WebTemplate.GetRowXPath("/Root[1]/sub[2]", "/Root[1]/sub[2]"));
  //  }
  //}

  //[TestFixture]
  //public class TemplateBrowserTests
  //{
  //  DataTemplate template;
  //  XmlDocument sourceDoc;
  //  Cortage cortage;
  //  Browser browser;

  //  [SetUp]
  //  public void SetUp()
  //  {
  //    template = new WebTemplate();
  //    cortage = new Cortage(template, null, sourceDoc);
  //    browser = new Browser();
  //  }

  //  private void LoadSource(string xmlCode)
  //  {
  //    sourceDoc = XmlHlp2.XmlDocFromString(string.Format("<Root><Page>{0}</Page></Root>", xmlCode));
  //  }

  //  [Ignore]
  //  [Test]
  //  public void OneColumn()
  //  {
  //    template.AddNode(0, 0, "/Doc/text()");
  //    LoadSource("<Doc>some text</Doc>");

  //    cortage = new Cortage(template, null, sourceDoc);

  //    IList<IGridColumn> columns = browser.GetColumns(cortage);
  //    IList rows = browser.GetRows(cortage);
  //    Assert.AreEqual("some text", columns[0].GetValue(rows[0]));
  //  }

  //  [Ignore]
  //  [Test]
  //  public void TwoColumns()
  //  {
  //    LoadSource("<Doc><r><c>1</c><c>2</c></r><r><c>3</c><c>4</c></r></Doc>");
  //    template.AddNode(0, 0, "/Doc/r/c[1]/text()");
  //    template.AddNode(0, 1, "/Doc/r/c[2]/text()");

  //    cortage = new Cortage(template, null, sourceDoc);
  //    IList<IGridColumn> columns = browser.GetColumns(cortage);
  //    IList rows = browser.GetRows(cortage);
  //    Assert.AreEqual("1", columns[0].GetValue(rows[0]));
  //    Assert.AreEqual("2", columns[1].GetValue(rows[0]));
  //    Assert.AreEqual("3", columns[0].GetValue(rows[1]));
  //    Assert.AreEqual("4", columns[1].GetValue(rows[1]));
  //  }

  //}

}
