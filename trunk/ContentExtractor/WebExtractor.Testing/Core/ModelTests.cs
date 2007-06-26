using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Core;
using System.Xml;
using MetaTech.Library;
using System.IO;

namespace WebExtractor_Testing.Core
{
  [TestFixture]
  public class ModelTests
  {
    Model model;

    [SetUp]
    public void SetUp()
    {
      model = new Model();
    }

    [Test]
    public void NameSpacedSourceTree()
    {
      string fileName = ApplicationHlp.MapPath("input.html");
      try
      {
        File.WriteAllText(fileName, "<html xmlns='http://www.w3.org/1999/xhtml'><body><p>text</p></body></html>");
        model.PositionsList.Add(WebPosition.Parse(fileName));
        do
        {
          Console.Write("{0} ", model.PositionsList[0].DocumentText.Length);
          System.Threading.Thread.Sleep(100);
        } while (AsyncLoader.Instance.HasWork);
        Console.WriteLine();
        Console.WriteLine(model.SourceTree.CreateNavigator().OuterXml);

        Console.WriteLine(model.PositionsList[0].XmlDocument.OuterXml);
      }
      finally
      {
        if (File.Exists(fileName))
          File.Delete(fileName);
      }
    }
  }
}
