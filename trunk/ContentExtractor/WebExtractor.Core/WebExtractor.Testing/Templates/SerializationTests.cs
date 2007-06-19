using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Core;
using MetaTech.Library;
using System.Xml.Serialization;
using System.IO;

namespace WebExtractor_Testing.Templates
{
  [TestFixture]
  public class SerializationTests
  {
    DataTemplate template;
    [SetUp]
    public void SetUp()
    {
      template = new DataTemplate();
    }

    private void AssertSerialization()
    {
      string result = XmlSerialization2.SaveToText(template);
      Console.WriteLine(result);
      XmlSerializer s = new XmlSerializer(typeof(DataTemplate));
      DataTemplate loaded = XmlSerialization2.LoadFromText<DataTemplate>(result);
      using (StringReader reader = new StringReader(result))
      {
        loaded = (DataTemplate)s.Deserialize(reader);
      }
      Console.WriteLine(XmlSerialization2.SaveToText(loaded));
      Assert.IsTrue(DataTemplate.Equals(template, loaded), "Восстановленный шаблон не совпадает с исходным");
    }

    [Test]
    public void _001_EmptyTemplate()
    {
      AssertSerialization();
    }
    [Test]
    public void _002_OneXPathRule()
    {
      XPathDataRowRule rule = new XPathDataRowRule();
      rule.RowsXPath = "/row";
      template.Rules.Add(rule);
      AssertSerialization();
    }
    [Test]
    public void _003_TwoRules()
    {
      XPathDataRowRule rule = new XPathDataRowRule();
      rule.RowsXPath = "/row";
      template.Rules.Add(rule);
      rule = new XPathDataRowRule();
      rule.RowsXPath = "/row2";
      template.Rules.Add(rule);
      AssertSerialization();
    }
    [Test]
    public void _004_OneColumn()
    {
      template.Columns.Add(new XPathDataColumn("asdf[1]/text()"));
      template.Columns[0].Name = "Name";
      AssertSerialization();
    }
    [Test]
    public void _005_FunctionColumn()
    {
      XPathDataColumn c = new XPathDataColumn("asdf[1]/text()");
      c.Function = "SDfS";
      template.Columns.Add(c);
      AssertSerialization();
    }
    [Test]
    public void _006_NamesRowRule()
    {
      template.Rules.Add(new NamesDataRowRule());
      AssertSerialization();
    }
  }
}
