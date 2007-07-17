//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 05.07.2007
// Time: 12:29
//

using NUnit.Framework;
using System;
using System.IO;

using ContentExtractor.Core;

namespace ContentExtractorTests.Core
{
  [TestFixture]
  public class LoaderTest
  {
    private string filename;
    
    [SetUp]
    public void SetUp()
    {
      filename = Path.GetFullPath("data.html");
    }
    
    [TearDown]
    public void TearDown()
    {
      if(File.Exists(filename))
        File.Delete(filename);
    }
    
    [Test]
    public void Test()
    {
      string code = "Some wonderful text!";
      File.WriteAllText(filename, code);
      Uri url = Utils.ParseUrl(filename);
      Assert.AreEqual(code, Loader.Instance.LoadSync(url));
    }
  }
}
