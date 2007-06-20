using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Gui.Templates;
using ContentExtractor.Core;

namespace WebExtractor_Testing.Utils
{
  [TestFixture]
  public class NextExcelNameTests
  {
    private void AssertConversion(string expected, string argument)
    {
      Assert.AreEqual(expected, WebExtractorHlp.NextExcelName(argument));
    }

    [Test]
    public void A_to_B()
    {
      AssertConversion("B", "A");
    }

    [Test]
    public void F_to_G()
    {
      AssertConversion("G", "F");
    }
    [Test]
    public void Z_to_AA()
    {
      AssertConversion("AA", "Z");
    }
    [Test]
    public void AU_to_AV()
    {
      AssertConversion("AV", "AU");
    }
    [Test]
    public void AZ_to_BA()
    {
      AssertConversion("BA", "AZ");
    }
  }
}
