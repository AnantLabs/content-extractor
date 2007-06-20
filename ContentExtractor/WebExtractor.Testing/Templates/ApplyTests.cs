using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Core;
using MetaTech.Library;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace WebExtractor_Testing.Templates
{
  //[TestFixture]
  //public class ApplyTests
  //{
  //  WebTemplate main;
  //  WebTemplate fresh;

  //  [SetUp]
  //  public void SetUp()
  //  {
  //    main = new WebTemplate();
  //    fresh = new WebTemplate();
  //  }

  //  XmlDocument LoadSource(string code)
  //  {
  //    return XmlHlp2.XmlDocFromString(string.Format("{0}", code));
  //    //return XmlHlp2.XmlDocFromString(string.Format("<Root><Page>{0}</Page></Root>", code));
  //  }

  //  XmlDocument ResultFrom(string code)
  //  {
  //    return WebTemplate.Apply(main, fresh, LoadSource(code));
  //  }

  //  XmlDocument Expected(params string[] rows)
  //  {
  //    return XmlHlp2.XmlDocFromString(string.Format("<Table>{0}</Table>",
  //      StringHlp2.Join("", "<Row>{0}</Row>", rows)));
  //  }

  //  [Test]
  //  public void AddOneCell()
  //  {
  //    fresh.AddNode(0, 0, "/Doc[1]/Row[1]/Cell[1]");
  //    XmlDocument result = WebTemplate.Apply(main, fresh,
  //    LoadSource(@"<Doc><Row><Cell>Text</Cell></Row></Doc>"));

  //    TestUtils.XmlAssertEqual(
  //      XmlHlp2.XmlDocFromString("<Table><Row><Cell new='true'>Text</Cell></Row></Table>"),
  //      result);
  //  }
  //  [Test]
  //  public void AddSecondCellToRow()
  //  {
  //    main.AddNode(0, 0, "/Doc[1]/Row[1]/Cell[1]");
  //    fresh.AddNode(0, 0, "/Doc[1]/Row[1]/Cell[1]");
  //    fresh.AddNode(0, 1, "/Doc[1]/Row[1]/Cell[2]");

  //    XmlDocument result = ResultFrom(@"<Doc><Row><Cell>Text 1</Cell><Cell>Text 2</Cell></Row></Doc>");

  //    TestUtils.XmlAssertEqual(Expected("<Cell>Text 1</Cell><Cell new='true'>Text 2</Cell>"),
  //      result);
  //  }
  //}
}
