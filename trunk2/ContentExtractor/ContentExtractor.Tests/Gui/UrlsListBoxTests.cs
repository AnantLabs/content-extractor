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
      Assert.AreEqual(state.Project.SourcePositions.Count, box.Items.Count);
    }

    private ListBox ListBox
    {
      get
      {
        FieldInfo field = typeof(UrlsListBox).GetField(
          "listBox1",
          BindingFlags.NonPublic | BindingFlags.Instance);
        ListBox box = (ListBox)field.GetValue(urlsBox);
        return box;
      }
    }

    [Test]
    public void T001_AddedItemsAreShown()
    {
      state.Project.SourcePositions.Add(new DocPosition(@"C:\test1.html"));
      state.Project.SourcePositions.Add(new DocPosition(@"C:\test2.html"));
      urlsBox.ForceSynchronize();
      Assert.AreEqual(2, ListBox.Items.Count);
    }

    [Test]
    public void T002_AutoSelectionForcesBrowse()
    {
      state.Project.SourcePositions.Add(new DocPosition(@"C:\test1.html"));
      state.Project.SourcePositions.Add(new DocPosition(@"C:\test2.html"));
      urlsBox.ForceSynchronize();
      Assert.AreEqual(0, ListBox.SelectedIndex);
      Assert.AreEqual(state.Project.SourcePositions[0], state.BrowserPosition);
    }

    [Test]
    public void T003_DeletionForceBrowse()
    {
      state.Project.SourcePositions.Add(new DocPosition(@"C:\test1.html"));
      state.Project.SourcePositions.Add(new DocPosition(@"C:\test2.html"));
      state.Project.SourcePositions.Add(new DocPosition(@"C:\test3.html"));
      
      urlsBox.ForceSynchronize();
      Assert.AreEqual(state.Project.SourcePositions[0], state.BrowserPosition);
      
      ListBox.SelectedIndex = 1;
      urlsBox.ForceSynchronize();
      Assert.AreEqual(state.Project.SourcePositions[1], state.BrowserPosition);

      state.Project.SourcePositions.RemoveAt(1);
      urlsBox.ForceSynchronize();
      Assert.AreEqual(state.Project.SourcePositions[1], state.BrowserPosition);
    }

    [Test]
    public void T004()
    {
      state.Project.SourcePositions.Add(new DocPosition(@"C:\test1.html"));
      urlsBox.ForceSynchronize();

      state.Project.SourcePositions.RemoveAt(0);
      urlsBox.ForceSynchronize();

      DocPosition lastPosition = new DocPosition(@"C:\test2.html");
      state.Project.SourcePositions.Add(lastPosition);
      urlsBox.ForceSynchronize();

      Assert.AreEqual(lastPosition, state.BrowserPosition);
    }
  
  }
}
