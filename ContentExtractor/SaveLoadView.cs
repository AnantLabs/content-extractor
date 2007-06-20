using System;
using System.Collections.Generic;
using System.Text;
using MetaTech.Library;
using System.IO;

namespace ContentExtractor.Core
{
  public class ModelStateManager : IView
  {
    Getter<Model> GetModel = delegate { return new Model(); };

    private List<Model> cachedModel = new List<Model>();
    private int index = -1;

    private Model CurrentModel
    {
      get
      {
        if (index == -1)
          return null;
        else
          return cachedModel[index];
      }
    }

    public ModelStateManager(Getter<Model> loader, Executter<Model> saver)
    {
      this.loader = loader;
      this.saver = saver;
    }
    private Getter<Model> loader;
    private Executter<Model> saver;

    public void InitModel(Getter<Model> modelGetter)
    {
      GetModel = modelGetter;
      GetModel().Load(loader());
      ForceSynchronize();
    }

    public void ForceSynchronize()
    {
      if (!Model.Equals(GetModel(), CurrentModel))
      {
        PushModel();
      }
    }

    private void PushModel()
    {
      cachedModel.RemoveRange(index + 1, cachedModel.Count - (index + 1));
      Model model = (Model)GetModel().Clone();
      cachedModel.Add(model);
      saver(model);
      //model.SaveToFile(ApplicationHlp.MapPath("last.cex"));
      index++;
    }

    public void Undo()
    {
      if (UndoAvailable)
      {
        index--;
        GetModel().Load(CurrentModel);
      }
    }

    public void Redo()
    {
      if (RedoAvailable)
      {
        index++;
        GetModel().Load(CurrentModel);
      }
    }

    public bool UndoAvailable
    {
      get
      {
        return index > 0;
      }
    }

    public bool RedoAvailable
    {
      get
      {
        return index < cachedModel.Count - 1;
      }
    }

    public int Count
    {
      get { return cachedModel.Count; }
    }

  }
}
