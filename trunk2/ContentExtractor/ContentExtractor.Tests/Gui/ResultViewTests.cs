using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using ContentExtractor.Gui;
using System.IO;
using ContentExtractor.Core;

namespace ContentExtractorTests.Gui
{
  [TestFixture]
  class ResultViewTests
  {
    private State state;
    private ResultsView resultView;
    private const string filename = "test.html";
    private readonly DocPosition pos = new DocPosition(Utils.ParseUrl(Path.GetFullPath(filename)));

    [SetUp]
    public void SetUp()
    {
      state = new State();
      resultView = new ResultsView();
      resultView.SetState(state);
    }
    
    [TearDown]
    public void TearDown()
    {
      if (File.Exists(filename))
        File.Delete(filename);
    }

    [Test]
    public void TestCase()
    {
      File.WriteAllText(filename, "<html><body><ol><li>1<li>2<li>3</ol></body></html>");
      state.Project.SourcePositions.Add(pos);
      state.Project.Template.RowXPath = "/html/body/ol/li";
      state.Project.Template.Columns.Add(new Column("text()"));
      // “ест получаетс€ не надежный, т.к. парсинг и загрузка могут происходить асинхронно.
    }
  }
}
