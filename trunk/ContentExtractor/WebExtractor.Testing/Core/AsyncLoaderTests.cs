//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 26.06.2007
// Time: 19:53
//

using System;
using System.IO;
using NUnit.Framework;
using ContentExtractor.Core;

namespace WebExtractor_Testing.Core
{
	/// <summary>
	/// Test AsyncLoader class.
	/// </summary>
	[TestFixture]
	public class AsyncLoaderTests
	{
	  [Test]
	  public void SyncLoading()
	  {
	    string filename = "document.html";
	    if(File.Exists(filename))
	      File.Delete(filename);
	    try
	    {
	      string expectedContent = "Here some bad code <&^^^sdafa s>//='>><'''asdf";
	      File.WriteAllText(filename, expectedContent);
	      WebPosition position = WebPosition.Parse(Path.GetFullPath(filename));
  	    string actualContent = AsyncLoader.Instance.Load(position);
  	    Assert.AreEqual(expectedContent, actualContent);
	    }
	    finally
	    {
	      File.Delete(filename);
	    }
	  }
	}
}
