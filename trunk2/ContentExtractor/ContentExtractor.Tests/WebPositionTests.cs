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
	public class WebPositionTests
	{
	  [Test]
	  public void ParseFileName()
	  {
	    WebPosition position = WebPosition.Parse(Path.GetFullPath("file.html"));
	    Assert.IsNotNull(position, "Couldn't parse local file as WebPosition");
	  }

	  [Test]
	  public void ParseDnsName()
	  {
	    WebPosition position = WebPosition.Parse("www.google.com");
	    Assert.IsNotNull(position, "Couldn't parse local file as WebPosition");
	    Assert.AreEqual("http://www.google.com/", position.Url.ToString());
	  }
	}
}
