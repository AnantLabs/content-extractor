using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MetaTech.Library;
using System.Reflection;

namespace ContentExtractor.Core
{
  public interface IView
  {
    void InitModel(Getter<Model> modelGetter);
    void ForceSynchronize();
  }
}
