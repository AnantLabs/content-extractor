//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 13.07.2007
// Time: 19:35
//

using NUnit.Framework;
using System;
using System.Reflection;
using System.Windows.Forms;

using ContentExtractor.Gui;
using ContentExtractor.Core;

namespace ContentExtractorTests.Gui
{
  [TestFixture]
  public class UrlsListBoxTests
  {
    private State state;
    private UrlsListBox urlsBox;
    
    [SetUp]
    public void SetUp()
    {
      state = new State();
      urlsBox = new UrlsListBox();
      urlsBox.SetState(state);
    }
    
    private void AssertAllUrlsVisible()
    {
      FieldInfo field = typeof(UrlsListBox).GetField(
        "listBox1",
        BindingFlags.NonPublic | BindingFlags.Instance);
      ListBox box = (ListBox)field.GetValue(urlsBox);
      Assert.AreEqual(state.Project.SourceUrls.Count, box.Items.Count);
    }
    
    [Test]
    public void AddingItemsChangeUls()
    {
      Assert.AreEqual(0, state.Project.SourceUrls.Count);
      AssertAllUrlsVisible();
      Uri uri = Utils.ParseUrl(@"c:\data.txt");
      urlsBox.AddUri(uri);
      Assert.AreEqual(1, state.Project.SourceUrls.Count);
      Assert.AreEqual(uri, state.Project.SourceUrls[0]);
      AssertAllUrlsVisible();
    }

    [Ignore("Have no idea how to test it")]
    [Test]
    public void DragNDrop()
    {
    }

    public Uri TestUrl(int index)
    {
      return Utils.ParseUrl(string.Format(@"c:\data{0}.txt", index));
    }

    [Test]
    public void SelectUrl()
    {
      urlsBox.AddUri(TestUrl(0));
      urlsBox.AddUri(TestUrl(1));
      urlsBox.AddUri(TestUrl(2));
      AssertAllUrlsVisible();
      urlsBox.SelectUri(1);
      AssertAllUrlsVisible();
      Assert.AreEqual(TestUrl(1), state.BrowserUri);
      AssertAllUrlsVisible();
    }

    [Test]
    public void DeleteUrl()
    {
      AssertAllUrlsVisible();
      urlsBox.AddUri(TestUrl(0));
      urlsBox.AddUri(TestUrl(1));
      urlsBox.AddUri(TestUrl(2));
      AssertAllUrlsVisible();
      urlsBox.Delete(1);
      Assert.AreEqual(2, state.Project.SourceUrls.Count);
      Assert.AreEqual(TestUrl(0), state.Project.SourceUrls[0]);
      Assert.AreEqual(TestUrl(2), state.Project.SourceUrls[1]);
    }
  }
}
