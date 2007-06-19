using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Core;
using MetaTech.Library;
using System.IO;

namespace WebExtractor_Testing.Gui
{
  [TestFixture]
  public class UndoRedoTests
  {
    [SetUp]
    public void SetUp()
    {
      if (File.Exists(ApplicationHlp.MapPath("last.cex")))
        File.Delete(ApplicationHlp.MapPath("last.cex"));
      model = new Model();
      manager = new ModelStateManager(delegate { return new Model(); }, delegate(Model m) { });
      manager.InitModel(delegate { return model; });
    }
    Model model;
    ModelStateManager manager;


    [TearDown]
    public void TearDown()
    {
      if (File.Exists(ApplicationHlp.MapPath("last.cex")))
        File.Delete(ApplicationHlp.MapPath("last.cex"));
    }

    [Test]
    public void InitStoresOneModel()
    {
      manager.ForceSynchronize();
      Assert.AreEqual(1, manager.Count);
    }

    [Test]
    public void ChangingSelectedNodeSaveOneModel()
    {
      manager.ForceSynchronize();
      model.SelectedNodes[model.ActivePosition.Persist] = "/html[1]/body[1]";
      manager.ForceSynchronize();
      Assert.AreEqual(2, manager.Count);
    }

    [Test]
    public void UndoRedoReturnsBack()
    {
      manager.ForceSynchronize();
      model.SelectedNodes[model.ActivePosition.Persist] = "html[1]";
      manager.ForceSynchronize();
      Model top = (Model)model.Clone();

      manager.Undo();
      Assert.AreNotEqual(top, model);
      manager.Redo();
      Assert.AreEqual(top, model);
    }

    [Test]
    public void DoubleForceSynchronizeDontAddNewRecord()
    {
      manager.ForceSynchronize();
      manager.ForceSynchronize();
      Assert.AreEqual(1, manager.Count);
    }

    [Test]
    public void ChangingCurrentPositionMakesOnlyOneUndoSave()
    {
      manager.ForceSynchronize();
      model.ActivePosition = new WebPosition(new Uri("http://www.yandex.ru"));
      manager.ForceSynchronize();
      Assert.AreEqual(2, manager.Count);
    }
  }
}
