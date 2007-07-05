//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 26.06.2007
// Time: 19:42
//

using System;
using System.IO;
using NUnit.Framework;

using ContentExtractor.Core;

namespace ContentExtractorTests
{
  [TestFixture]
	public class UtilsTests
	{
	  [Test]
	  public void ParseFileName()
	  {
	    Uri position = Utils.ParseUrl(Path.GetFullPath("file.html"));
	    Assert.IsNotNull(position, "Couldn't parse local file as Uri");
	  }

	  [Test]
	  public void ParseDnsName()
	  {
	    Uri position = Utils.ParseUrl("www.google.com");
	    Assert.IsNotNull(position, "Couldn't parse local file as Uri");
	    Assert.AreEqual("http://www.google.com/", position.ToString());
	  }
	}
}
